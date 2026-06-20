using HotelReservation.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HotelReservation.Infrastructure.Extensions;

public static class HostExtensions
{
    public static void InitialiseDatabase(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();

            // 1. Run Pending Migrations
            context.Database.Migrate();

            // 2. Data Seeding Section
            // Seed sample rooms with photos if none exist
            if (!context.Rooms.Any())
            {
                var rooms = new[]
                {
                    new Domain.Entities.Room { Name = "Seaside Deluxe", Description = "Ocean view deluxe room", Capacity = Domain.Enums.RoomCapacity.King, PricePerNight = 249.99m, Photo = "" },
                    new Domain.Entities.Room { Name = "City Comfort", Description = "Comfortable city-center room", Capacity = Domain.Enums.RoomCapacity.Queen, PricePerNight = 159.50m, Photo = "" },
                    new Domain.Entities.Room { Name = "Cozy Single", Description = "Compact single room", Capacity = Domain.Enums.RoomCapacity.Single, PricePerNight = 79.99m, Photo = "" },
                    new Domain.Entities.Room { Name = "Family Suite", Description = "Spacious suite for families", Capacity = Domain.Enums.RoomCapacity.Double, PricePerNight = 199.00m, Photo = "" },
                    new Domain.Entities.Room { Name = "Executive King", Description = "Business-friendly room", Capacity = Domain.Enums.RoomCapacity.King, PricePerNight = 219.00m, Photo = "" },
                    new Domain.Entities.Room { Name = "Romantic Queen", Description = "Perfect for couples", Capacity = Domain.Enums.RoomCapacity.Queen, PricePerNight = 179.00m, Photo = "" },
                    new Domain.Entities.Room { Name = "Alpine Retreat", Description = "Mountain-view room with fireplace", Capacity = Domain.Enums.RoomCapacity.King, PricePerNight = 289.00m, Photo = "" },
                    new Domain.Entities.Room { Name = "Garden View", Description = "Ground-floor room overlooking the garden", Capacity = Domain.Enums.RoomCapacity.Double, PricePerNight = 139.00m, Photo = "" },
                    new Domain.Entities.Room { Name = "Skyline Loft", Description = "Penthouse loft with panoramic city views", Capacity = Domain.Enums.RoomCapacity.King, PricePerNight = 399.00m, Photo = "" },
                    new Domain.Entities.Room { Name = "Budget Traveler", Description = "Affordable room for solo travelers", Capacity = Domain.Enums.RoomCapacity.Single, PricePerNight = 59.99m, Photo = "" },
                    new Domain.Entities.Room { Name = "Sunset Suite", Description = "West-facing suite with sunset views", Capacity = Domain.Enums.RoomCapacity.King, PricePerNight = 329.00m, Photo = "" },
                    new Domain.Entities.Room { Name = "Harbor View", Description = "Room overlooking the marina harbor", Capacity = Domain.Enums.RoomCapacity.Queen, PricePerNight = 189.00m, Photo = "" },
                    new Domain.Entities.Room { Name = "Zen Garden Room", Description = "Japanese-inspired tranquil retreat", Capacity = Domain.Enums.RoomCapacity.Double, PricePerNight = 175.00m, Photo = "" },
                    new Domain.Entities.Room { Name = "Backpacker Bunk", Description = "Simple and clean budget accommodation", Capacity = Domain.Enums.RoomCapacity.Single, PricePerNight = 45.00m, Photo = "" },
                    new Domain.Entities.Room { Name = "Royal Chamber", Description = "Luxurious room with premium amenities", Capacity = Domain.Enums.RoomCapacity.King, PricePerNight = 499.00m, Photo = "" },
                    new Domain.Entities.Room { Name = "Poolside Cabana", Description = "Direct access to the swimming pool", Capacity = Domain.Enums.RoomCapacity.Double, PricePerNight = 209.00m, Photo = "" },
                    new Domain.Entities.Room { Name = "Artist's Loft", Description = "Bohemian-styled creative space", Capacity = Domain.Enums.RoomCapacity.Queen, PricePerNight = 165.00m, Photo = "" },
                    new Domain.Entities.Room { Name = "Traveler's Nest", Description = "Cozy room with all travel essentials", Capacity = Domain.Enums.RoomCapacity.Single, PricePerNight = 69.99m, Photo = "" },
                    new Domain.Entities.Room { Name = "Honeymoon Haven", Description = "Romantic getaway with spa bath", Capacity = Domain.Enums.RoomCapacity.King, PricePerNight = 349.00m, Photo = "" },
                    new Domain.Entities.Room { Name = "Lakeview Lodge", Description = "Rustic charm with serene lake views", Capacity = Domain.Enums.RoomCapacity.Double, PricePerNight = 185.00m, Photo = "" },
                    new Domain.Entities.Room { Name = "Metro Express", Description = "Modern room near transit hub", Capacity = Domain.Enums.RoomCapacity.Single, PricePerNight = 89.00m, Photo = "" },
                    new Domain.Entities.Room { Name = "Presidential Suite", Description = "Top-floor suite with butler service", Capacity = Domain.Enums.RoomCapacity.King, PricePerNight = 599.00m, Photo = "" },
                    new Domain.Entities.Room { Name = "Orchard Room", Description = "Countryside room surrounded by orchards", Capacity = Domain.Enums.RoomCapacity.Queen, PricePerNight = 145.00m, Photo = "" },
                    new Domain.Entities.Room { Name = "Minimalist Studio", Description = "Clean Scandinavian-design studio", Capacity = Domain.Enums.RoomCapacity.Double, PricePerNight = 129.00m, Photo = "" },
                    new Domain.Entities.Room { Name = "Coral Reef Room", Description = "Tropical-themed room with aquarium wall", Capacity = Domain.Enums.RoomCapacity.Queen, PricePerNight = 199.99m, Photo = "" },
                    new Domain.Entities.Room { Name = "Vintage Heritage", Description = "Classic room with antique furnishings", Capacity = Domain.Enums.RoomCapacity.King, PricePerNight = 275.00m, Photo = "" },
                };

                context.Rooms.AddRange(rooms);
                context.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<ApplicationDbContext>>();
            logger.LogError(ex, "An error occurred during database migration or seeding.");
        }
    }
}
