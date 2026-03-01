using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Data;
using PeliculasAPI.DTOs;
using PeliculasAPI.Models;

namespace PeliculasAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class MediaController : ControllerBase
{
    private readonly AppDbContext _context;

    public MediaController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/media
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MediaResponseDto>>> GetAll()
    {
        var medias = await _context.Medias
            .Include(m => m.Genero)
            .Include(m => m.Director)
            .Include(m => m.Productora)
            .Include(m => m.Tipo)
            .ToListAsync();

        return Ok(medias.Select(MapToDto));
    }

    // GET: api/media/5
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MediaResponseDto>> GetById(int id)
    {
        var media = await _context.Medias
            .Include(m => m.Genero)
            .Include(m => m.Director)
            .Include(m => m.Productora)
            .Include(m => m.Tipo)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (media is null)
            return NotFound(new { mensaje = $"Media con ID {id} no encontrada" });

        return Ok(MapToDto(media));
    }

    // GET: api/media/serial/ABC123
    [HttpGet("serial/{serial}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MediaResponseDto>> GetBySerial(string serial)
    {
        var media = await _context.Medias
            .Include(m => m.Genero)
            .Include(m => m.Director)
            .Include(m => m.Productora)
            .Include(m => m.Tipo)
            .FirstOrDefaultAsync(m => m.Serial == serial);

        if (media is null)
            return NotFound(new { mensaje = $"Media con serial '{serial}' no encontrada" });

        return Ok(MapToDto(media));
    }

    // GET: api/media/por-genero/1
    [HttpGet("por-genero/{generoId}")]
    public async Task<ActionResult<IEnumerable<MediaResponseDto>>> GetByGenero(int generoId)
    {
        var medias = await _context.Medias
            .Include(m => m.Genero)
            .Include(m => m.Director)
            .Include(m => m.Productora)
            .Include(m => m.Tipo)
            .Where(m => m.GeneroId == generoId)
            .ToListAsync();

        return Ok(medias.Select(MapToDto));
    }

    // POST: api/media
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MediaResponseDto>> Create([FromBody] MediaCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Validar uniqueness
        if (await _context.Medias.AnyAsync(m => m.Serial == dto.Serial))
            return BadRequest(new { mensaje = "Ya existe una media con ese serial" });

        if (await _context.Medias.AnyAsync(m => m.Url == dto.Url))
            return BadRequest(new { mensaje = "Ya existe una media con esa URL" });

        // Validar que el Género exista y esté ACTIVO
        var genero = await _context.Generos.FindAsync(dto.GeneroId);
        if (genero is null)
            return BadRequest(new { mensaje = $"El género con ID {dto.GeneroId} no existe" });
        if (!genero.Estado)
            return BadRequest(new { mensaje = "El género seleccionado está inactivo. Solo se permiten géneros activos" });

        // Validar que el Director exista y esté ACTIVO
        var director = await _context.Directores.FindAsync(dto.DirectorId);
        if (director is null)
            return BadRequest(new { mensaje = $"El director con ID {dto.DirectorId} no existe" });
        if (!director.Estado)
            return BadRequest(new { mensaje = "El director seleccionado está inactivo. Solo se permiten directores activos" });

        // Validar que la Productora exista y esté ACTIVA
        var productora = await _context.Productoras.FindAsync(dto.ProductoraId);
        if (productora is null)
            return BadRequest(new { mensaje = $"La productora con ID {dto.ProductoraId} no existe" });
        if (!productora.Estado)
            return BadRequest(new { mensaje = "La productora seleccionada está inactiva. Solo se permiten productoras activas" });

        // Validar que el Tipo exista
        var tipo = await _context.Tipos.FindAsync(dto.TipoId);
        if (tipo is null)
            return BadRequest(new { mensaje = $"El tipo con ID {dto.TipoId} no existe" });

        var media = new Media
        {
            Serial             = dto.Serial,
            Titulo             = dto.Titulo,
            Sinopsis           = dto.Sinopsis,
            Url                = dto.Url,
            ImagenPortada      = dto.ImagenPortada,
            AnoEstreno         = dto.AnoEstreno,
            GeneroId           = dto.GeneroId,
            DirectorId         = dto.DirectorId,
            ProductoraId       = dto.ProductoraId,
            TipoId             = dto.TipoId,
            FechaCreacion      = DateTime.UtcNow,
            FechaActualizacion = DateTime.UtcNow
        };

        _context.Medias.Add(media);
        await _context.SaveChangesAsync();

        // Recargar con navegación para la respuesta
        await _context.Entry(media).Reference(m => m.Genero).LoadAsync();
        await _context.Entry(media).Reference(m => m.Director).LoadAsync();
        await _context.Entry(media).Reference(m => m.Productora).LoadAsync();
        await _context.Entry(media).Reference(m => m.Tipo).LoadAsync();

        return CreatedAtAction(nameof(GetById), new { id = media.Id }, MapToDto(media));
    }

    // PUT: api/media/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MediaResponseDto>> Update(int id, [FromBody] MediaUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var media = await _context.Medias
            .Include(m => m.Genero)
            .Include(m => m.Director)
            .Include(m => m.Productora)
            .Include(m => m.Tipo)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (media is null)
            return NotFound(new { mensaje = $"Media con ID {id} no encontrada" });

        // Validar uniqueness excluyendo el actual
        if (await _context.Medias.AnyAsync(m => m.Serial == dto.Serial && m.Id != id))
            return BadRequest(new { mensaje = "Ya existe otra media con ese serial" });

        if (await _context.Medias.AnyAsync(m => m.Url == dto.Url && m.Id != id))
            return BadRequest(new { mensaje = "Ya existe otra media con esa URL" });

        // Validar relaciones activas
        var genero = await _context.Generos.FindAsync(dto.GeneroId);
        if (genero is null || !genero.Estado)
            return BadRequest(new { mensaje = "El género seleccionado no existe o está inactivo" });

        var director = await _context.Directores.FindAsync(dto.DirectorId);
        if (director is null || !director.Estado)
            return BadRequest(new { mensaje = "El director seleccionado no existe o está inactivo" });

        var productora = await _context.Productoras.FindAsync(dto.ProductoraId);
        if (productora is null || !productora.Estado)
            return BadRequest(new { mensaje = "La productora seleccionada no existe o está inactiva" });

        var tipo = await _context.Tipos.FindAsync(dto.TipoId);
        if (tipo is null)
            return BadRequest(new { mensaje = "El tipo seleccionado no existe" });

        media.Serial             = dto.Serial;
        media.Titulo             = dto.Titulo;
        media.Sinopsis           = dto.Sinopsis;
        media.Url                = dto.Url;
        media.ImagenPortada      = dto.ImagenPortada;
        media.AnoEstreno         = dto.AnoEstreno;
        media.GeneroId           = dto.GeneroId;
        media.DirectorId         = dto.DirectorId;
        media.ProductoraId       = dto.ProductoraId;
        media.TipoId             = dto.TipoId;
        media.FechaActualizacion = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        // Refrescar navegación
        media.Genero    = genero;
        media.Director  = director;
        media.Productora = productora;
        media.Tipo      = tipo;

        return Ok(MapToDto(media));
    }

    // DELETE: api/media/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var media = await _context.Medias.FindAsync(id);
        if (media is null)
            return NotFound(new { mensaje = $"Media con ID {id} no encontrada" });

        _context.Medias.Remove(media);
        await _context.SaveChangesAsync();
        return Ok(new { mensaje = "Media eliminada correctamente" });
    }

    private static MediaResponseDto MapToDto(Media m) => new()
    {
        Id               = m.Id,
        Serial           = m.Serial,
        Titulo           = m.Titulo,
        Sinopsis         = m.Sinopsis,
        Url              = m.Url,
        ImagenPortada    = m.ImagenPortada,
        FechaCreacion    = m.FechaCreacion,
        FechaActualizacion = m.FechaActualizacion,
        AnoEstreno       = m.AnoEstreno,
        GeneroId         = m.GeneroId,
        NombreGenero     = m.Genero?.Nombre ?? string.Empty,
        DirectorId       = m.DirectorId,
        NombreDirector   = m.Director?.Nombres ?? string.Empty,
        ProductoraId     = m.ProductoraId,
        NombreProductora = m.Productora?.Nombre ?? string.Empty,
        TipoId           = m.TipoId,
        NombreTipo       = m.Tipo?.Nombre ?? string.Empty
    };
}
