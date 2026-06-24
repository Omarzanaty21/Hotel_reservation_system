using Application.DTOs.Common;
using AutoMapper;
using HotelReservation.Application.Dtos;
using HotelReservation.Application.DTOs;
using HotelReservation.Application.Exceptions;
using HotelReservation.Application.Interfaces;
using HotelReservation.Domain.Common;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;
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

    public RoomManagementController(IRoomService roomService,
                                    IReservationService reservationService,
                                    IMapper mapper,
                                    IUnitOfWork unitOfWork)
    {
        _roomService = roomService;
        _reservationService = reservationService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
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

    #endregion

    #region Reservations

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

        await _unitOfWork.SaveChangesAsync();

        return CreatedAtAction(nameof(CreateReservation), new { id = reservation.Id }, reservation);
    }
    #endregion

    #region Helpers
    private void ValidateReservationDates(DateOnly? checkIn, DateOnly? checkOut)
    {
        if(checkIn < DateOnly.FromDateTime(DateTime.Now))
        {
            throw new InvalidTimeSpanException("Check-in date cannot be in the past.");
        }
        else if (checkIn >= checkOut)
        {
            throw new InvalidTimeSpanException("Check-out date must be after check-in date.");
        }
    }
    #endregion
}
    
