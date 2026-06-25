using AutoMapper;
using HotelReservation.Application.Dtos;
using HotelReservation.Application.DTOs;
using HotelReservation.Domain.Entities;

namespace HotelReservation.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        #region Room
        CreateMap<Room, RoomDto>().ReverseMap();
        CreateMap<Room, CreateRoomDto>().ReverseMap();
        #endregion
     
        #region Reservation
        CreateMap<Reservation, CreateReservationDto>().ReverseMap();
        CreateMap<Reservation, ReservationDto>().ReverseMap();
        #endregion
    
    }
}
