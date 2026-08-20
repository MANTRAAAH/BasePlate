namespace BasePlate.API.DTOs;

public class CreateTenantRequest
{
    public string NomeRistorante { get; set; } = string.Empty;
    public string PartitaIva { get; set; } = string.Empty;
    public string EmailManager { get; set; } = string.Empty;
    // Qui in futuro potrai aggiungere il piano scelto: "Pro", "Enterprise", ecc.
}