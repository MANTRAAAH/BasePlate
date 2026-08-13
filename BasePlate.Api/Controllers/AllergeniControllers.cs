using BasePlate.Core.DTOs;
using BasePlate.Core.Entities;
using BasePlate.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BasePlate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AllergeniController : ControllerBase
{
    private readonly BasePlateDbContext _context;
    public AllergeniController(BasePlateDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _context.Allergeni.ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] LookupDto request)
    {
        var entity = new Allergene { Nome = request.Nome };
        _context.Allergeni.Add(entity);
        await _context.SaveChangesAsync();
        return Ok(entity);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] LookupDto request)
    {
        var entity = await _context.Allergeni.FindAsync(id);
        if (entity == null) return NotFound();
        entity.Nome = request.Nome;
        await _context.SaveChangesAsync();
        return Ok(entity);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _context.Allergeni.FindAsync(id);
        if (entity == null) return NotFound();
        _context.Allergeni.Remove(entity);
        await _context.SaveChangesAsync();
        return Ok();
    }
}