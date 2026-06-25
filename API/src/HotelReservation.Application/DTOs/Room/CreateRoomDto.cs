using System.ComponentModel.DataAnnotations;
using System.Runtime;
using HotelReservation.Domain.Enums;
using Microsoft.AspNetCore.Http;


namespace HotelReservation.Application.Dtos;
public class CreateRoomDto{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Room name is required.")]
    public string Name { get; set; } = string.Empty;
    [Required(AllowEmptyStrings = false, ErrorMessage = "Room description is required.")]
    public string Description { get; set; } = string.Empty;
    [Required(AllowEmptyStrings = false, ErrorMessage = "Room capacity is required.")]
    [EnumDataType(typeof(RoomCapacity), ErrorMessage = "Invalid room capacity.")]
    public RoomCapacity Capacity { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "Price per night is required.")]
    [Range(1 , double.MaxValue, ErrorMessage = "Invalid price format.")]
    public decimal PricePerNight { get; set; }
    public IFormFile? PhotoUpload { get; set; }
    public string? Photo { get; set; }
}
   
