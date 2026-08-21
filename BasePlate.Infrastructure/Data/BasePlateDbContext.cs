using BasePlate.Core.Entities;
using BasePlate.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BasePlate.Infrastructure.Data;

public class BasePlateDbContext : DbContext
{
    private readonly ITenantProvider _tenantProvider;

    // 🟢 LA MAGIA È QUI: Una proprietà dinamica (=>) che interroga il provider LIVE ad ogni query
    public Guid DynamicTenantId => _tenantProvider.GetTenantId();

    public BasePlateDbContext(DbContextOptions<BasePlateDbContext> options, ITenantProvider tenantProvider)
        : base(options)
    {
        _tenantProvider = tenantProvider;
    }

    public DbSet<TenantSettings> TenantSettings { get; set; } = null!;
    public DbSet<Prodotto> Prodotti { get; set; } = null!;
    public DbSet<Categoria> Categorie { get; set; } = null!;
    public DbSet<Ingrediente> Ingredienti { get; set; } = null!;
    public DbSet<Allergene> Allergeni { get; set; } = null!;
    public DbSet<GruppoModificatore> GruppiModificatori { get; set; } = null!;
    public DbSet<OpzioneModificatore> OpzioniModificatore { get; set; } = null!;
    public DbSet<ProdottoGruppoModificatore> ProdottoGruppiModificatori { get; set; } = null!;
    public DbSet<Utente> Utenti { get; set; } = null!;
    public DbSet<Ristorante> Ristoranti { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(TenantEntity).IsAssignableFrom(entityType.ClrType))
            {
                var method = typeof(BasePlateDbContext)
                    .GetMethod(nameof(ApplyTenantFilter), BindingFlags.NonPublic | BindingFlags.Instance)
                    ?.MakeGenericMethod(entityType.ClrType);

                method?.Invoke(this, new object[] { modelBuilder });
            }
        }
    }

    private void ApplyTenantFilter<T>(ModelBuilder builder) where T : TenantEntity
    {
        // 🟢 Usando la proprietà DynamicTenantId, EF Core parametrizza la query SQL
        builder.Entity<T>().HasQueryFilter(e => DynamicTenantId == Guid.Empty || e.TenantId == DynamicTenantId);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<TenantEntity>().Where(e => e.State == EntityState.Added))
        {
            // 🟢 Usiamo la proprietà dinamica anche in inserimento
            if (DynamicTenantId != Guid.Empty)
            {
                entry.Entity.TenantId = DynamicTenantId;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}