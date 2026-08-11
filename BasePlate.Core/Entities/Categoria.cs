namespace BasePlate.Core.Entities;

public class Categoria : TenantEntity
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;

    // Navigation property: una categoria ha molti prodotti
    public ICollection<Prodotto> Prodotti { get; set; } = new List<Prodotto>();
}