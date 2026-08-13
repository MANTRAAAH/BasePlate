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
    // ==========================================
    // PUT: api/prodotti/{id} (MODIFICA)
    // ==========================================
    // ==========================================
    // PUT: api/prodotti/{id} (MODIFICA)
    // ==========================================
    [HttpPut("{id}")]
    public async Task<IActionResult> ModificaProdotto(int id, [FromBody] CreaProdottoDto request)
    {
        var prodotto = await _context.Prodotti
            .Include(p => p.Allergeni)
            .Include(p => p.Ingredienti)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (prodotto == null)
            return NotFound(new { Message = "Prodotto non trovato" });

        // 1. Aggiorniamo i dati base
        prodotto.Nome = request.Nome;
        prodotto.Descrizione = request.Descrizione;
        prodotto.Prezzo = request.Prezzo;
        prodotto.CategoriaId = request.CategoriaId;
        prodotto.ImmagineUrl = request.ImmagineUrl;

        // Inizializziamo le liste vuote se per caso arrivano null dal frontend
        request.IngredientiIds ??= new List<int>();
        request.AllergeniIds ??= new List<int>();

        // -----------------------------------------------------
        // 2. SINCRONIZZAZIONE INTELLIGENTE INGREDIENTI
        // -----------------------------------------------------
        // A. Trova e rimuovi gli ingredienti deselezionati
        var ingredientiDaRimuovere = prodotto.Ingredienti
            .Where(i => !request.IngredientiIds.Contains(i.IngredienteId))
            .ToList();
        foreach (var r in ingredientiDaRimuovere)
            prodotto.Ingredienti.Remove(r);

        // B. Trova e aggiungi SOLO i nuovi ingredienti selezionati
        var ingredientiAttuali = prodotto.Ingredienti.Select(i => i.IngredienteId).ToList();
        var ingredientiDaAggiungere = request.IngredientiIds
            .Where(id => !ingredientiAttuali.Contains(id))
            .ToList();
        foreach (var idNuovo in ingredientiDaAggiungere)
            prodotto.Ingredienti.Add(new ProdottoIngrediente { IngredienteId = idNuovo });


        // -----------------------------------------------------
        // 3. SINCRONIZZAZIONE INTELLIGENTE ALLERGENI
        // -----------------------------------------------------
        // A. Trova e rimuovi gli allergeni deselezionati
        var allergeniDaRimuovere = prodotto.Allergeni
            .Where(a => !request.AllergeniIds.Contains(a.AllergeneId))
            .ToList();
        foreach (var r in allergeniDaRimuovere)
            prodotto.Allergeni.Remove(r);

        // B. Trova e aggiungi SOLO i nuovi allergeni selezionati
        var allergeniAttuali = prodotto.Allergeni.Select(a => a.AllergeneId).ToList();
        var allergeniDaAggiungere = request.AllergeniIds
            .Where(id => !allergeniAttuali.Contains(id))
            .ToList();
        foreach (var idNuovo in allergeniDaAggiungere)
            prodotto.Allergeni.Add(new ProdottoAllergene { AllergeneId = idNuovo });


        // 4. Salvataggio finale
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