using HotelReservation.Domain.Enums;

namespace HotelReservation.Domain.Entities;

public class Room : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public RoomCapacity Capacity { get; set; }
    public decimal PricePerNight { get; set; }
    public string? Photo { get; set; } 

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
