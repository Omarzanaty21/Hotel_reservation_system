using System.Linq.Expressions;
using AutoMapper;
using HotelReservation.Application.Dtos;
using HotelReservation.Application.DTOs;
using HotelReservation.Application.Exceptions;
using HotelReservation.Application.Interfaces;
using HotelReservation.Domain.Common;
using HotelReservation.Domain.Entities;
using HotelReservation.Domain.Interfaces;

namespace HotelReservation.Application.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IMapper _mapper;

    public ReservationService(IReservationRepository reservationRepository, IMapper mapper)
    {
        _reservationRepository = reservationRepository;
        _mapper = mapper;
    }

    public async Task<Reservation> CreateReservationAsync(CreateReservationDto reservation)
    {
        // Check for overlapping reservations
        var isRoomAvailable = IsRoomAvailable(reservation.RoomId, reservation.CheckIn, reservation.CheckOut);

        if (!isRoomAvailable)
        {
            throw new OverlappingDatesException("The room is already booked for the selected dates.");
        }

        Reservation createdReservation = _mapper.Map<Reservation>(reservation);

        await _reservationRepository.AddAsync(createdReservation);
        return createdReservation;
    }
    public async Task<PagedResult<Reservation>> GetReservationsAsync(ReservationFilterDto filter, int pageIndex, int pageSize)
    {
        var reservationFilter = BuildReservationFilter(filter);

        return await _reservationRepository.GetPagedAsync(
            pageIndex,
            pageSize,
            reservationFilter,
            includeRoom: true);
    }

    private static Expression<Func<Reservation, bool>>? BuildReservationFilter(ReservationFilterDto filter)
    {
        return reservation =>
            (filter.SearchQuery == null || reservation.GuestName.ToLower().Contains(filter.SearchQuery.ToLower()) ||
            reservation.GuestEmail.ToLower().Contains(filter.SearchQuery.ToLower()) ||
            reservation.GuestNumber.ToLower().Contains(filter.SearchQuery.ToLower()) ) &&
            (filter.CreatedAt == null || reservation.CreatedAt.Date == filter.CreatedAt.Value.Date);
    }

    private bool IsRoomAvailable(int? roomId, DateOnly? checkIn, DateOnly? checkOut)
    {
        return !_reservationRepository.Find(r =>
            r.RoomId == roomId &&
            r.CheckIn < checkOut &&
            r.CheckOut > checkIn).Any();
    }
}