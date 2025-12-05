using AutoMapper;
using WebDevelopment.Domain.Entities;
using WebDevelopment.Shared.DTO;

namespace WebDevelopment.Application.Mapper;

public class CountryMappingProfile : Profile
{
    public CountryMappingProfile()
    {
        CreateMap<CountryDto, Country>()
            .ReverseMap();
    }
}
