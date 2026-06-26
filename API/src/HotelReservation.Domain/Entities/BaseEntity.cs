using System.ComponentModel.DataAnnotations;

namespace HotelReservation.Domain.Entities;

public abstract class BaseEntity
{
    [Key]
    public int Id { get; set; }
    public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public DateOnly? UpdatedAt { get; set; }
}
