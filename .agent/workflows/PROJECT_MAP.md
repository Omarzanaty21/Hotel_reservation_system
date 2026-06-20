---
description: Full architectural overview and project map for the Hotel Reservation System
---

# Hotel Reservation System — Project Map

## Tech Stack

| Layer    | Technology                         |
| -------- | ---------------------------------- |
| Backend  | .NET 8 Web API, Onion Architecture |
| ORM      | Entity Framework Core (SQLite)     |
| Mapping  | AutoMapper                         |
| Frontend | Angular 22, TypeScript 6           |
| Database | SQLite (`HotelReservation.db`)     |

---

## Repository Layout

```
Hotel_reservation_system/
├── API/                          # .NET backend
│   ├── HotelReservation.sln
│   └── src/
│       ├── HotelReservation.Domain/         # Core domain (innermost)
│       │   ├── Entities/       Room, Reservation, BaseEntity
│       │   ├── Enums/          RoomCapacity
│       │   ├── Common/         PagedResult<T>
│       │   └── Interfaces/    IGenericRepository<T>, IRoomRepository,
│       │                       IReservationRepository, IUnitOfWork
│       ├── HotelReservation.Application/    # Use-cases & DTOs
│       │   ├── DTOs/           RoomDto, RoomFilterDto
│       │   ├── Interfaces/    IRoomService
│       │   ├── Mappings/      AutoMapper profiles
│       │   ├── Services/      RoomService
│       │   └── Extensions/    ServiceCollectionExtensions (DI)
│       ├── HotelReservation.Infrastructure/ # Data access
│       │   ├── Data/           ApplicationDbContext (+ seeding)
│       │   ├── Repositories/  GenericRepository<T>, RoomRepository,
│       │   │                   ReservationRepository, UnitOfWork
│       │   ├── Migrations/    EF Core migrations
│       │   └── Extensions/    ServiceCollectionExtensions (DI),
│       │                       ServiceProviderExtensions (auto-migrate + seed)
│       └── HotelReservation.API/            # Entry point
│           ├── Program.cs      Startup (CORS, DI, Swagger, static files)
│           ├── Controllers/   RoomManagementController
│           ├── appsettings.json
│           └── wwwroot/        Static files (room photos)
├── Presentation/                 # Angular frontend
│   └── src/
│       ├── app/
│       │   ├── config/         app.config.ts (apiHost resolution)
│       │   ├── core/services/  RoomsService
│       │   ├── features/rooms/ RoomsComponent (listing + filter)
│       │   ├── models/         room.model.ts (Room, PagedResult)
│       │   ├── layout/         (empty — ready for navbar/footer)
│       │   └── shared/         (empty — ready for shared components)
│       └── environments/       environment.ts, environment.prod.ts
└── CONFIGURATION.md              # CORS & API host config guide
```

---

## Domain Entities

### Room

`Name`, `Description`, `Capacity` (enum), `PricePerNight`, `Photo`, navigation → `Reservations`

### Reservation

`RoomId` → `Room`, `CheckIn`, `CheckOut`, `GuestName`, `GuestEmail`, `GuestNumber`

### BaseEntity

`Id` (int)

---

## API Endpoints

| Method | Route                       | Description                           |
| ------ | --------------------------- | ------------------------------------- |
| POST   | `/api/roommanagement/rooms` | Get available rooms (filtered, paged) |

Query params: `pageIndex`, `pageSize`
Body: `{ checkIn?, checkOut?, capacity? }`

Swagger UI: http://localhost:5026/swagger (dev only)

---

## Configuration

- **Database**: `Data Source=HotelReservation.db` (SQLite, in API project root)
- **CORS**: Allowed origins in `appsettings.json → Cors.AllowedOrigins`
- **Frontend API host**: resolved via `window.APP_CONFIG.apiHost` → `localStorage('apiHost')` → default `http://localhost:5026`
- See [CONFIGURATION.md](file:///home/omar/Projects/Hotel_reservation_system/CONFIGURATION.md) for Docker/prod overrides

---

## Dev Commands

```bash
# Backend
cd API && dotnet run --project src/HotelReservation.API     # http://localhost:5026

# Frontend
cd Presentation && npm start                                  # http://localhost:4200

# EF Core Migrations (from API/ directory)
dotnet ef migrations add <Name> --project src/HotelReservation.Infrastructure --startup-project src/HotelReservation.API
dotnet ef database update --project src/HotelReservation.Infrastructure --startup-project src/HotelReservation.API
```

---

## Architecture Notes

- **Onion Architecture**: Domain → Application → Infrastructure → API (dependency flows inward)
- **Database seeding**: Runs automatically on startup via `ServiceProviderExtensions.InitialiseDatabase()` — applies pending migrations + seeds rooms
- **AutoMapper**: Maps `Room` entity → `RoomDto` in the controller layer
- **Generic Repository + Unit of Work**: All data access goes through `IGenericRepository<T>` / `IUnitOfWork`
