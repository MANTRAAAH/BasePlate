using BasePlate.Core.Entities;
using BasePlate.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Linq.Expressions;

namespace BasePlate.Infrastructure.Data;

public class BasePlateDbContext : DbContext
{
    private readonly Guid _tenantId;

    public BasePlateDbContext(DbContextOptions<BasePlateDbContext> options, ITenantProvider tenantProvider)
        : base(options)
    {
        _tenantId = tenantProvider.GetTenantId();
    }

    // Le nostre tabelle nel database
    public DbSet<Prodotto> Prodotti { get; set; } = null!;
    public DbSet<Categoria> Categorie { get; set; } = null!;
    public DbSet<TipologiaCottura> TipologieCottura { get; set; } = null!;
    public DbSet<Ingrediente> Ingredienti { get; set; } = null!;
    public DbSet<Allergene> Allergeni { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Applichiamo la configurazione da un assembly separato (buona pratica per tenere pulito il db context)
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // 🛡️ SICUREZZA MULTI-TENANT: GLOBAL QUERY FILTERS
        // Questo ciclo itera su tutti i tipi entità definiti nel modello
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            // Se il tipo entità eredita da TenantEntity...
            if (typeof(TenantEntity).IsAssignableFrom(entityType.ClrType))
            {
                // ...applica un filtro globale: mostra solo i record dove il TenantId corrisponde a quello corrente.
                modelBuilder.Entity(entityType.ClrType)
                            .HasQueryFilter(ConvertFilterExpression(entityType.ClrType));
            }
        }
    }

    // Metodo helper per generare l'espressione lambda corretta per il filtro
    private LambdaExpression ConvertFilterExpression(Type type)
    {
        var param = System.Linq.Expressions.Expression.Parameter(type, "e");
        var property = System.Linq.Expressions.Expression.Property(param, nameof(TenantEntity.TenantId));
        var value = System.Linq.Expressions.Expression.Constant(_tenantId);
        var body = System.Linq.Expressions.Expression.Equal(property, value);

        return System.Linq.Expressions.Expression.Lambda(body, param);
    }

    // Assegnazione automatica del TenantId durante i salvataggi
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<TenantEntity>().Where(e => e.State == EntityState.Added))
        {
            entry.Entity.TenantId = _tenantId;
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}