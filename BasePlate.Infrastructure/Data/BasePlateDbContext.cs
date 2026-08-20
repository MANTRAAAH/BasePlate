using BasePlate.Core.Entities;
using BasePlate.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BasePlate.Infrastructure.Data;

public class BasePlateDbContext : DbContext
{
    private readonly Guid _tenantId;

    public BasePlateDbContext(DbContextOptions<BasePlateDbContext> options, ITenantProvider tenantProvider)
        : base(options)
    {
        _tenantId = tenantProvider.GetTenantId();
    }

    // --- FASE 1: Master Data ---
    public DbSet<Prodotto> Prodotti { get; set; } = null!;
    public DbSet<Categoria> Categorie { get; set; } = null!;
    public DbSet<Ingrediente> Ingredienti { get; set; } = null!;
    public DbSet<Allergene> Allergeni { get; set; } = null!;
    // Le nuove tabelle del motore menu
    public DbSet<GruppoModificatore> GruppiModificatori { get; set; } = null!;
    public DbSet<OpzioneModificatore> OpzioniModificatore { get; set; } = null!;
    public DbSet<ProdottoGruppoModificatore> ProdottoGruppiModificatori { get; set; } = null!;

    /* // --- FASE 2: Logica Core & Ordini ---
     public DbSet<TenantSettings> TenantSettings { get; set; } = null!; // Gestione Coperto e Orari
     public DbSet<GruppoModificatore> GruppiModificatori { get; set; } = null!; // Sostituisce TipologiaCottura per essere più flessibile
     public DbSet<Ordine> Ordini { get; set; } = null!;
     public DbSet<RigaOrdine> RigheOrdine { get; set; } = null!;
 */
    // --- Amministrazione ---
    public DbSet<Utente> Utenti { get; set; } = null!;
    public DbSet<Ristorante> Ristoranti { get; set; } = null!; // La tabella master (Tenants)

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Applichiamo le configurazioni mappate (Fluent API)
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // 🛡️ SICUREZZA MULTI-TENANT: GLOBAL QUERY FILTERS (Corretto per la cache di EF Core)
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(TenantEntity).IsAssignableFrom(entityType.ClrType))
            {
                // Invochiamo il metodo generico ApplyTenantFilter per ogni entità trovata
                var method = typeof(BasePlateDbContext)
                    .GetMethod(nameof(ApplyTenantFilter), BindingFlags.NonPublic | BindingFlags.Instance)
                    ?.MakeGenericMethod(entityType.ClrType);

                method?.Invoke(this, new object[] { modelBuilder });
            }
        }
    }

    // Metodo helper generico: EF Core capisce questa sintassi e la valuta dinamicamente ad ogni query!
    private void ApplyTenantFilter<T>(ModelBuilder builder) where T : TenantEntity
    {
        // Se sei SuperAdmin (Guid.Empty) ignora il filtro, altrimenti controlla l'ID del Tenant.
        builder.Entity<T>().HasQueryFilter(e => _tenantId == Guid.Empty || e.TenantId == _tenantId);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        // Intercetta tutti i nuovi record in inserimento che ereditano da TenantEntity
        foreach (var entry in ChangeTracker.Entries<TenantEntity>().Where(e => e.State == EntityState.Added))
        {
            // Sovrascrivi in automatico SOLO se c'è un tenant reale loggato
            // Questo impedisce che l'utente inserisca un record per un altro ristorante
            if (_tenantId != Guid.Empty)
            {
                entry.Entity.TenantId = _tenantId;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}