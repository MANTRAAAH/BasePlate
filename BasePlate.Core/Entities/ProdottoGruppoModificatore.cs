namespace BasePlate.Core.Entities;

public class ProdottoGruppoModificatore : TenantEntity
{
    public Guid Id { get; set; }
    public int ProdottoId { get; set; }
    public Prodotto Prodotto { get; set; } = null!;

    public Guid GruppoModificatoreId { get; set; }
    public GruppoModificatore GruppoModificatore { get; set; } = null!;

    // Fondamentale: decide in che ordine il cameriere vede i gruppi sull'app 
    // (es. prima scegli l'impasto, poi le aggiunte)
    public int OrdineVisualizzazione { get; set; }
}