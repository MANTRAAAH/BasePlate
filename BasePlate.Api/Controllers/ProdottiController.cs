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

    // GET: api/prodotti/lookup
    // Questo endpoint serve al form del frontend per caricare i menu a tendina e le checkbox
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
    // ==========================================
    // GET: api/prodotti/{id} (LETTURA SINGOLA PER MODIFICA)
    // ==========================================
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProdottoSingolo(int id)
    {
        var prodotto = await _context.Prodotti
            .Include(p => p.Allergeni)
            .Include(p => p.Ingredienti)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (prodotto == null) return NotFound();

        // Restituiamo un oggetto disegnato esattamente per riempire il Form di Angular
        return Ok(new
        {
            id = prodotto.Id,
            nome = prodotto.Nome,
            descrizione = prodotto.Descrizione,
            prezzo = prodotto.Prezzo,
            categoriaId = prodotto.CategoriaId,
            immagineUrl = prodotto.ImmagineUrl,
            // Estraiamo solo gli ID delle relazioni molti-a-molti!
            ingredientiIds = prodotto.Ingredienti.Select(i => i.IngredienteId).ToList(),
            allergeniIds = prodotto.Allergeni.Select(a => a.AllergeneId).ToList()
        });
    }
    // ==========================================
    // PUT: api/prodotti/{id} (MODIFICA)
    // ==========================================
    [HttpPut("{id}")]
    public async Task<IActionResult> ModificaProdotto(int id, [FromBody] CreaProdottoDto request)
    {
        // 1. Cerchiamo il prodotto includendo le sue liste attuali di ingredienti e allergeni
        var prodotto = await _context.Prodotti
            .Include(p => p.Allergeni)
            .Include(p => p.Ingredienti)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (prodotto == null)
            return NotFound(new { Message = "Prodotto non trovato" });

        // 2. Aggiorniamo i dati base
        prodotto.Nome = request.Nome;
        prodotto.Descrizione = request.Descrizione;
        prodotto.Prezzo = request.Prezzo;
        prodotto.CategoriaId = request.CategoriaId;
        prodotto.ImmagineUrl = request.ImmagineUrl;

        // 3. Il trucco per aggiornare le relazioni molti-a-molti: 
        // Svuotiamo le liste attuali e le ricreiamo con i nuovi dati!
        prodotto.Allergeni.Clear();
        if (request.AllergeniIds != null && request.AllergeniIds.Any())
        {
            prodotto.Allergeni = request.AllergeniIds
                .Select(allId => new ProdottoAllergene { AllergeneId = allId }).ToList();
        }

        prodotto.Ingredienti.Clear();
        if (request.IngredientiIds != null && request.IngredientiIds.Any())
        {
            prodotto.Ingredienti = request.IngredientiIds
                .Select(ingId => new ProdottoIngrediente { IngredienteId = ingId }).ToList();
        }

        // 4. Salviamo su database
        await _context.SaveChangesAsync();
        return Ok(new { Message = "Prodotto aggiornato con successo!" });
    }

    // ==========================================
    // DELETE: api/prodotti/{id} (ELIMINA)
    // ==========================================
    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminaProdotto(int id)
    {
        var prodotto = await _context.Prodotti.FindAsync(id);

        if (prodotto == null)
            return NotFound(new { Message = "Prodotto non trovato" });

        // Entity Framework è intelligente: eliminando il prodotto, eliminerà automaticamente
        // anche i record di collegamento nella tabella ProdottiAllergeni e ProdottiIngredienti.
        _context.Prodotti.Remove(prodotto);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Prodotto eliminato con successo!" });
    }
}