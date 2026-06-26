using HotelReservation.Application.Interfaces;
using HotelReservation.Application.Services;
using HotelReservation.Application.Settings;
using HotelReservation.Domain.Interfaces;
using HotelReservation.Infrastructure.Data;
using HotelReservation.Infrastructure.Repositories;
using HotelReservation.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelReservation.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IHashingService, HashingService>();
        services.AddScoped<IRoomPhotoUploadService, RoomPhotoUploadService>();
        services.AddScoped<IJwtService, JwtService>();

        // Bind JwtSettings from configuration
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        return services;
    }
}
