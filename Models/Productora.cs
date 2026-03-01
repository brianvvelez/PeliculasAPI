using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Models;

public class Productora
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Nombre { get; set; } = string.Empty;

    public bool Estado { get; set; } = true; // true = Activo, false = Inactivo

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

    [MaxLength(300)]
    public string? Slogan { get; set; }

    [MaxLength(1000)]
    public string? Descripcion { get; set; }

    // Navegación
    public ICollection<Media> Medias { get; set; } = new List<Media>();
}
