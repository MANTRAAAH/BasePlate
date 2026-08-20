using BasePlate.Core.DTOs;
using BasePlate.Core.Entities;
using BasePlate.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BasePlate.Api.Controllers;

[Authorize(Roles = "Admin")] // 👈 IL LUCCHETTO PRINCIPALE RESTA
[ApiController]
[Route("api/[controller]")]
public class IngredientiController : ControllerBase
{
    private readonly BasePlateDbContext _context;
    public IngredientiController(BasePlateDbContext context) => _context = context;

    // ==========================================
    // GET: api/ingredienti (PUBBLICO - Vetrina)
    // ==========================================
    [AllowAnonymous] // 👈 SBLOCCO CHIRURGICO PER IL PUBBLICO
    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _context.Ingredienti.ToListAsync());

    // ==========================================
    // METODI POST, PUT, DELETE (PROTETTI - Solo Admin)
    // ==========================================
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] LookupDto request)
    {
        var entity = new Ingrediente { Nome = request.Nome };
        _context.Ingredienti.Add(entity);
        await _context.SaveChangesAsync();
        return Ok(entity);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] LookupDto request)
    {
        var entity = await _context.Ingredienti.FindAsync(id);
        if (entity == null) return NotFound();
        entity.Nome = request.Nome;
        await _context.SaveChangesAsync();
        return Ok(entity);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _context.Ingredienti.FindAsync(id);
        if (entity == null) return NotFound();
        _context.Ingredienti.Remove(entity);
        await _context.SaveChangesAsync();
        return Ok();
    }
}