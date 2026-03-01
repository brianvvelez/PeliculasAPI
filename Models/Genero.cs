using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Models;

public class Genero
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    public bool Estado { get; set; } = true; // true = Activo, false = Inactivo

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

    [MaxLength(500)]
    public string? Descripcion { get; set; }

    // Navegación
    public ICollection<Media> Medias { get; set; } = new List<Media>();
}
