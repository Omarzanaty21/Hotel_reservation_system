using HotelReservation.Application.Dtos;
using HotelReservation.Domain.Entities;

namespace HotelReservation.Application.Interfaces;

public interface IReservationService
{
    Task<Reservation> CreateReservationAsync(CreateReservationDto createReservationDto);
}