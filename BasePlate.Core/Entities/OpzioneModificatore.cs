namespace BasePlate.Core.Entities;

public class OpzioneModificatore : TenantEntity
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;

    // Se la cottura "In Pala" costa 2€ in più, lo segni qui.
    public decimal Sovrapprezzo { get; set; } = 0m;

    // Relazione col Padre (Il Gruppo)
    public Guid GruppoModificatoreId { get; set; }
    public GruppoModificatore Gruppo { get; set; } = null!;
}