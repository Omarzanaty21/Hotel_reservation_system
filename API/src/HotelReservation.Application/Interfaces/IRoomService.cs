using HotelReservation.Application.DTOs;
using HotelReservation.Domain.Common;
using HotelReservation.Domain.Entities;

namespace HotelReservation.Application.Interfaces;

public interface IRoomService
{
    Task<PagedResult<Room>> GetAvailableRoomsAsync(RoomFilterDto filter, int pageIndex, int pageSize);
}
