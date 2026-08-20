namespace BasePlate.Core.Entities;

public class TenantSettings : TenantEntity
{
    public int Id { get; set; }

    // Flag per attivare/disattivare il calcolo automatico del coperto
    public bool IsCoverChargeEnabled { get; set; } = true;

    // Il prezzo del coperto (es. 2.00)
    public decimal CoverChargePrice { get; set; } = 2.00m;

    // Fuso orario per calcolare le chiusure cassa correttamente, fondamentale in un SaaS
    public string Timezone { get; set; } = "Europe/Rome";
}