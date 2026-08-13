namespace BasePlate.Core.Entities;



public class ProdottoIngrediente
{
    public int ProdottoId { get; set; }
    public Prodotto Prodotto { get; set; } = null!;

    public int IngredienteId { get; set; }
    public Ingrediente Ingrediente { get; set; } = null!;
}