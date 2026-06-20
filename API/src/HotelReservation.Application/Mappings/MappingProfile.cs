using AutoMapper;
using HotelReservation.Application.DTOs;
using HotelReservation.Domain.Entities;

namespace HotelReservation.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Room, RoomDto>();
    }
}
