using System.Data;
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
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ReservationService(
        IReservationRepository reservationRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _reservationRepository = reservationRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Reservation> CreateReservationAsync(CreateReservationDto reservation)
    {
        await _unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable);

        try
        {
            // Re-check availability inside the serializable transaction to prevent double booking
            var isRoomAvailable = IsRoomAvailable(reservation.RoomId, reservation.CheckIn, reservation.CheckOut);

            if (!isRoomAvailable)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new OverlappingDatesException("The room is already booked for the selected dates.");
            }

            Reservation createdReservation = _mapper.Map<Reservation>(reservation);

            await _reservationRepository.AddAsync(createdReservation);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            return createdReservation;
        }
        catch (OverlappingDatesException)
        {
            throw;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
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

    public async Task DeleteReservationAsync(int reservationId)
    {
        var reservation = await _reservationRepository.GetByIdAsync(reservationId);
        if (reservation == null)
        {
            throw new KeyNotFoundException($"Reservation not found.");
        }

        _reservationRepository.Delete(reservation);
    }

    private static Expression<Func<Reservation, bool>>? BuildReservationFilter(ReservationFilterDto filter)
    {
        return reservation =>
            (filter.SearchQuery == null || reservation.GuestName.ToLower().Contains(filter.SearchQuery.ToLower()) ||
            reservation.GuestEmail.ToLower().Contains(filter.SearchQuery.ToLower()) ||
            reservation.GuestNumber.ToLower().Contains(filter.SearchQuery.ToLower())) &&
            (filter.CreatedAt == null || reservation.CreatedAt == filter.CreatedAt);
    }

    private bool IsRoomAvailable(int? roomId, DateOnly? checkIn, DateOnly? checkOut)
    {
        return !_reservationRepository.Find(r =>
            r.RoomId == roomId &&
            r.CheckIn < checkOut &&
            r.CheckOut > checkIn).Any();
    }
}