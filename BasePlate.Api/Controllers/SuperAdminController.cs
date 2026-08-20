using BasePlate.API.DTOs;
using BasePlate.Core.Entities;
using BasePlate.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace BasePlate.API.Controllers;

[ApiController]
[Route("api/master/tenants")]
public class SuperAdminController : ControllerBase
{
    private readonly BasePlateDbContext _context;

    public SuperAdminController(BasePlateDbContext context)
    {
        _context = context;
    }

    [HttpPost("provision")]
    public async Task<IActionResult> ProvisionNewTenant([FromBody] CreateTenantRequest request)
    {
        // 1. Generiamo il nuovo ID univoco per il ristorante (Il TenantId assoluto)
        var newTenantId = Guid.NewGuid();

        // 2. Creiamo il record anagrafico del ristorante (Tabella Master)
        // Presupponendo che la tua classe Ristorante abbia queste proprietà base
        var nuovoRistorante = new Ristorante
        {
            Id = newTenantId,
            Nome = request.NomeRistorante,
            PartitaIva = request.PartitaIva,
            DataCreazione = DateTime.UtcNow,
            IsActive = true,
            // Lasciamo i valori di default per il frontend, se non li passiamo nel DTO
            TemaLayout = "tema-liquid-glass",
            ColorePrimario = "#F59E0B"
        };

        // 3. Creiamo le impostazioni di default per questo ristorante
        var defaultSettings = new TenantSettings
        {
            TenantId = newTenantId,
            IsCoverChargeEnabled = true,
            CoverChargePrice = 2.00m // Default standard per l'Italia
        };

        // 4. Inseriamo tutto nel DbContext
        _context.Ristoranti.Add(nuovoRistorante);
        _context.TenantSettings.Add(defaultSettings);

        // 5. Salviamo su PostgreSQL in un'unica transazione
        await _context.SaveChangesAsync();

        // 6. Restituiamo il TenantId. Da questo momento in poi, ogni chiamata 
        // per questo cliente dovrà avere l'header "X-Tenant-Id" impostato con questo Guid.
        return Ok(new
        {
            Message = "Provisioning completato con successo!",
            TenantId = newTenantId,
            Nome = request.NomeRistorante
        });
    }
}