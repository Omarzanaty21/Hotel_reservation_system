using HotelReservation.Application.DTOs;

namespace HotelReservation.Application.Dtos;

public class ReservationDto
{
    public DateOnly CheckIn { get; set; }
    public DateOnly CheckOut { get; set; }
    public RoomDto Room {get; set;}
    public string GuestName { get; set; } = string.Empty;
    public string GuestEmail { get; set; } = string.Empty;
    public string GuestNumber { get; set; } = string.Empty;
}