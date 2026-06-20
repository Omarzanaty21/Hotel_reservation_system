using AutoMapper;
using HotelReservation.Application.DTOs;
using HotelReservation.Application.Interfaces;
using HotelReservation.Domain.Common;
using HotelReservation.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomManagementController : ControllerBase
{
    private readonly IRoomService _roomService;
    private readonly IMapper _mapper;

    public RoomManagementController(IRoomService roomService, IMapper mapper)
    {
        _roomService = roomService;
        _mapper = mapper;
    }

    [HttpPost("Rooms")]
    public async Task<IActionResult> Index([FromBody] RoomFilterDto filter, [FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
    {
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
}
