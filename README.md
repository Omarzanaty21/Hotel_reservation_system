# Hotel Reservation System

A full-stack hotel reservation application with an Angular 22 frontend and an ASP.NET Core 8 Web API backend backed by PostgreSQL.

Admin credentials: `admin` / `Admin@123` used to access admin features: {add_room, list_reservations ,cancel-reservations}

---

## Key Design Decisions

### PostgreSQL over SQLite

SQLite was used during early development because it is file-based, requires no external server, and allows the application to run with zero infrastructure setup. However, PostgreSQL was chosen for the final solution for the following reasons:

- **Concurrency** — PostgreSQL handles multiple simultaneous connections properly, whereas SQLite serialises all writes and degrades quickly under concurrent load.
- **Transaction isolation** — PostgreSQL supports `Serializable` isolation, which is used to prevent double-booking of rooms. SQLite's isolation model is insufficient for this guarantee.
- **Production readiness** — PostgreSQL is a battle-tested, production-grade database with robust support for connection pooling, backups, replication, and monitoring tooling.
- **Scalability** — PostgreSQL scales horizontally and vertically in ways SQLite cannot.

The tradeoff is increased setup complexity and a dependency on a running database server. This is mitigated in development and deployment by Docker, which provides a PostgreSQL instance with a single command.

### Environment Variables (.env)

Runtime configuration is stored in a `.env` file rather than hardcoded in source files. This approach:

- **Separates configuration from code** — values like connection strings, JWT secrets, and ports can change between environments without touching application code.
- **Improves security** — sensitive values are not embedded in the codebase or build artifacts.
- **Simplifies deployment** — Docker Compose reads the `.env` file directly to configure all containers, making environment-specific setup explicit and repeatable.
- **Supports multiple environments** — the same codebase runs locally, in Docker, or in a CI pipeline by swapping a single file.

The `.env` file in this repository contains only non-sensitive defaults so the project works immediately after cloning. In a real production deployment these values should be replaced with strong secrets and managed through a secrets manager or CI/CD environment variables.

---

## Future Improvements

- **Refresh tokens and token revocation** — the current JWT implementation uses short-lived access tokens. Adding refresh tokens and a server-side revocation mechanism would improve security for longer sessions.
- **Email notifications** — send confirmation and cancellation emails to guests when reservations are created or deleted.
- **Room availability calendar** — a visual calendar view showing booked and available dates per room, giving users a clearer picture before making a reservation.
- **Unit and integration tests** — increase confidence in business logic and API behaviour with automated tests covering reservation rules, authentication, and edge cases.
- **CI/CD pipeline** — automate building, testing, and Docker image publishing on every push using GitHub Actions or a similar platform.
- **Rate limiting** — protect the API from abuse by throttling repeated requests per client, particularly on authentication endpoints.
- **Caching** — cache frequently accessed, rarely changing data such as room listings to reduce database load and improve response times.
- **API versioning** — introduce versioning to allow the API to evolve without breaking existing clients.
- **Localization** — support multiple languages in both the frontend and API error messages to make the application accessible to a wider audience.

---

## Table of Contents

