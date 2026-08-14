namespace BasePlate.Core.Entities;

using System.ComponentModel.DataAnnotations.Schema;

// Ereditando da TenantEntity, anche l'Utente prende il "Guid TenantId"
public class Utente : TenantEntity
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Ruolo { get; set; } = "Cameriere"; // Admin, Ristoratore, Cameriere, Cuoco
    [ForeignKey("TenantId")] // Diciamo a EF di usare il TenantId ereditato per questa relazione
    public Ristorante? Ristorante { get; set; }

}