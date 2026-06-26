using HotelReservation.Domain.Enums;

namespace HotelReservation.Application.DTOs;

public class ReservationFilterDto
{
    public DateOnly? CreatedAt { get; set; }
    public string? SearchQuery { get; set; }
}