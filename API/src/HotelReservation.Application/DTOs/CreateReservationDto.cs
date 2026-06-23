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
}
