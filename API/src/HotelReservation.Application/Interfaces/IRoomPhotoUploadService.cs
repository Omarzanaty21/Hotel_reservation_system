namespace HotelReservation.Application.Interfaces;
using Microsoft.AspNetCore.Http;

public interface IRoomPhotoUploadService
{
    Task<string> UploadRoomImageAsync(IFormFile? file);
}