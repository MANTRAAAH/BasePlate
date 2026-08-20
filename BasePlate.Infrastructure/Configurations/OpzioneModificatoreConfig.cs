using BasePlate.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasePlate.Infrastructure.Data.Configurations;

public class OpzioneModificatoreConfig : IEntityTypeConfiguration<OpzioneModificatore>
{
    public void Configure(EntityTypeBuilder<OpzioneModificatore> builder)
    {
        // Se cancello il gruppo "Gradi di cottura", le opzioni "Al sangue" e "Media" devono sparire
        builder.HasOne(o => o.Gruppo)
            .WithMany(g => g.Opzioni)
            .HasForeignKey(o => o.GruppoModificatoreId)
            .OnDelete(DeleteBehavior.Cascade);

        // Forziamo il formato del prezzo nel database (es. 999.99€)
        builder.Property(o => o.Sovrapprezzo)
            .HasColumnType("decimal(18,2)");
    }
}