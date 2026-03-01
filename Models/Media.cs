using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Models;

public class Media
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Serial { get; set; } = string.Empty; // Único

    [Required, MaxLength(300)]
    public string Titulo { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Sinopsis { get; set; }

    [Required, MaxLength(500)]
    public string Url { get; set; } = string.Empty; // Único

    [MaxLength(500)]
    public string? ImagenPortada { get; set; } // Ruta o URL de la imagen

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

    [Required]
    public int AnoEstreno { get; set; }

    // FK - Género (solo activos)
    [Required]
    public int GeneroId { get; set; }
    public Genero? Genero { get; set; }

    // FK - Director (solo activos)
    [Required]
    public int DirectorId { get; set; }
    public Director? Director { get; set; }

    // FK - Productora (solo activas)
    [Required]
    public int ProductoraId { get; set; }
    public Productora? Productora { get; set; }

    // FK - Tipo
    [Required]
    public int TipoId { get; set; }
    public Tipo? Tipo { get; set; }
}
