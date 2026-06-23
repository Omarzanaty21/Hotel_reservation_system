using Microsoft.Extensions.DependencyInjection;
using HotelReservation.Application.Services;
using HotelReservation.Application.Interfaces;

namespace HotelReservation.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register AutoMapper and scan the current assembly for Profiles using the updated syntax
        services.AddAutoMapper(cfg => cfg.AddMaps(typeof(ServiceCollectionExtensions).Assembly));

        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IReservationService, ReservationService>();

        return services;
    }
}
