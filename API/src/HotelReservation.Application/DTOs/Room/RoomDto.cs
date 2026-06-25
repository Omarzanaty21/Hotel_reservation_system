using HotelReservation.Domain.Enums;

namespace HotelReservation.Application.DTOs;

public sealed class RoomDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public RoomCapacity Capacity { get; init; }
    public decimal PricePerNight { get; init; }
    public string? Photo { get; init; } = string.Empty;
}
