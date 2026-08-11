namespace BasePlate.Core.Entities;

public class TipologiaCottura : TenantEntity
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descrizione { get; set; } = string.Empty;

    // Navigation property: una cottura si applica a molti prodotti
    public ICollection<Prodotto> Prodotti { get; set; } = new List<Prodotto>();
}