- [Key Design Decisions](#key-design-decisions)
- [Future Improvements](#future-improvements)
- [Prerequisites](#prerequisites)
- [Project Structure](#project-structure)
- [Running Locally (without Docker)](#running-locally-without-docker)
- [Running with Docker](#running-with-docker)
- [Docker-related Files](#docker-related-files)
- [Architecture Notes](#architecture-notes)

---

## Prerequisites

### For local development
| Tool | Version |
|------|---------|
| .NET SDK | 8.0 |
| Node.js | 20 or 22 LTS |
| Angular CLI | 22.x (`npm install -g @angular/cli`) |
| PostgreSQL | 15 or 16 |

### For Docker
| Tool | Notes |
|------|-------|
| Docker Desktop | Latest (includes Docker Compose v2) |
| Docker Compose | v2 (`docker compose`) or v1 (`docker-compose`) |

---

## Project Structure

```
Hotel_reservation_system/
├── API/                                     ← ASP.NET Core solution
│   ├── Dockerfile                           ← Multi-stage API image
│   ├── .dockerignore
│   └── src/
│       ├── HotelReservation.API/            ← Web API entry point
│       ├── HotelReservation.Application/    ← Application layer (class library)
│       ├── HotelReservation.Domain/         ← Domain layer (class library)
│       └── HotelReservation.Infrastructure/ ← Data access, EF Core (class library)
│
├── Presentation/                            ← Angular 22 frontend
│   ├── Dockerfile                           ← Multi-stage Angular + Nginx image
│   ├── nginx.conf                           ← Nginx config (SPA + reverse proxy)
│   ├── proxy.conf.json                      ← Dev-server proxy (local development)
│   ├── .dockerignore
│   └── src/environments/
│       ├── environment.ts                   ← Local development
│       ├── environment.prod.ts              ← Standard production
│       └── environment.docker.ts            ← Docker environment
│
├── docker-compose.yml                       ← Orchestrates all containers
├── .env                                     ← Environment variable overrides
├── build-and-run.sh                         ← One-command build & run (Linux/macOS)
├── build-and-run.bat                        ← One-command build & run (Windows)
└── README.md
```

---

## Running Locally (without Docker)

### 1. Configure the database

Make sure PostgreSQL is running locally, then verify the connection string in
`API/src/HotelReservation.API/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=5432;User Id=postgres;Password=secret;Database=HotelReservationDb;"
}
```

### 2. Start the API

```bash
cd API
dotnet run --project src/HotelReservation.API/HotelReservation.API.csproj
```

The API starts on `http://localhost:5026`.

**Database migrations and seeding run automatically on startup.**
The database is created and seeded with 26 sample rooms and an admin user:

| Field    | Value       |
|----------|-------------|
| Username | `admin`     |
| Password | `Admin@123` |

### 3. Start the Angular frontend

```bash
cd Presentation
npm install
npm start
```

The frontend is available at `http://localhost:4200`.

The dev-server proxy (`proxy.conf.json`) forwards both `/api` and `/images` requests to
`http://localhost:5026`, so uploaded room photos and the default image are served correctly
without any extra configuration.

### 4. Swagger (development only)

Available at `http://localhost:5026/swagger` when the API runs in `Development` mode.

---

## Running with Docker

### 1. Configure environment variables (optional)

All defaults are already set in `.env`. Edit it to customise ports, secrets, or credentials
before running:

```env
# Database
DB_USER=postgres
DB_PASSWORD=secret
DB_NAME=HotelReservationDb

# Exposed ports on the host
API_PORT=8080
FRONTEND_PORT=4200

# JWT — change the secret before deploying to production!
JWT_SECRET=your-super-secret-key-change-this-in-production-min-32-chars
```

### 2. Build and start everything

#### Linux / macOS

```bash
chmod +x build-and-run.sh
./build-and-run.sh
```

#### Windows

```bat
build-and-run.bat
```

Both scripts:
1. Build the API Docker image (multi-stage .NET 8 build).
2. Build the Angular Docker image (multi-stage Node build + Nginx serve).
3. Start all three containers (`hotel-db`, `hotel-api`, `hotel-frontend`).
4. Print the access URLs once everything is ready.

### 3. Access the application

| Service  | URL                          |
|----------|------------------------------|
| Frontend | `http://localhost:4200`      |
| API      | `http://localhost:8080`      |

Admin credentials: `admin` / `Admin@123`

### 4. Stop containers

```bash
docker compose down
```

### 5. Rebuild images after code changes

```bash
docker compose build --no-cache
docker compose up -d
```

Or rebuild a single service:

```bash
docker compose build hotel-api
docker compose up -d hotel-api
```

### 6. Remove containers and volumes

```bash
# Remove containers and networks (keeps volumes/data intact)
docker compose down

# Remove containers, networks, AND all data (database + uploaded images)
docker compose down -v
```

### 7. View logs

```bash
docker compose logs -f                 # all services
docker compose logs -f hotel-api       # API only
docker compose logs -f hotel-frontend  # Nginx / Angular
docker compose logs -f hotel-db        # PostgreSQL
```

---

## Docker-related Files

| File | Description |
|------|-------------|
| `API/Dockerfile` | Multi-stage image: `dotnet/sdk:8.0` builds and publishes in Release mode; `dotnet/aspnet:8.0` is the lean runtime. |
| `API/.dockerignore` | Excludes `bin/`, `obj/`, `.idea/`, and other build artifacts from the Docker build context. |
| `Presentation/Dockerfile` | Multi-stage image: `node:22-alpine` builds the Angular app with the `docker` configuration; `nginx:1.27-alpine` serves it. |
| `Presentation/nginx.conf` | Serves the Angular SPA, reverse-proxies `/api/*` to the API container, and reverse-proxies `/images/*` to the API's static file middleware. |
| `Presentation/.dockerignore` | Excludes `node_modules/`, `dist/`, and `.angular/` from the build context. |
| `Presentation/proxy.conf.json` | Angular dev-server proxy — forwards `/api` and `/images` to `http://localhost:5026` for local development. |
| `Presentation/src/environments/environment.docker.ts` | Docker-specific Angular environment (production build with relative API paths proxied by Nginx). |
| `docker-compose.yml` | Defines three services: `hotel-db` (PostgreSQL 16), `hotel-api` (ASP.NET Core 8), `hotel-frontend` (Nginx). Uses two named volumes: `postgres-data` (database) and `room-images` (uploaded photos). |
| `.env` | Default environment variable values consumed by `docker-compose.yml`. |
| `build-and-run.sh` | One-command build + run script for Linux/macOS. |
| `build-and-run.bat` | One-command build + run script for Windows. |

---

## Architecture Notes

- All `/api/*` and `/images/*` traffic from the browser goes through the Nginx container, which reverse-proxies it to `hotel-api` on the internal Docker network. The browser never speaks directly to the API container.
- Room photo URLs are stored as relative paths (e.g. `/images/rooms/uuid.jpg`), so they resolve correctly in both local development (proxied by the Angular dev server) and Docker (proxied by Nginx) without any host dependency.
- The default room image (`hotel_default_pic.avif`) is served by the API's static file middleware alongside uploaded photos, keeping all image serving in one place.
- The database is backed by the `postgres-data` Docker volume and survives container recreation.
- Uploaded room images are stored in the `room-images` Docker volume (mounted at `/app/wwwroot/images/rooms` in the API container) and also survive container recreation.
- Database migrations and data seeding run automatically when the API starts (`InitialiseDatabase()` in `Program.cs`).
