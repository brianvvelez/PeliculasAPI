# PeliculasAPI
**Caso de Estudio - Ingeniería Web II | IU Digital de Antioquia**

API REST para gestión de películas y series construida con **ASP.NET + Entity Framework Core + SQLite**.

---

## Estructura del Proyecto

```
PeliculasAPI/
├── Controllers/
│   ├── GeneroController.cs
│   ├── DirectorController.cs
│   ├── ProductoraController.cs
│   ├── TipoController.cs
│   └── MediaController.cs
├── Data/
│   └── AppDbContext.cs        ← DbContext + Seed data
├── DTOs/
│   └── Dtos.cs                ← Todos los DTOs (Request/Response)
├── Models/
│   ├── Genero.cs
│   ├── Director.cs
│   ├── Productora.cs
│   ├── Tipo.cs
│   └── Media.cs
├── Program.cs
├── appsettings.json
└── PeliculasAPI.csproj
```

---

## Modelo de Base de Datos

```
Genero          Director        Productora      Tipo
──────          ────────        ──────────      ────
Id (PK)         Id (PK)         Id (PK)         Id (PK)
Nombre          Nombres         Nombre          Nombre
Estado          Estado          Estado          Descripcion
Descripcion     FechaCreacion   Slogan          FechaCreacion
FechaCreacion   FechaActualiz.  Descripcion     FechaActualiz.
FechaActualiz.                  FechaCreacion
                                FechaActualiz.

Media
─────────────────────────────────
Id (PK)
Serial (ÚNICO)
Titulo
Sinopsis
Url (ÚNICO)
ImagenPortada
AnoEstreno
FechaCreacion
FechaActualizacion
GeneroId    → FK → Genero (solo activos)
DirectorId  → FK → Director (solo activos)
ProductoraId→ FK → Productora (solo activas)
TipoId      → FK → Tipo
```

---

## Pasos para ejecutar

### 1. Prerrequisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### 2. Clonar / copiar el proyecto
```bash
cd PeliculasAPI
```

### 3. Restaurar paquetes
```bash
dotnet restore
```

### 4. Crear la migración inicial
```bash
dotnet ef migrations add InitialCreate
```

### 5. Ejecutar la aplicación
```bash
dotnet run
```
> La BD SQLite (`peliculas.db`) se crea y migra automáticamente al iniciar.

### 6. Abrir Swagger UI
```
http://localhost:5000
```

---

## Endpoints de la API

### Género `/api/genero`
| Método | Ruta                  | Descripción                     |
|--------|-----------------------|---------------------------------|
| GET    | /api/genero           | Listar todos los géneros        |
| GET    | /api/genero/activos   | Listar solo géneros activos     |
| GET    | /api/genero/{id}      | Obtener género por ID           |
| POST   | /api/genero           | Crear género                    |
| PUT    | /api/genero/{id}      | Actualizar género               |
| DELETE | /api/genero/{id}      | Eliminar género                 |

### Director `/api/director`
| Método | Ruta                   | Descripción                     |
|--------|------------------------|---------------------------------|
| GET    | /api/director          | Listar todos                    |
| GET    | /api/director/activos  | Listar solo activos             |
| GET    | /api/director/{id}     | Obtener por ID                  |
| POST   | /api/director          | Crear director                  |
| PUT    | /api/director/{id}     | Actualizar director             |
| DELETE | /api/director/{id}     | Eliminar director               |

### Productora `/api/productora`
| Método | Ruta                    | Descripción                     |
|--------|-------------------------|---------------------------------|
| GET    | /api/productora         | Listar todas                    |
| GET    | /api/productora/activas | Listar solo activas             |
| GET    | /api/productora/{id}    | Obtener por ID                  |
| POST   | /api/productora         | Crear productora                |
| PUT    | /api/productora/{id}    | Actualizar productora           |
| DELETE | /api/productora/{id}    | Eliminar productora             |

### Tipo `/api/tipo`
| Método | Ruta            | Descripción     |
|--------|-----------------|-----------------|
| GET    | /api/tipo       | Listar todos    |
| GET    | /api/tipo/{id}  | Obtener por ID  |
| POST   | /api/tipo       | Crear tipo      |
| PUT    | /api/tipo/{id}  | Actualizar tipo |
| DELETE | /api/tipo/{id}  | Eliminar tipo   |

### Media `/api/media`
| Método | Ruta                          | Descripción                 |
|--------|-------------------------------|-----------------------------|
| GET    | /api/media                    | Listar todas las medias     |
| GET    | /api/media/{id}               | Obtener por ID              |
| GET    | /api/media/serial/{serial}    | Obtener por serial          |
| GET    | /api/media/por-genero/{id}    | Filtrar por género          |
| POST   | /api/media                    | Crear media                 |
| PUT    | /api/media/{id}               | Actualizar media            |
| DELETE | /api/media/{id}               | Eliminar media              |

---

## Ejemplos de Peticiones (Postman / cURL)

### Crear un Director
```json
POST /api/director
{
  "nombres": "Christopher Nolan",
  "estado": true
}
```

### Crear una Productora
```json
POST /api/productora
{
  "nombre": "Warner Bros",
  "estado": true,
  "slogan": "The stuff that dreams are made of",
  "descripcion": "Estudio cinematográfico fundado en 1923"
}
```

### Crear una Película
```json
POST /api/media
{
  "serial": "INC-001",
  "titulo": "Inception",
  "sinopsis": "Un ladrón que roba secretos corporativos...",
  "url": "https://streaming.iudigital.edu.co/inception",
  "imagenPortada": "https://cdn.iudigital.edu.co/inception.jpg",
  "anoEstreno": 2010,
  "generoId": 3,
  "directorId": 1,
  "productoraId": 1,
  "tipoId": 1
}
```

---
