using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Configurar puerto dinámico para plataformas Cloud (Render, Railway, etc.)
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}

// ── Servicios ──────────────────────────────────────────────────────────

builder.Services.AddControllers();

// SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=peliculas.db"));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title       = "PeliculasAPI - IU Digital de Antioquia",
        Version     = "v1",
        Description = "API REST para gestión de películas y series. Módulos: Género, Director, Productora, Tipo y Media."
    });
});

// CORS (para cuando conectes el Frontend)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// ── Middleware ─────────────────────────────────────────────────────────

// Aplicar migraciones y seed automáticamente al iniciar
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PeliculasAPI v1");
    c.RoutePrefix = string.Empty; // Swagger en la raíz: http://localhost:5000
});

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
