using HotelReservation.Domain.Entities;

namespace HotelReservation.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(ApplicationUser user);
}
