using AutoMapper;
using csharp_api.Application.Dtos;
using csharp_api.Application.Dtos.Auth;
using csharp_api.Application.Dtos.Court;
using csharp_api.Application.Dtos.Reservation;
using csharp_api.Domain.Entities;

namespace csharp_api.Domain.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User mappings
        CreateMap<Users, UserDto>().ReverseMap();
        CreateMap<Users, RegisterDto>().ReverseMap();

        // Court mappings
        CreateMap<Court, CourtDto>().ReverseMap();
        CreateMap<CreateCourtDto, Court>();
        CreateMap<UpdateCourtDto, Court>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // Reservation mappings
        CreateMap<Reservation, ReservationDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Nome))
            .ForMember(dest => dest.CourtName, opt => opt.MapFrom(src => src.Court.Name));
        
        CreateMap<CreateReservationDto, Reservation>();
        CreateMap<UpdateReservationDto, Reservation>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}