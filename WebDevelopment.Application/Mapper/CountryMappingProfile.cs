using AutoMapper;
using WebDevelopment.Domain.Entities;
using WebDevelopment.Shared.DTOs.Country;

namespace WebDevelopment.Application.Mapper;

public class CountryMappingProfile : Profile
{
    public CountryMappingProfile()
    {
        CreateMap<CountryDto, Country>()
            .ReverseMap();
    }
}
