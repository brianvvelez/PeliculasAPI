using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Models;

public class Director
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Nombres { get; set; } = string.Empty;

    public bool Estado { get; set; } = true; // true = Activo, false = Inactivo

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

    // Navegación
    public ICollection<Media> Medias { get; set; } = new List<Media>();
}
