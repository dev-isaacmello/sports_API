using AutoMapper;
using csharp_api.Application.Dtos;
using csharp_api.Domain.Entities;

namespace csharp_api.Domain.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Users, UserDto>().ReverseMap();
    }
    
}