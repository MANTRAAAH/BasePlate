using BasePlate.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasePlate.Infrastructure.Configurations; // Adatta il namespace

public class ProdottoIngredienteConfiguration : IEntityTypeConfiguration<ProdottoIngrediente>
{
    public void Configure(EntityTypeBuilder<ProdottoIngrediente> builder)
    {
        // Diciamo a EF Core che la chiave primaria è composta
        builder.HasKey(pi => new { pi.ProdottoId, pi.IngredienteId });
    }
}