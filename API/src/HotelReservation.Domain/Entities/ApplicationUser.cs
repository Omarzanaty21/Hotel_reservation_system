using System.ComponentModel.DataAnnotations;

namespace HotelReservation.Domain.Entities;

public class ApplicationUser : BaseEntity
{
    [Required(AllowEmptyStrings = false)]
    [StringLength(100)]
    public string UserName { get;  set; }
    [Required(AllowEmptyStrings = false)]
    [StringLength(512)]
    public string PasswordHash { get;  set; }
    [Required]
    public bool IsAdmin { get;  set; }
}