using BasePlate.API.DTOs;
using BasePlate.Core.Entities;
using Microsoft.EntityFrameworkCore;
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
        // 1. Controllo preliminare: l'email del manager deve essere univoca
        if (!string.IsNullOrWhiteSpace(request.EmailManager) && 
            await _context.Utenti.AnyAsync(u => u.Email == request.EmailManager))
        {
            return BadRequest(new { message = $"L'email '{request.EmailManager}' è già registrata nel sistema." });
        }

        // 2. Transazione atomica per garantire rollback automatico in caso di errore
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // 3. Generazione TenantId univoco
            var newTenantId = Guid.NewGuid();

            // 4. Creazione Record Ristorante (Master)
            var nuovoRistorante = new Ristorante
            {
                Id = newTenantId,
                Nome = request.NomeRistorante,
                PartitaIva = request.PartitaIva,
                DominioPersonalizzato = request.DominioPersonalizzato,
                TemaLayout = string.IsNullOrWhiteSpace(request.TemaLayout) ? "tema-liquid-glass" : request.TemaLayout,
                ColorePrimario = string.IsNullOrWhiteSpace(request.ColorePrimario) ? "#F59E0B" : request.ColorePrimario,
                DataCreazione = DateTime.UtcNow,
                IsActive = true
            };

            // 5. Creazione Impostazioni SaaS (TenantSettings)
            var defaultSettings = new TenantSettings
            {
                TenantId = newTenantId,
                IsCoverChargeEnabled = request.IsCoverChargeEnabled,
                CoverChargePrice = request.CoverChargePrice >= 0 ? request.CoverChargePrice : 2.00m,
                Timezone = string.IsNullOrWhiteSpace(request.Timezone) ? "Europe/Rome" : request.Timezone
            };

            _context.Ristoranti.Add(nuovoRistorante);
            _context.TenantSettings.Add(defaultSettings);

            // 6. Creazione Utente Root (Manager del locale)
            if (!string.IsNullOrWhiteSpace(request.EmailManager))
            {
                var password = string.IsNullOrWhiteSpace(request.PasswordManager) ? "Admin123!" : request.PasswordManager;
                var utenteRoot = new Utente
                {
                    TenantId = newTenantId,
                    Nome = string.IsNullOrWhiteSpace(request.NomeManager) ? "Titolare" : request.NomeManager,
                    Email = request.EmailManager,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                    Ruolo = "Admin"
                };

                _context.Utenti.Add(utenteRoot);
            }

            // 7. Salvataggio atomico su database
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok(new
            {
                Message = "Provisioning completato con successo!",
                TenantId = newTenantId,
                Nome = request.NomeRistorante,
                EmailManager = request.EmailManager
            });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, new { message = "Errore durante il provisioning del tenant", error = ex.Message });
        }
    }
    [HttpGet]
    public async Task<IActionResult> GetAllTenants()
    {
        // Recuperiamo tutti i ristoranti, ordinati dal più recente
        var tenants = await _context.Ristoranti
            .OrderByDescending(r => r.DataCreazione)
            .Select(r => new
            {
                r.Id,
                r.Nome,
                r.PartitaIva,
                r.DataCreazione,
                r.IsActive
            })
            .ToListAsync();

        return Ok(tenants);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTenant(Guid id, [FromBody] UpdateTenantRequest request)
    {
        // 1. Cerchiamo il ristorante nel database
        var tenant = await _context.Ristoranti.FindAsync(id);
        if (tenant == null)
            return NotFound(new { message = "Ristorante non trovato" });

        // 2. Aggiorniamo i campi
        tenant.Nome = request.NomeRistorante;
        tenant.PartitaIva = request.PartitaIva;
        tenant.IsActive = request.IsActive;
        // La data di creazione non si tocca!

        // 3. Salviamo sul DB
        await _context.SaveChangesAsync();

        return Ok(tenant);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTenant(Guid id)
    {
        var tenant = await _context.Ristoranti.FindAsync(id);
        if (tenant == null)
            return NotFound(new { message = "Ristorante non trovato" });

        // Apriamo una transazione: o si cancella TUTTO, o non si cancella niente
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // 🧨 DISTRUZIONE A CASCATA: Eliminiamo prima tutti i dati figli associati a questo TenantId
            // Usa IgnoreQueryFilters() per sicurezza, per bypassare eventuali filtri attivi.
            await _context.Prodotti.IgnoreQueryFilters().Where(p => p.TenantId == id).ExecuteDeleteAsync();
            await _context.Categorie.IgnoreQueryFilters().Where(c => c.TenantId == id).ExecuteDeleteAsync();
            await _context.Ingredienti.IgnoreQueryFilters().Where(i => i.TenantId == id).ExecuteDeleteAsync();
            await _context.Allergeni.IgnoreQueryFilters().Where(a => a.TenantId == id).ExecuteDeleteAsync();
            await _context.GruppiModificatori.IgnoreQueryFilters().Where(g => g.TenantId == id).ExecuteDeleteAsync();
            await _context.OpzioniModificatore.IgnoreQueryFilters().Where(o => o.TenantId == id).ExecuteDeleteAsync();
            await _context.ProdottoGruppiModificatori.IgnoreQueryFilters().Where(p => p.TenantId == id).ExecuteDeleteAsync();
            await _context.TenantSettings.IgnoreQueryFilters().Where(t => t.TenantId == id).ExecuteDeleteAsync();

            // Infine, eliminiamo la radice: il ristorante stesso
            _context.Ristoranti.Remove(tenant);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return Ok(new { message = "Tenant e dati associati eliminati con successo." });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, new { message = "Errore durante l'eliminazione", error = ex.Message });
        }
    }
}
public class UpdateTenantRequest
{
    public string NomeRistorante { get; set; } = string.Empty;
    public string PartitaIva { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
