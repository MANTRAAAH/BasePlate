using BasePlate.Core.DTOs.Prodotti; // 📦 Importiamo i nuovi DTO ordinati
using BasePlate.Core.Entities;
using BasePlate.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization; // 👈 Aggiungi questo in cima se non c'è
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BasePlate.Api.Controllers;

[Authorize(Roles = "Admin")] // 👈 IL LUCCHETTO! Solo gli Admin possono usare questi endpoint
[ApiController]
[Route("api/[controller]")]
public class CategorieController : ControllerBase
{
    private readonly BasePlateDbContext _context;

    public CategorieController(BasePlateDbContext context)
    {
        _context = context;
    }

    // GET: api/categorie
    [HttpGet]
    public async Task<IActionResult> GetCategorie()
    {
        // 🛡️ Mappiamo direttamente l'entità al ReadDto per non esporre dettagli interni
        var categorie = await _context.Categorie
            .Select(c => new CategoriaReadDto
            {
                Id = c.Id,
                Nome = c.Nome
            })
            .ToListAsync();

        return Ok(categorie);
    }

    // POST: api/categorie
    [HttpPost]
    public async Task<IActionResult> CreaCategoria([FromBody] CreaCategoriaDto request)
    {
        var nuovaCategoria = new Categoria
        {
            Nome = request.Nome
            // 🛡️ Il TenantId viene iniettato automaticamente dal DbContext/Interceptors
        };

        _context.Categorie.Add(nuovaCategoria);
        await _context.SaveChangesAsync();

        // Restituiamo il DTO di lettura pulito anziché l'entità grezza del database
        var responseDto = new CategoriaReadDto
        {
            Id = nuovaCategoria.Id,
            Nome = nuovaCategoria.Nome
        };

        return CreatedAtAction(nameof(GetCategorie), new { id = responseDto.Id }, responseDto);
    }
}