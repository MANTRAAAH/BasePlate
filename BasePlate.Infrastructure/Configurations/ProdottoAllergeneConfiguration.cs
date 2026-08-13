using BasePlate.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasePlate.Infrastructure.Configurations; // Adatta il namespace alla tua cartella

public class ProdottoAllergeneConfiguration : IEntityTypeConfiguration<ProdottoAllergene>
{
    public void Configure(EntityTypeBuilder<ProdottoAllergene> builder)
    {
        // Diciamo a EF Core che la chiave primaria è composta da questi due ID
        builder.HasKey(pa => new { pa.ProdottoId, pa.AllergeneId });
    }
}