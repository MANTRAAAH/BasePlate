namespace BasePlate.API.DTOs;

public class CreateTenantRequest
{
    // Step 1: Identità & Brand Locale
    public string NomeRistorante { get; set; } = string.Empty;
    public string PartitaIva { get; set; } = string.Empty;
    public string DominioPersonalizzato { get; set; } = string.Empty;
    public string ColorePrimario { get; set; } = "#F59E0B";
    public string TemaLayout { get; set; } = "tema-liquid-glass";

    // Step 2: Account Root / Titolare
    public string NomeManager { get; set; } = string.Empty;
    public string EmailManager { get; set; } = string.Empty;
    public string PasswordManager { get; set; } = string.Empty;

    // Step 3: Moduli & Impostazioni
    public bool IsCoverChargeEnabled { get; set; } = true;
    public decimal CoverChargePrice { get; set; } = 2.00m;
    public string Timezone { get; set; } = "Europe/Rome";
}