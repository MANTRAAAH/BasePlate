using BasePlate.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasePlate.Infrastructure.Configurations;

public class ProdottoAllergeneConfiguration : IEntityTypeConfiguration<ProdottoAllergene>
{
    public void Configure(EntityTypeBuilder<ProdottoAllergene> builder)
    {
        // 1. Diciamo a EF Core che la chiave primaria è composta
        builder.HasKey(pa => new { pa.ProdottoId, pa.AllergeneId });

        // 2. Blindiamo la relazione verso Prodotto
        builder.HasOne(pa => pa.Prodotto)
               .WithMany(p => p.Allergeni) // Questo collega la ICollection che hai messo in Prodotto.cs
               .HasForeignKey(pa => pa.ProdottoId);

        // 3. Blindiamo la relazione verso Allergene (evita colonne ombra su Prodotti)
        builder.HasOne(pa => pa.Allergene)
               .WithMany() // Lascialo vuoto se in Allergene.cs non hai esplicitato la ICollection di ritorno
               .HasForeignKey(pa => pa.AllergeneId);
    }
}