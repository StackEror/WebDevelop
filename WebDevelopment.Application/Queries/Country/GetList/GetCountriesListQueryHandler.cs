using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebDevelopment.Application.Commands.Country.Add;
using WebDevelopment.Infrastructure;
using WebDevelopment.Shared.DTO;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Application.Queries.Country.GetList
{
    public class GetCountriesListQueryHandler(
        AppDbContext _dbContext,
        IMapper _mapper,
        ILogger<AddCountryCommandHandler> _logger
        ) : IRequestHandler<GetCountriesListQuery, Response<List<CountryDto>>>
    {
        public async Task<Response<List<CountryDto>>> Handle(GetCountriesListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Executing {this.GetType().Name} query handler");
                var result = await _dbContext.Countries.ToListAsync();

                if (result != null)
                {
                    var list = _mapper.Map<List<CountryDto>>(result);
                    return new Response<List<CountryDto>>(list);
                }

                _logger.LogError($"Result is null");
                return new Response<List<CountryDto>>([]) { IsSuccess = false };

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: [{ex.Message}] - occured in {this.GetType().Name}");
                return new Response<List<CountryDto>>([]) { IsSuccess = false };
            }
        }
    }
}
