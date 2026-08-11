using BasePlate.Core.Entities;
using BasePlate.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BasePlate.Api.Controllers;

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
        // 🛡️ MAGIA MULTI-TENANT IN AZIONE:
        // Non c'è alcun ".Where(c => c.TenantId == x)". 
        // Interroghiamo tutta la tabella, ma EF Core applicherà in automatico 
        // il filtro invisibile per restituire SOLO le categorie di questo locale.
        var categorie = await _context.Categorie.ToListAsync();

        return Ok(categorie);
    }

    // POST: api/categorie
    [HttpPost]
    public async Task<IActionResult> CreaCategoria([FromBody] CategoriaDto request)
    {
        var nuovaCategoria = new Categoria
        {
            Nome = request.Nome
            // 🛡️ ALTRA MAGIA: Non stiamo assegnando il TenantId qui.
        };

        _context.Categorie.Add(nuovaCategoria);

        // Il DbContext intercetterà il salvataggio e inietterà il TenantId corretto
        // pescato dal nostro "CurrentTenantProvider" prima di scrivere su PostgreSQL.
        await _context.SaveChangesAsync();

        return Ok(nuovaCategoria);
    }
}

// Data Transfer Object (DTO) per ricevere solo i dati necessari dal client
public class CategoriaDto
{
    public string Nome { get; set; } = string.Empty;
}