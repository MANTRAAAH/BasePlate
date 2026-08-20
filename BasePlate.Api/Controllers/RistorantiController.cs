using Microsoft.AspNetCore.Mvc;
using BasePlate.Core.Entities;
using BasePlate.Core.DTOs.Ristorante;
using BasePlate.Infrastructure.Data;

namespace BasePlate.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RistorantiController : ControllerBase
{
    private readonly BasePlateDbContext _context;

    public RistorantiController(BasePlateDbContext context)
    {
        _context = context;
    }

    [HttpPost("setup-nuovo-cliente")]
    public async Task<IActionResult> CreaRistorante([FromBody] CreaRistoranteDto request)
    {
        // 1. Inizializziamo il nuovo Tenant (Ristorante)
        var nuovoRistorante = new Ristorante
        {
            Id = Guid.NewGuid(), // 🔑 ECCOLO! Generiamo il TenantId univoco
            Nome = request.Nome,
            PartitaIva = request.PartitaIva,
            DominioPersonalizzato = request.DominioPersonalizzato,
            // Impostiamo dei valori di default per il frontend Angular
            TemaLayout = "Light",
            ColorePrimario = "#e63946", // Rosso pizzeria di default
            LogoUrl = string.Empty
        };

        // 2. Salviamo nel database
        _context.Ristoranti.Add(nuovoRistorante);
        await _context.SaveChangesAsync();

        // 3. Restituiamo il TenantId generato
        return Ok(new
        {
            Messaggio = "Nuovo Tenant creato con successo nell'ecosistema SaaS!",
            Ristorante = nuovoRistorante.Nome,
            TenantId = nuovoRistorante.Id
        });
    }
}