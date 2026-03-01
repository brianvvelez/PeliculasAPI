using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Data;
using PeliculasAPI.DTOs;
using PeliculasAPI.Models;

namespace PeliculasAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductoraController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductoraController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/productora
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductoraResponseDto>>> GetAll()
    {
        var productoras = await _context.Productoras.ToListAsync();
        return Ok(productoras.Select(MapToDto));
    }

    // GET: api/productora/activas
    [HttpGet("activas")]
    public async Task<ActionResult<IEnumerable<ProductoraResponseDto>>> GetActivas()
    {
        var productoras = await _context.Productoras
            .Where(p => p.Estado)
            .ToListAsync();
        return Ok(productoras.Select(MapToDto));
    }

    // GET: api/productora/5
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductoraResponseDto>> GetById(int id)
    {
        var productora = await _context.Productoras.FindAsync(id);
        if (productora is null)
            return NotFound(new { mensaje = $"Productora con ID {id} no encontrada" });

        return Ok(MapToDto(productora));
    }

    // POST: api/productora
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductoraResponseDto>> Create([FromBody] ProductoraCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existe = await _context.Productoras.AnyAsync(p => p.Nombre.ToLower() == dto.Nombre.ToLower());
        if (existe)
            return BadRequest(new { mensaje = "Ya existe una productora con ese nombre" });

        var productora = new Productora
        {
            Nombre             = dto.Nombre,
            Estado             = dto.Estado,
            Slogan             = dto.Slogan,
            Descripcion        = dto.Descripcion,
            FechaCreacion      = DateTime.UtcNow,
            FechaActualizacion = DateTime.UtcNow
        };

        _context.Productoras.Add(productora);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = productora.Id }, MapToDto(productora));
    }

    // PUT: api/productora/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductoraResponseDto>> Update(int id, [FromBody] ProductoraUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var productora = await _context.Productoras.FindAsync(id);
        if (productora is null)
            return NotFound(new { mensaje = $"Productora con ID {id} no encontrada" });

        var existe = await _context.Productoras.AnyAsync(p => p.Nombre.ToLower() == dto.Nombre.ToLower() && p.Id != id);
        if (existe)
            return BadRequest(new { mensaje = "Ya existe una productora con ese nombre" });

        productora.Nombre            = dto.Nombre;
        productora.Estado            = dto.Estado;
        productora.Slogan            = dto.Slogan;
        productora.Descripcion       = dto.Descripcion;
        productora.FechaActualizacion = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(MapToDto(productora));
    }

    // DELETE: api/productora/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id)
    {
        var productora = await _context.Productoras.FindAsync(id);
        if (productora is null)
            return NotFound(new { mensaje = $"Productora con ID {id} no encontrada" });

        var tieneMedias = await _context.Medias.AnyAsync(m => m.ProductoraId == id);
        if (tieneMedias)
            return BadRequest(new { mensaje = "No se puede eliminar la productora porque tiene películas/series asociadas" });

        _context.Productoras.Remove(productora);
        await _context.SaveChangesAsync();
        return Ok(new { mensaje = "Productora eliminada correctamente" });
    }

    private static ProductoraResponseDto MapToDto(Productora p) => new()
    {
        Id                 = p.Id,
        Nombre             = p.Nombre,
        Estado             = p.Estado,
        FechaCreacion      = p.FechaCreacion,
        FechaActualizacion = p.FechaActualizacion,
        Slogan             = p.Slogan,
        Descripcion        = p.Descripcion
    };
}
