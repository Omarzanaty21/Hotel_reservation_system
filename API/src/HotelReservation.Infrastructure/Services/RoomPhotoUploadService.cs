namespace HotelReservation.Infrastructure.Services;

using HotelReservation.Application.Interfaces;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Hosting;

public class RoomPhotoUploadService : IRoomPhotoUploadService
{
    private readonly IWebHostEnvironment _environment;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private static readonly string[] AllowedExtensions =
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".webp"
    };

    public RoomPhotoUploadService(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
    {
        _environment = environment;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> UploadRoomImageAsync(IFormFile photo)
    {
        if (photo == null || photo.Length == 0)
            throw new ArgumentException("A room photo is required.");

        var extension = Path.GetExtension(photo.FileName).ToLowerInvariant();

        if (!AllowedExtensions.Contains(extension))
            throw new ArgumentException(
                "Only JPG, JPEG, PNG and WEBP images are allowed.");

        if (!photo.ContentType.StartsWith("image/"))
            throw new ArgumentException(
                "The uploaded file must be an image.");

        const long maxFileSize = 5 * 1024 * 1024;

        if (photo.Length > maxFileSize)
            throw new ArgumentException(
                "The image size cannot exceed 5 MB.");

        var imagesFolder = Path.Combine(
            _environment.WebRootPath,
            "images",
            "rooms");

        Directory.CreateDirectory(imagesFolder);

        var fileName = $"{Guid.NewGuid()}{extension}";

        var filePath = Path.Combine(
            imagesFolder,
            fileName);

        await using var stream = new FileStream(
            filePath,
            FileMode.Create);

        await photo.CopyToAsync(stream);

        var request = _httpContextAccessor.HttpContext!.Request;

        return $"{request.Scheme}://{request.Host}/images/rooms/{fileName}";
    }
}