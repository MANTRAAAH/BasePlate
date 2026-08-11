namespace BasePlate.Core.Entities;

public class Prodotto : TenantEntity
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Prezzo { get; set; }

    // Chiavi Esterne
    public int CategoriaId { get; set; }
    public Categoria Categoria { get; set; } = null!;

    public int? TipologiaCotturaId { get; set; } // Nullable, non tutti i prodotti hanno una cottura specifica (es. una bibita)
    public TipologiaCottura? TipologiaCottura { get; set; }

    // Navigation property: un prodotto ha molti ingredienti
    public ICollection<Ingrediente> Ingredienti { get; set; } = new List<Ingrediente>();
}