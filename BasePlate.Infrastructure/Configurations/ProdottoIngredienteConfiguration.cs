using BasePlate.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasePlate.Infrastructure.Configurations;

public class ProdottoIngredienteConfiguration : IEntityTypeConfiguration<ProdottoIngrediente>
{
    public void Configure(EntityTypeBuilder<ProdottoIngrediente> builder)
    {
        // 1. Diciamo a EF Core che la chiave primaria è composta
        builder.HasKey(pi => new { pi.ProdottoId, pi.IngredienteId });

        // 2. Blindiamo la relazione verso Prodotto
        builder.HasOne(pi => pi.Prodotto)
               .WithMany(p => p.Ingredienti)
               .HasForeignKey(pi => pi.ProdottoId);

        // 3. Blindiamo la relazione verso Ingrediente (evita colonne "ombra" create per sbaglio)
        builder.HasOne(pi => pi.Ingrediente)
               .WithMany() // Lascialo vuoto se in Ingrediente.cs non hai messo una lista di ritorno
               .HasForeignKey(pi => pi.IngredienteId);
    }
}