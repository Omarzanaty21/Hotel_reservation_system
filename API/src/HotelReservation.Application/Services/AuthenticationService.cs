using HotelReservation.Application.DTOs.Authentication;
using HotelReservation.Application.Interfaces;
using HotelReservation.Domain.Interfaces;

namespace HotelReservation.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IGenericRepository<Domain.Entities.ApplicationUser> _userRepository;
    private readonly IHashingService _hashingService;
    private readonly IJwtService _jwtService;

    public AuthenticationService(
        IGenericRepository<Domain.Entities.ApplicationUser> userRepository,
        IHashingService hashingService,
        IJwtService jwtService)
    {
        _userRepository = userRepository;
        _hashingService = hashingService;
        _jwtService = jwtService;
    }

    public Task<AuthenticationResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = _userRepository
            .Find(u => u.UserName == loginDto.UserName)
            .FirstOrDefault();

        if (user is null)
            throw new UnauthorizedAccessException("Invalid username or password.");

        var isPasswordValid = _hashingService.VerifyPassword(loginDto.Password, user.PasswordHash);

        if (!isPasswordValid)
            throw new UnauthorizedAccessException("Invalid username or password.");

        var token = _jwtService.GenerateToken(user);

        return Task.FromResult(new AuthenticationResponseDto { Token = token });
    }

    public Task<LogoutResponseDto> LogoutAsync()
    {
        return Task.FromResult(new LogoutResponseDto { Message = "Logged out successfully." });
    }
}
