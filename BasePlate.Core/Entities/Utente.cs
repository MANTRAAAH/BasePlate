namespace BasePlate.Core.Entities;

public class Utente
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    // Qui salveremo la password crittografata, mai in chiaro!
    public string PasswordHash { get; set; } = string.Empty;

    // Ruolo: es. "Admin", "Cameriere", ecc.
    public string Ruolo { get; set; } = "Admin";
}