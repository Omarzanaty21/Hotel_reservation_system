using System.Linq.Expressions;
using HotelReservation.Application.DTOs;
using HotelReservation.Application.Interfaces;
using HotelReservation.Domain.Common;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;

namespace HotelReservation.Application.Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;

    public RoomService(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<PagedResult<Room>> GetAvailableRoomsAsync(RoomFilterDto filter, int pageIndex, int pageSize)
    {
        var roomFilter = BuildRoomFilter(filter);

        return await _roomRepository.GetPagedAsync(
            pageIndex,
            pageSize,
            roomFilter,
            includeReservations: true);
    }

    private static Expression<Func<Room, bool>>? BuildRoomFilter(RoomFilterDto filter)
    {
        if (filter.CheckIn == null && filter.CheckOut == null && filter.Capacity == null)
        {
            return null;
        }

        return room =>
            (filter.Capacity == null || room.Capacity == filter.Capacity) &&
            (filter.CheckIn == null || filter.CheckOut == null ||
                room.Reservations.All(reservation =>
                    reservation.CheckOut <= filter.CheckIn.Value || reservation.CheckIn >= filter.CheckOut.Value));
    }
}
