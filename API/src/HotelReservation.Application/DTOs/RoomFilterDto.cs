using HotelReservation.Domain.Enums;

namespace HotelReservation.Application.DTOs;

public sealed class RoomFilterDto
{
    public DateOnly? CheckIn { get; set; }
    public DateOnly? CheckOut { get; set; }
    public RoomCapacity? Capacity { get; set; }
}
