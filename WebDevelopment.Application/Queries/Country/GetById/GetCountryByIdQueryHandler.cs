using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebDevelopment.Application.Commands.Country.Add;
using WebDevelopment.Infrastructure;
using WebDevelopment.Shared.DTOs;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Application.Queries.Country.GetById;

public record GetCountryByIdQueryHandler(
    AppDbContext _dbContext,
    IMapper _mapper,
    ILogger<AddCountryCommandHandler> _logger
    ) : IRequestHandler<GetCountryByIdQuery, Response<CountryDto>>
{
    public async Task<Response<CountryDto>> Handle(GetCountryByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Executing {this.GetType().Name} query handler");

            var result = await _dbContext.Countries.FirstOrDefaultAsync(i => i.Id == request.id);

            if (result != null)
            {
                var country = _mapper.Map<CountryDto>(result);
                return new Response<CountryDto>(country);
            }

            _logger.LogError($"Country with id [{request.id}] was not found");
            return new Response<CountryDto>(new()) { IsSuccess = false };

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: [{ex.Message}] - occured in {this.GetType().Name}");
            return new Response<CountryDto>(new()) { IsSuccess = false };
        }
    }
}
