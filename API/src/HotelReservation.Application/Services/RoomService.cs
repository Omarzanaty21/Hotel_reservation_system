using System.Linq.Expressions;
using AutoMapper;
using HotelReservation.Application.Dtos;
using HotelReservation.Application.DTOs;
using HotelReservation.Application.Interfaces;
using HotelReservation.Domain.Common;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;

namespace HotelReservation.Application.Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;

    public RoomService(IRoomRepository roomRepository, IMapper mapper)
    {
        _roomRepository = roomRepository;
        _mapper = mapper;
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
    
    public async Task<Room> CreateRoomAsync(CreateRoomDto room)
    {       
        Room createdRoom = _mapper.Map<Room>(room);

        await _roomRepository.AddAsync(createdRoom);
        return createdRoom;
    }

    private static Expression<Func<Room, bool>>? BuildRoomFilter(RoomFilterDto filter)
    {     
        return room =>
            (filter.Capacity == null || room.Capacity == filter.Capacity) &&
            (filter.CheckIn == null || filter.CheckOut == null ||
                room.Reservations.All(reservation =>
                    reservation.CheckOut <= filter.CheckIn.Value || reservation.CheckIn >= filter.CheckOut.Value));
    }
}
