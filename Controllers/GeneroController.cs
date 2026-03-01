using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Data;
using PeliculasAPI.DTOs;
using PeliculasAPI.Models;

namespace PeliculasAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class GeneroController : ControllerBase
{
    private readonly AppDbContext _context;

    public GeneroController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/genero
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GeneroResponseDto>>> GetAll()
    {
        var generos = await _context.Generos.ToListAsync();
        return Ok(generos.Select(MapToDto));
    }

    // GET: api/genero/activos
    [HttpGet("activos")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GeneroResponseDto>>> GetActivos()
    {
        var generos = await _context.Generos
            .Where(g => g.Estado)
            .ToListAsync();
        return Ok(generos.Select(MapToDto));
    }

    // GET: api/genero/5
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GeneroResponseDto>> GetById(int id)
    {
        var genero = await _context.Generos.FindAsync(id);
        if (genero is null)
            return NotFound(new { mensaje = $"Género con ID {id} no encontrado" });

        return Ok(MapToDto(genero));
    }

    // POST: api/genero
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GeneroResponseDto>> Create([FromBody] GeneroCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Verificar nombre duplicado
        var existe = await _context.Generos.AnyAsync(g => g.Nombre.ToLower() == dto.Nombre.ToLower());
        if (existe)
            return BadRequest(new { mensaje = "Ya existe un género con ese nombre" });

        var genero = new Genero
        {
            Nombre           = dto.Nombre,
            Estado           = dto.Estado,
            Descripcion      = dto.Descripcion,
            FechaCreacion    = DateTime.UtcNow,
            FechaActualizacion = DateTime.UtcNow
        };

        _context.Generos.Add(genero);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = genero.Id }, MapToDto(genero));
    }

    // PUT: api/genero/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GeneroResponseDto>> Update(int id, [FromBody] GeneroUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var genero = await _context.Generos.FindAsync(id);
        if (genero is null)
            return NotFound(new { mensaje = $"Género con ID {id} no encontrado" });

        // Verificar nombre duplicado (excluyendo el actual)
        var existe = await _context.Generos.AnyAsync(g => g.Nombre.ToLower() == dto.Nombre.ToLower() && g.Id != id);
        if (existe)
            return BadRequest(new { mensaje = "Ya existe un género con ese nombre" });

        genero.Nombre            = dto.Nombre;
        genero.Estado            = dto.Estado;
        genero.Descripcion       = dto.Descripcion;
        genero.FechaActualizacion = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(MapToDto(genero));
    }

    // DELETE: api/genero/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id)
    {
        var genero = await _context.Generos.FindAsync(id);
        if (genero is null)
            return NotFound(new { mensaje = $"Género con ID {id} no encontrado" });

        // No eliminar si tiene películas asociadas
        var tieneMedias = await _context.Medias.AnyAsync(m => m.GeneroId == id);
        if (tieneMedias)
            return BadRequest(new { mensaje = "No se puede eliminar el género porque tiene películas/series asociadas" });

        _context.Generos.Remove(genero);
        await _context.SaveChangesAsync();
        return Ok(new { mensaje = "Género eliminado correctamente" });
    }

    private static GeneroResponseDto MapToDto(Genero g) => new()
    {
        Id                 = g.Id,
        Nombre             = g.Nombre,
        Estado             = g.Estado,
        FechaCreacion      = g.FechaCreacion,
        FechaActualizacion = g.FechaActualizacion,
        Descripcion        = g.Descripcion
    };
}
