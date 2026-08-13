namespace BasePlate.Core.Entities;

public class ProdottoAllergene
{
    public int ProdottoId { get; set; }
    public Prodotto Prodotto { get; set; } = null!;

    public int AllergeneId { get; set; }
    public Allergene Allergene { get; set; } = null!;
}