using BasePlate.Core.DTOs;
using BasePlate.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using BasePlate.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BasePlate.Api.Controllers;

[Authorize(Roles = "Admin")] // 👈 IL LUCCHETTO PRINCIPALE RESTA
[ApiController]
[Route("api/[controller]")]
public class ProdottiController : ControllerBase
{
    private readonly BasePlateDbContext _context;

    public ProdottiController(BasePlateDbContext context)
    {
        _context = context;
    }

    // ==========================================
    // GET: api/prodotti (PUBBLICO - Vetrina)
    // ==========================================
    [AllowAnonymous] // 👈 SBLOCCO CHIRURGICO PER IL PUBBLICO
    [HttpGet]
    public async Task<IActionResult> GetProdotti()
    {
        var prodotti = await _context.Prodotti
            .Include(p => p.Categoria)
            .Include(p => p.Allergeni).ThenInclude(pa => pa.Allergene)
            .Include(p => p.Ingredienti).ThenInclude(pi => pi.Ingrediente)
            .Select(p => new ProdottoReadDto
            {
                Id = p.Id,
                Nome = p.Nome,
                Descrizione = p.Descrizione,
                Prezzo = p.Prezzo,
                ImmagineUrl = p.ImmagineUrl,
                NomeCategoria = p.Categoria != null ? p.Categoria.Nome : "Nessuna",
                Allergeni = p.Allergeni.Select(pa => pa.Allergene.Nome).ToList(),
                Ingredienti = p.Ingredienti.Select(pi => pi.Ingrediente.Nome).ToList()
            })
            .ToListAsync();

        return Ok(prodotti);
    }

    // ==========================================
    // GET: api/prodotti/lookup (PUBBLICO - Vetrina/Filtri)
    // ==========================================
    [AllowAnonymous] // 👈 SBLOCCO CHIRURGICO
    [HttpGet("lookup")]
    public async Task<IActionResult> GetLookupData()
    {
        var categorie = await _context.Categorie.Select(c => new { c.Id, c.Nome }).ToListAsync();
        var allergeni = await _context.Allergeni.Select(a => new { a.Id, a.Nome }).ToListAsync();
        var ingredienti = await _context.Ingredienti.Select(i => new { i.Id, i.Nome }).ToListAsync();

        return Ok(new
        {
            Categorie = categorie,
            Allergeni = allergeni,
            Ingredienti = ingredienti
        });
    }

    // ==========================================
    // GET: api/prodotti/{id} (PUBBLICO - Dettaglio Vetrina)
    // ==========================================
    [AllowAnonymous] // 👈 SBLOCCO CHIRURGICO
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProdottoSingolo(int id)
    {
        var prodotto = await _context.Prodotti
            .Include(p => p.Allergeni)
            .Include(p => p.Ingredienti)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (prodotto == null) return NotFound();

        return Ok(new
        {
            id = prodotto.Id,
            nome = prodotto.Nome,
            descrizione = prodotto.Descrizione,
            prezzo = prodotto.Prezzo,
            categoriaId = prodotto.CategoriaId,
            immagineUrl = prodotto.ImmagineUrl,
            ingredientiIds = prodotto.Ingredienti.Select(i => i.IngredienteId).ToList(),
            allergeniIds = prodotto.Allergeni.Select(a => a.AllergeneId).ToList()
        });
    }

    // ==========================================
    // POST: api/prodotti (PROTETTO - Solo Admin)
    // ==========================================
    [HttpPost]
    public async Task<IActionResult> CreaProdotto([FromBody] CreaProdottoDto request)
    {
        // ... (Il tuo codice originale resta invariato, è perfetto)
        var nuovoProdotto = new Prodotto
        {
            Nome = request.Nome,
            Descrizione = request.Descrizione,
            Prezzo = request.Prezzo,
            CategoriaId = request.CategoriaId,
            ImmagineUrl = request.ImmagineUrl,
        };

        if (request.AllergeniIds != null && request.AllergeniIds.Any())
        {
            nuovoProdotto.Allergeni = request.AllergeniIds
                .Select(id => new ProdottoAllergene { AllergeneId = id }).ToList();
        }

        if (request.IngredientiIds != null && request.IngredientiIds.Any())
        {
            nuovoProdotto.Ingredienti = request.IngredientiIds
                .Select(id => new ProdottoIngrediente { IngredienteId = id }).ToList();
        }

        _context.Prodotti.Add(nuovoProdotto);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Prodotto creato con successo!", ProdottoId = nuovoProdotto.Id });
    }

    // ==========================================
    // PUT: api/prodotti/{id} (PROTETTO - Solo Admin)
    // ==========================================
    [HttpPut("{id}")]
    public async Task<IActionResult> ModificaProdotto(int id, [FromBody] CreaProdottoDto request)
    {
        // ... (Il tuo codice originale resta invariato)
        var prodotto = await _context.Prodotti
            .Include(p => p.Allergeni)
            .Include(p => p.Ingredienti)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (prodotto == null) return NotFound(new { Message = "Prodotto non trovato" });

        prodotto.Nome = request.Nome;
        prodotto.Descrizione = request.Descrizione;
        prodotto.Prezzo = request.Prezzo;
        prodotto.CategoriaId = request.CategoriaId;
        prodotto.ImmagineUrl = request.ImmagineUrl;

        request.IngredientiIds ??= new List<int>();
        request.AllergeniIds ??= new List<int>();

        var ingredientiDaRimuovere = prodotto.Ingredienti.Where(i => !request.IngredientiIds.Contains(i.IngredienteId)).ToList();
        foreach (var r in ingredientiDaRimuovere) prodotto.Ingredienti.Remove(r);

        var ingredientiAttuali = prodotto.Ingredienti.Select(i => i.IngredienteId).ToList();
        var ingredientiDaAggiungere = request.IngredientiIds.Where(id => !ingredientiAttuali.Contains(id)).ToList();
        foreach (var idNuovo in ingredientiDaAggiungere) prodotto.Ingredienti.Add(new ProdottoIngrediente { IngredienteId = idNuovo });

        var allergeniDaRimuovere = prodotto.Allergeni.Where(a => !request.AllergeniIds.Contains(a.AllergeneId)).ToList();
        foreach (var r in allergeniDaRimuovere) prodotto.Allergeni.Remove(r);

        var allergeniAttuali = prodotto.Allergeni.Select(a => a.AllergeneId).ToList();
        var allergeniDaAggiungere = request.AllergeniIds.Where(id => !allergeniAttuali.Contains(id)).ToList();
        foreach (var idNuovo in allergeniDaAggiungere) prodotto.Allergeni.Add(new ProdottoAllergene { AllergeneId = idNuovo });

        await _context.SaveChangesAsync();
        return Ok(new { Message = "Prodotto aggiornato con successo!" });
    }

    // ==========================================
    // DELETE: api/prodotti/{id} (PROTETTO - Solo Admin)
    // ==========================================
    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminaProdotto(int id)
    {
        // ... (Il tuo codice originale resta invariato)
        var prodotto = await _context.Prodotti.FindAsync(id);
        if (prodotto == null) return NotFound(new { Message = "Prodotto non trovato" });

        _context.Prodotti.Remove(prodotto);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Prodotto eliminato con successo!" });
    }
}