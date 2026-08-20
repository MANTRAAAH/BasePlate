namespace BasePlate.Core.Entities;

public class Prodotto : TenantEntity
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Prezzo { get; set; }

    // NUOVI CAMPI AGGIUNTI:
    public string Descrizione { get; set; } = string.Empty;
    public string? ImmagineUrl { get; set; } // Opzionale (può essere null)

    // Relazione Uno-a-Molti con Categoria
    public int CategoriaId { get; set; }
    public Categoria Categoria { get; set; } = null!;

    // NUOVE RELAZIONI: Navigazione verso le tabelle ponte
    // Attenzione: deve essere ICollection<ProdottoAllergene> e non <Allergene>
    public ICollection<ProdottoAllergene> Allergeni { get; set; } = new List<ProdottoAllergene>();

    // Attenzione: deve essere ICollection<ProdottoIngrediente> e non <Ingrediente>
    public ICollection<ProdottoIngrediente> Ingredienti { get; set; } = new List<ProdottoIngrediente>();
    public ICollection<ProdottoGruppoModificatore> GruppiModificatori { get; set; } = new List<ProdottoGruppoModificatore>();
}