using System.ComponentModel.DataAnnotations;

namespace HotelReservation.Application.Dtos;

public class CreateReservationDto
{
    [Required( ErrorMessage = "You have to choose a room!")]
    public int? RoomId { get; set; }
    [Required( ErrorMessage = "You have to choose a checkIn date!")]
    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    public DateOnly? CheckIn { get; set; }
    [Required( ErrorMessage = "You have to choose a checkOut date!")]
    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    public DateOnly? CheckOut { get; set; }
    [Required( ErrorMessage = "You have to enter a guest name!")]
    public string? GuestName { get; set; } = string.Empty;
    [Required( ErrorMessage = "You have to enter a guest email!")]
    [EmailAddress( ErrorMessage = "Invalid email format.")]
    public string? GuestEmail { get; set; } = string.Empty;
    [Required( ErrorMessage = "You have to enter a phone number!")]
    [Phone( ErrorMessage = "Invalid phone number format.")]
    public string? GuestNumber { get; set; } = string.Empty;
}
