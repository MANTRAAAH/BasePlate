using BasePlate.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasePlate.Infrastructure.Data.Configurations;

public class ProdottoGruppoModificatoreConfig : IEntityTypeConfiguration<ProdottoGruppoModificatore>
{
    public void Configure(EntityTypeBuilder<ProdottoGruppoModificatore> builder)
    {
        // 1. Evita duplicati: una pizza non può avere lo stesso gruppo due volte
        builder.HasIndex(pgm => new { pgm.ProdottoId, pgm.GruppoModificatoreId }).IsUnique();

        // 2. Relazione con Prodotto (Delete a cascata)
        builder.HasOne(pgm => pgm.Prodotto)
            .WithMany(p => p.GruppiModificatori)
            .HasForeignKey(pgm => pgm.ProdottoId)
            .OnDelete(DeleteBehavior.Cascade);

        // 3. Relazione con GruppoModificatore (Delete a cascata)
        builder.HasOne(pgm => pgm.GruppoModificatore)
            .WithMany(g => g.ProdottiAssociati)
            .HasForeignKey(pgm => pgm.GruppoModificatoreId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}