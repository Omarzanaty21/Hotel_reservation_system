using Application.DTOs.Common;
using AutoMapper;
using HotelReservation.Application.Dtos;
using HotelReservation.Application.DTOs;
using HotelReservation.Application.Exceptions;
using HotelReservation.Application.Interfaces;
using HotelReservation.Domain.Common;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomManagementController : ControllerBase
{
    private readonly IRoomService _roomService;
    private readonly IReservationService _reservationService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRoomPhotoUploadService _roomPhotoUploadService;

    public RoomManagementController(IRoomService roomService,
                                    IReservationService reservationService,
                                    IMapper mapper,
                                    IUnitOfWork unitOfWork,
                                    IRoomPhotoUploadService roomPhotoUploadService)
    {
        _roomService = roomService;
        _reservationService = reservationService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _roomPhotoUploadService = roomPhotoUploadService;
    }
    #region Rooms
    [HttpGet("Rooms")]
    public async Task<IActionResult> Index([FromQuery] RoomFilterDto filter, [FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
    {
        if(filter.CheckIn >= filter.CheckOut)
          throw new InvalidTimeSpanException("Check-in date must be before check-out date.");

        var pagedAvailableRooms = await _roomService.GetAvailableRoomsAsync(filter, pageIndex, pageSize);

        var roomsDto = _mapper.Map<IReadOnlyList<RoomDto>>(pagedAvailableRooms.Items);

        var response = new PagedResult<RoomDto>
        {
            Items = roomsDto,
            TotalCount = pagedAvailableRooms.TotalCount,
            PageIndex = pagedAvailableRooms.PageIndex,
            PageSize = pagedAvailableRooms.PageSize
        };

        return Ok(response);
    }
    [Authorize("AdminOnly")]
    [HttpPost("Rooms")]
    public async Task<IActionResult> CreateRoom([FromForm] CreateRoomDto createRoomDto)
    {
        string? photoUrl = null;
        if (createRoomDto.PhotoUpload is not null)
        {
            photoUrl = await _roomPhotoUploadService.UploadRoomImageAsync(createRoomDto.PhotoUpload);
        }

        // Clear the PhotoUpload property to avoid serialization issues
        createRoomDto.PhotoUpload = null; 
        createRoomDto.Photo = photoUrl;

        var room = await _roomService.CreateRoomAsync(createRoomDto);

        await _unitOfWork.SaveChangesAsync();

        return CreatedAtAction(nameof(CreateRoom), new { id = room.Id }, room);
    }

    #endregion

    #region Reservations
    [Authorize("AdminOnly")]
    [HttpGet("Reservations")]
    public async Task<IActionResult> Index([FromQuery] ReservationFilterDto filter, [FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
    {

        var pagedReservations = await _reservationService.GetReservationsAsync(filter, pageIndex, pageSize);

        var reservationsDto = _mapper.Map<IReadOnlyList<ReservationDto>>(pagedReservations.Items);

        var response = new PagedResult<ReservationDto>
        {
            Items = reservationsDto,
            TotalCount = pagedReservations.TotalCount,
            PageIndex = pagedReservations.PageIndex,
            PageSize = pagedReservations.PageSize
        };

        return Ok(response);
    }

    [HttpPost("Reservations")]
    public async Task<IActionResult> CreateReservation([FromBody] CreateReservationDto createReservationDto)
    {
        ValidateReservationDates(createReservationDto.CheckIn, createReservationDto.CheckOut);

        var reservation = await _reservationService.CreateReservationAsync(createReservationDto);

        return CreatedAtAction(nameof(CreateReservation), new { id = reservation.Id }, reservation);
    }
    [Authorize("AdminOnly")]
    [HttpDelete("Reservations/{reservationId}")]
    public async Task<IActionResult> DeleteReservation(int reservationId)
    {
        await _reservationService.DeleteReservationAsync(reservationId);
        await _unitOfWork.SaveChangesAsync();
        
        return NoContent();
    }
    #endregion

    #region Helpers
    private void ValidateReservationDates(DateOnly checkIn, DateOnly checkOut)
    {
        if(checkIn < DateOnly.FromDateTime(DateTime.Now))
        {
            throw new InvalidTimeSpanException("Check-in date cannot be in the past.");
        }
        if (checkOut < DateOnly.FromDateTime(DateTime.Now))
        {
            throw new InvalidTimeSpanException("Check-out date cannot be in the past.");
        }
        if (checkIn >= checkOut)
        {
            throw new InvalidTimeSpanException("Check-out date must be after check-in date.");
        }
        if(checkOut > checkIn.AddMonths(1))
        {
            throw new InvalidTimeSpanException("Reservations cannot be made for more than 30 days.");
        }
    }
    #endregion
}
    
