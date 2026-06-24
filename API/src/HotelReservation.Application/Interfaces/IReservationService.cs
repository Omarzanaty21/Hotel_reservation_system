using HotelReservation.Application.Dtos;
using HotelReservation.Application.DTOs;
using HotelReservation.Domain.Common;
using HotelReservation.Domain.Entities;

namespace HotelReservation.Application.Interfaces;

public interface IReservationService
{
    Task<PagedResult<Reservation>> GetReservationsAsync(ReservationFilterDto filter, int pageIndex, int pageSize);
    Task<Reservation> CreateReservationAsync(CreateReservationDto createReservationDto);
    Task DeleteReservationAsync(int reservationId);
}