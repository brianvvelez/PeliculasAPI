using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Models;

namespace PeliculasAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Genero> Generos { get; set; }
    public DbSet<Director> Directores { get; set; }
    public DbSet<Productora> Productoras { get; set; }
    public DbSet<Tipo> Tipos { get; set; }
    public DbSet<Media> Medias { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Índices únicos
        modelBuilder.Entity<Media>()
            .HasIndex(m => m.Serial)
            .IsUnique();

        modelBuilder.Entity<Media>()
            .HasIndex(m => m.Url)
            .IsUnique();

        // Relaciones explícitas para evitar cascada circular
        modelBuilder.Entity<Media>()
            .HasOne(m => m.Genero)
            .WithMany(g => g.Medias)
            .HasForeignKey(m => m.GeneroId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Media>()
            .HasOne(m => m.Director)
            .WithMany(d => d.Medias)
            .HasForeignKey(m => m.DirectorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Media>()
            .HasOne(m => m.Productora)
            .WithMany(p => p.Medias)
            .HasForeignKey(m => m.ProductoraId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Media>()
            .HasOne(m => m.Tipo)
            .WithMany(t => t.Medias)
            .HasForeignKey(m => m.TipoId)
            .OnDelete(DeleteBehavior.Restrict);

        // ── Seed Data ──────────────────────────────────────────────

        // Géneros iniciales
        modelBuilder.Entity<Genero>().HasData(
            new Genero { Id = 1, Nombre = "Acción",         Estado = true, Descripcion = "Películas de acción y adrenalina",    FechaCreacion = new DateTime(2024,1,1), FechaActualizacion = new DateTime(2024,1,1) },
            new Genero { Id = 2, Nombre = "Aventura",       Estado = true, Descripcion = "Películas de aventura y exploración", FechaCreacion = new DateTime(2024,1,1), FechaActualizacion = new DateTime(2024,1,1) },
            new Genero { Id = 3, Nombre = "Ciencia Ficción",Estado = true, Descripcion = "Películas de ciencia y futuro",       FechaCreacion = new DateTime(2024,1,1), FechaActualizacion = new DateTime(2024,1,1) },
            new Genero { Id = 4, Nombre = "Drama",          Estado = true, Descripcion = "Películas dramáticas y emotivas",     FechaCreacion = new DateTime(2024,1,1), FechaActualizacion = new DateTime(2024,1,1) },
            new Genero { Id = 5, Nombre = "Terror",         Estado = true, Descripcion = "Películas de terror y suspenso",      FechaCreacion = new DateTime(2024,1,1), FechaActualizacion = new DateTime(2024,1,1) }
        );

        // Tipos iniciales
        modelBuilder.Entity<Tipo>().HasData(
            new Tipo { Id = 1, Nombre = "Película", Descripcion = "Largometraje cinematográfico", FechaCreacion = new DateTime(2024,1,1), FechaActualizacion = new DateTime(2024,1,1) },
            new Tipo { Id = 2, Nombre = "Serie",    Descripcion = "Serie de episodios",           FechaCreacion = new DateTime(2024,1,1), FechaActualizacion = new DateTime(2024,1,1) }
        );
    }
}
