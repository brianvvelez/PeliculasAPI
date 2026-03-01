using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Models;

public class Tipo
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

    [MaxLength(500)]
    public string? Descripcion { get; set; }

    // Navegación
    public ICollection<Media> Medias { get; set; } = new List<Media>();
}
