using Isopoh.Cryptography.Argon2;
using HotelReservation.Application.Interfaces;

namespace HotelReservation.Infrastructure.Services;

public class HashingService : IHashingService
{
    public string HashPassword(string password)
    {
        return Argon2.Hash(password);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        return Argon2.Verify(hashedPassword, password);
    }
}