using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DTOs;

// ── GENERO ─────────────────────────────────────────────────────────────
public class GeneroCreateDto
{
    [Required(ErrorMessage = "El nombre es requerido")]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;
    public bool Estado { get; set; } = true;
    [MaxLength(500)]
    public string? Descripcion { get; set; }
}

public class GeneroUpdateDto : GeneroCreateDto { }

public class GeneroResponseDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool Estado { get; set; }
    public string EstadoTexto => Estado ? "Activo" : "Inactivo";
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaActualizacion { get; set; }
    public string? Descripcion { get; set; }
}

// ── DIRECTOR ───────────────────────────────────────────────────────────
public class DirectorCreateDto
{
    [Required(ErrorMessage = "El nombre es requerido")]
    [MaxLength(200)]
    public string Nombres { get; set; } = string.Empty;
    public bool Estado { get; set; } = true;
}

public class DirectorUpdateDto : DirectorCreateDto { }

public class DirectorResponseDto
{
    public int Id { get; set; }
    public string Nombres { get; set; } = string.Empty;
    public bool Estado { get; set; }
    public string EstadoTexto => Estado ? "Activo" : "Inactivo";
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaActualizacion { get; set; }
}

// ── PRODUCTORA ─────────────────────────────────────────────────────────
public class ProductoraCreateDto
{
    [Required(ErrorMessage = "El nombre es requerido")]
    [MaxLength(200)]
    public string Nombre { get; set; } = string.Empty;
    public bool Estado { get; set; } = true;
    [MaxLength(300)]
    public string? Slogan { get; set; }
    [MaxLength(1000)]
    public string? Descripcion { get; set; }
}

public class ProductoraUpdateDto : ProductoraCreateDto { }

public class ProductoraResponseDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool Estado { get; set; }
    public string EstadoTexto => Estado ? "Activo" : "Inactivo";
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaActualizacion { get; set; }
    public string? Slogan { get; set; }
    public string? Descripcion { get; set; }
}

// ── TIPO ───────────────────────────────────────────────────────────────
public class TipoCreateDto
{
    [Required(ErrorMessage = "El nombre es requerido")]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;
    [MaxLength(500)]
    public string? Descripcion { get; set; }
}

public class TipoUpdateDto : TipoCreateDto { }

public class TipoResponseDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaActualizacion { get; set; }
    public string? Descripcion { get; set; }
}

// ── MEDIA ──────────────────────────────────────────────────────────────
public class MediaCreateDto
{
    [Required(ErrorMessage = "El serial es requerido")]
    [MaxLength(50)]
    public string Serial { get; set; } = string.Empty;

    [Required(ErrorMessage = "El título es requerido")]
    [MaxLength(300)]
    public string Titulo { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Sinopsis { get; set; }

    [Required(ErrorMessage = "La URL es requerida")]
    [MaxLength(500)]
    public string Url { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? ImagenPortada { get; set; }

    [Required(ErrorMessage = "El año de estreno es requerido")]
    [Range(1888, 2100, ErrorMessage = "Año de estreno inválido")]
    public int AnoEstreno { get; set; }

    [Required(ErrorMessage = "El género es requerido")]
    public int GeneroId { get; set; }

    [Required(ErrorMessage = "El director es requerido")]
    public int DirectorId { get; set; }

    [Required(ErrorMessage = "La productora es requerida")]
    public int ProductoraId { get; set; }

    [Required(ErrorMessage = "El tipo es requerido")]
    public int TipoId { get; set; }
}

public class MediaUpdateDto : MediaCreateDto { }

public class MediaResponseDto
{
    public int Id { get; set; }
    public string Serial { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string? Sinopsis { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? ImagenPortada { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaActualizacion { get; set; }
    public int AnoEstreno { get; set; }
    public int GeneroId { get; set; }
    public string NombreGenero { get; set; } = string.Empty;
    public int DirectorId { get; set; }
    public string NombreDirector { get; set; } = string.Empty;
    public int ProductoraId { get; set; }
    public string NombreProductora { get; set; } = string.Empty;
    public int TipoId { get; set; }
    public string NombreTipo { get; set; } = string.Empty;
}
