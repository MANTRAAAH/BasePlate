namespace BasePlate.Core.Entities;

public class Ingrediente : TenantEntity
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;

    // Relazione molti-a-molti con i prodotti (tramite tabella ponte in EF Core 5+)
    public ICollection<Prodotto> Prodotti { get; set; } = new List<Prodotto>();
    // Relazione molti-a-molti con gli allergeni
    public ICollection<Allergene> Allergeni { get; set; } = new List<Allergene>();
}