using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BasePlate.Core.DTOs;
using BasePlate.Core.Entities;
using BasePlate.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BasePlate.Api.Controllers;

[Authorize] // 👈 Di base richiede il login per tutto il controller...
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly BasePlateDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(BasePlateDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [AllowAnonymous] // 👈 ...MA sblocca esplicitamente la registrazione!
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto request)
    {
        if (await _context.Utenti.AnyAsync(u => u.Email == request.Email))
            return BadRequest(new { Message = "Email già in uso." });

        // Verifichiamo che il ristorante esista davvero!
        if (!await _context.Ristoranti.AnyAsync(r => r.Id == request.TenantId))
            return BadRequest(new { Message = "Ristorante (Tenant) non trovato." });

        var utente = new Utente
        {
            Nome = request.Nome,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Ruolo = "Admin",
            TenantId = request.TenantId // 👈 Assegniamo fisicamente l'utente al Ristorante
        };

        _context.Utenti.Add(utente);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Amministratore creato con successo!" });
    }

    [AllowAnonymous] // 👈 Sblocca esplicitamente il login!
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        var utente = await _context.Utenti.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (utente == null || !BCrypt.Net.BCrypt.Verify(request.Password, utente.PasswordHash))
            return Unauthorized(new { Message = "Credenziali non valide." });

        var token = GeneraJwtToken(utente);

        return Ok(new
        {
            Token = token,
            Nome = utente.Nome,
            Ruolo = utente.Ruolo,
            TenantId = utente.TenantId // Lo restituiamo anche nel JSON per comodità del frontend
        });
    }

    private string GeneraJwtToken(Utente utente)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, utente.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, utente.Email),
            new Claim("nome", utente.Nome),
            new Claim(ClaimTypes.Role, utente.Ruolo),
            // 🔥 IL CUORE DEL MULTI-TENANT: Scriviamo il TenantId dentro il Token!
            new Claim("tenantId", utente.TenantId.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}