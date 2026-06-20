namespace HotelReservation.Domain.Entities;

public class Reservation : BaseEntity
{
    public int RoomId { get; set; }
    public Room? Room { get; set; }
    
    public DateOnly CheckIn { get; set; }
    public DateOnly CheckOut { get; set; }
    
    public string GuestName { get; set; } = string.Empty;
    public string GuestEmail { get; set; } = string.Empty;
    public string GuestNumber { get; set; } = string.Empty;
}
