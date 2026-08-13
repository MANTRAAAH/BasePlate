using BasePlate.Core.DTOs; // Assicurati che il namespace corrisponda a dove hai messo CreaProdottoDto
using BasePlate.Core.Entities;
using BasePlate.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BasePlate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdottiController : ControllerBase
{
    private readonly BasePlateDbContext _context;

    public ProdottiController(BasePlateDbContext context)
    {
        _context = context;
    }

    // GET: api/prodotti
    [HttpGet]
    public async Task<IActionResult> GetProdotti()
    {
        // 🛡️ Mappatura complessa: Peschiamo il prodotto e le sue relazioni.
        // Il filtro Multi-Tenant farà in modo che tu veda solo i prodotti del tuo locale.
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
                ImmagineUrl = p.ImmagineUrl, // 👈 ECCO LA RIGA CHE MANCAVA!
                NomeCategoria = p.Categoria != null ? p.Categoria.Nome : "Nessuna",

                // Estraiamo solo i nomi per alleggerire la risposta al frontend
                Allergeni = p.Allergeni.Select(pa => pa.Allergene.Nome).ToList(),
                Ingredienti = p.Ingredienti.Select(pi => pi.Ingrediente.Nome).ToList()
            })
            .ToListAsync();

        return Ok(prodotti);
    }

    // POST: api/prodotti
    [HttpPost]
    public async Task<IActionResult> CreaProdotto([FromBody] CreaProdottoDto request)
    {
        // 1. Creiamo l'entità principale
        var nuovoProdotto = new Prodotto
        {
            Nome = request.Nome,
            Descrizione = request.Descrizione,
            Prezzo = request.Prezzo,
            CategoriaId = request.CategoriaId,
            ImmagineUrl = request.ImmagineUrl,
            // 🛡️ Il TenantId viene sempre gestito in automatico!
        };

        // 2. Mappiamo le relazioni molti-a-molti (Allergeni)
        if (request.AllergeniIds != null && request.AllergeniIds.Any())
        {
            nuovoProdotto.Allergeni = request.AllergeniIds
                .Select(id => new ProdottoAllergene { AllergeneId = id })
                .ToList();
        }

        // 3. Mappiamo le relazioni molti-a-molti (Ingredienti)
        if (request.IngredientiIds != null && request.IngredientiIds.Any())
        {
            nuovoProdotto.Ingredienti = request.IngredientiIds
                .Select(id => new ProdottoIngrediente { IngredienteId = id })
                .ToList();
        }

        // 4. Salvataggio transazionale (Salva prodotto e relazioni in un colpo solo)
        _context.Prodotti.Add(nuovoProdotto);
        await _context.SaveChangesAsync();

        // 5. Risposta
        return Ok(new { Message = "Prodotto creato con successo!", ProdottoId = nuovoProdotto.Id });
    }
}