using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Data;
using PeliculasAPI.DTOs;
using PeliculasAPI.Models;

namespace PeliculasAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class DirectorController : ControllerBase
{
    private readonly AppDbContext _context;

    public DirectorController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/director
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DirectorResponseDto>>> GetAll()
    {
        var directores = await _context.Directores.ToListAsync();
        return Ok(directores.Select(MapToDto));
    }

    // GET: api/director/activos
    [HttpGet("activos")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DirectorResponseDto>>> GetActivos()
    {
        var directores = await _context.Directores
            .Where(d => d.Estado)
            .ToListAsync();
        return Ok(directores.Select(MapToDto));
    }

    // GET: api/director/5
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DirectorResponseDto>> GetById(int id)
    {
        var director = await _context.Directores.FindAsync(id);
        if (director is null)
            return NotFound(new { mensaje = $"Director con ID {id} no encontrado" });

        return Ok(MapToDto(director));
    }

    // POST: api/director
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DirectorResponseDto>> Create([FromBody] DirectorCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var director = new Director
        {
            Nombres            = dto.Nombres,
            Estado             = dto.Estado,
            FechaCreacion      = DateTime.UtcNow,
            FechaActualizacion = DateTime.UtcNow
        };

        _context.Directores.Add(director);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = director.Id }, MapToDto(director));
    }

    // PUT: api/director/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DirectorResponseDto>> Update(int id, [FromBody] DirectorUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var director = await _context.Directores.FindAsync(id);
        if (director is null)
            return NotFound(new { mensaje = $"Director con ID {id} no encontrado" });

        director.Nombres           = dto.Nombres;
        director.Estado            = dto.Estado;
        director.FechaActualizacion = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(MapToDto(director));
    }

    // DELETE: api/director/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id)
    {
        var director = await _context.Directores.FindAsync(id);
        if (director is null)
            return NotFound(new { mensaje = $"Director con ID {id} no encontrado" });

        var tieneMedias = await _context.Medias.AnyAsync(m => m.DirectorId == id);
        if (tieneMedias)
            return BadRequest(new { mensaje = "No se puede eliminar el director porque tiene películas/series asociadas" });

        _context.Directores.Remove(director);
        await _context.SaveChangesAsync();
        return Ok(new { mensaje = "Director eliminado correctamente" });
    }

    private static DirectorResponseDto MapToDto(Director d) => new()
    {
        Id                 = d.Id,
        Nombres            = d.Nombres,
        Estado             = d.Estado,
        FechaCreacion      = d.FechaCreacion,
        FechaActualizacion = d.FechaActualizacion
    };
}
