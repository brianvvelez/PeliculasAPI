using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Data;
using PeliculasAPI.DTOs;
using PeliculasAPI.Models;

namespace PeliculasAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TipoController : ControllerBase
{
    private readonly AppDbContext _context;

    public TipoController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/tipo
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TipoResponseDto>>> GetAll()
    {
        var tipos = await _context.Tipos.ToListAsync();
        return Ok(tipos.Select(MapToDto));
    }

    // GET: api/tipo/5
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TipoResponseDto>> GetById(int id)
    {
        var tipo = await _context.Tipos.FindAsync(id);
        if (tipo is null)
            return NotFound(new { mensaje = $"Tipo con ID {id} no encontrado" });

        return Ok(MapToDto(tipo));
    }

    // POST: api/tipo
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TipoResponseDto>> Create([FromBody] TipoCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existe = await _context.Tipos.AnyAsync(t => t.Nombre.ToLower() == dto.Nombre.ToLower());
        if (existe)
            return BadRequest(new { mensaje = "Ya existe un tipo con ese nombre" });

        var tipo = new Tipo
        {
            Nombre             = dto.Nombre,
            Descripcion        = dto.Descripcion,
            FechaCreacion      = DateTime.UtcNow,
            FechaActualizacion = DateTime.UtcNow
        };

        _context.Tipos.Add(tipo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = tipo.Id }, MapToDto(tipo));
    }

    // PUT: api/tipo/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TipoResponseDto>> Update(int id, [FromBody] TipoUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var tipo = await _context.Tipos.FindAsync(id);
        if (tipo is null)
            return NotFound(new { mensaje = $"Tipo con ID {id} no encontrado" });

        var existe = await _context.Tipos.AnyAsync(t => t.Nombre.ToLower() == dto.Nombre.ToLower() && t.Id != id);
        if (existe)
            return BadRequest(new { mensaje = "Ya existe un tipo con ese nombre" });

        tipo.Nombre            = dto.Nombre;
        tipo.Descripcion       = dto.Descripcion;
        tipo.FechaActualizacion = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(MapToDto(tipo));
    }

    // DELETE: api/tipo/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id)
    {
        var tipo = await _context.Tipos.FindAsync(id);
        if (tipo is null)
            return NotFound(new { mensaje = $"Tipo con ID {id} no encontrado" });

        var tieneMedias = await _context.Medias.AnyAsync(m => m.TipoId == id);
        if (tieneMedias)
            return BadRequest(new { mensaje = "No se puede eliminar el tipo porque tiene películas/series asociadas" });

        _context.Tipos.Remove(tipo);
        await _context.SaveChangesAsync();
        return Ok(new { mensaje = "Tipo eliminado correctamente" });
    }

    private static TipoResponseDto MapToDto(Tipo t) => new()
    {
        Id                 = t.Id,
        Nombre             = t.Nombre,
        FechaCreacion      = t.FechaCreacion,
        FechaActualizacion = t.FechaActualizacion,
        Descripcion        = t.Descripcion
    };
}
