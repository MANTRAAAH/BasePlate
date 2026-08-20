namespace BasePlate.Core.DTOs;

public class RegisterDto
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    // NUOVO: Serve per sapere a quale ristorante agganciare l'utente
    public Guid TenantId { get; set; }
}

public class LoginDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}