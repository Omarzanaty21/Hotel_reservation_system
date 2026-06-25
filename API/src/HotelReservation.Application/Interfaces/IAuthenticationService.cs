using HotelReservation.Application.DTOs.Authentication;

namespace HotelReservation.Application.Interfaces;

public interface IAuthenticationService
{
    Task<AuthenticationResponseDto> LoginAsync(LoginDto loginDto);
    Task<LogoutResponseDto> LogoutAsync();
}
