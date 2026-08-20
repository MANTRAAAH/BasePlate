namespace BasePlate.Core.Entities;

public class Ristorante
{
    // La Primary Key del Tenant (corrisponde al Guid TenantId delle altre tabelle)
    public Guid Id { get; set; }

    public string Nome { get; set; } = string.Empty;
    public string PartitaIva { get; set; } = string.Empty;

    // Identità Digitale e Frontend
    public string DominioPersonalizzato { get; set; } = string.Empty;
    public string TemaLayout { get; set; } = "tema-liquid-glass";
    public string ColorePrimario { get; set; } = "#F59E0B";
    public string LogoUrl { get; set; } = string.Empty;

    // Campi SaaS per il SuperAdmin
    public DateTime DataCreazione { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
}