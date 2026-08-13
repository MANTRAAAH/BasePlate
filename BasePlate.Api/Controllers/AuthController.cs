using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BasePlate.Core.DTOs;
using Microsoft.AspNetCore.Authorization; // 👈 Aggiungi questo in cima se non c'è
using BasePlate.Core.Entities;
using BasePlate.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BasePlate.Api.Controllers;

[Authorize(Roles = "Admin")] // 👈 IL LUCCHETTO! Solo gli Admin possono usare questi endpoint
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

    // POST: api/auth/register (Endpoint temporaneo per creare il tuo utente)
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto request)
    {
        if (await _context.Utenti.AnyAsync(u => u.Email == request.Email))
            return BadRequest(new { Message = "Email già in uso." });

        var utente = new Utente
        {
            Nome = request.Nome,
            Email = request.Email,
            // Criptiamo la password usando BCrypt!
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Ruolo = "Admin"
        };

        _context.Utenti.Add(utente);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Amministratore creato con successo!" });
    }

    // POST: api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        var utente = await _context.Utenti.FirstOrDefaultAsync(u => u.Email == request.Email);

        // Controlliamo che l'utente esista e che la password coincida con l'hash salvato
        if (utente == null || !BCrypt.Net.BCrypt.Verify(request.Password, utente.PasswordHash))
            return Unauthorized(new { Message = "Credenziali non valide." });

        var token = GeneraJwtToken(utente);

        return Ok(new
        {
            Token = token,
            Nome = utente.Nome,
            Ruolo = utente.Ruolo
        });
    }

    private string GeneraJwtToken(Utente utente)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // I "Claims" sono le informazioni leggibili all'interno del token
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, utente.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, utente.Email),
            new Claim("nome", utente.Nome),
            new Claim(ClaimTypes.Role, utente.Ruolo)
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