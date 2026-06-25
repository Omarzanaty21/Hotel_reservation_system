using System.ComponentModel.DataAnnotations;

namespace HotelReservation.Application.DTOs.Authentication;

public sealed class LoginDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Username is required.")]
    public string UserName { get; set; } 
    [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required.")]
    public string Password { get; set; } 
}
