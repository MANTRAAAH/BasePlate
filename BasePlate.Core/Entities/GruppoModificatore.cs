namespace BasePlate.Core.Entities;

public class GruppoModificatore : TenantEntity
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;

    // Questa è la nota formativa che legge SOLO il cameriere sull'app
    public string InternalDescription { get; set; } = string.Empty;

    // Le regole di validazione per bloccare/sbloccare il carrello
    public int MinSelections { get; set; } = 0;
    public int MaxSelections { get; set; } = 1;

    // La lista delle opzioni disponibili in questo gruppo
    public ICollection<OpzioneModificatore> Opzioni { get; set; } = new List<OpzioneModificatore>();

    // Relazione molti-a-molti coi prodotti
    public ICollection<ProdottoGruppoModificatore> ProdottiAssociati { get; set; } = new List<ProdottoGruppoModificatore>();
}