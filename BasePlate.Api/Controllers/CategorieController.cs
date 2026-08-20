using BasePlate.Core.DTOs.Prodotti;
using BasePlate.Core.Entities;
using BasePlate.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BasePlate.Api.Controllers;

[Authorize(Roles = "Admin")] // 👈 IL LUCCHETTO PRINCIPALE RESTA
[ApiController]
[Route("api/[controller]")]
public class CategorieController : ControllerBase
{
    private readonly BasePlateDbContext _context;

    public CategorieController(BasePlateDbContext context)
    {
        _context = context;
    }

    // ==========================================
    // GET: api/categorie (PUBBLICO - Vetrina)
    // ==========================================
    [AllowAnonymous] // 👈 SBLOCCO CHIRURGICO PER IL PUBBLICO
    [HttpGet]
    public async Task<IActionResult> GetCategorie()
    {
        var categorie = await _context.Categorie
            .Select(c => new CategoriaReadDto
            {
                Id = c.Id,
                Nome = c.Nome
            })
            .ToListAsync();

        return Ok(categorie);
    }

    // ==========================================
    // POST: api/categorie (PROTETTO - Solo Admin)
    // ==========================================
    [HttpPost]
    public async Task<IActionResult> CreaCategoria([FromBody] CreaCategoriaDto request)
    {
        var nuovaCategoria = new Categoria
        {
            Nome = request.Nome
        };

        _context.Categorie.Add(nuovaCategoria);
        await _context.SaveChangesAsync();

        var responseDto = new CategoriaReadDto
        {
            Id = nuovaCategoria.Id,
            Nome = nuovaCategoria.Nome
        };

        return CreatedAtAction(nameof(GetCategorie), new { id = responseDto.Id }, responseDto);
    }
}