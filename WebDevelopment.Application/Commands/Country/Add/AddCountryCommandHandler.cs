using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using WebDevelopment.Infrastructure;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Application.Commands.Country.Add
{
    public class AddCountryCommandHandler(
        AppDbContext _dbContext,
        IMapper _mapper,
        ILogger<AddCountryCommandHandler> _logger
        ) : IRequestHandler<AddCountryCommand, Response<Guid>>
    {
        public async Task<Response<Guid>> Handle(AddCountryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Executing {GetType().Name} command handler");

                var country = _mapper.Map<Domain.Entities.Country>(request.country);
                country.CreatedAt = DateTime.Now;
                country.ModifiedAt = DateTime.Now;

                await _dbContext.AddAsync(country);
                await _dbContext.SaveChangesAsync();    

                _logger.LogInformation($"Sucesfuly added country with Id [{country.Id}] {GetType().Name}");
                return new Response<Guid>(country.Id) { IsSuccess = true};
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: [{ex.Message}] - occured in {GetType().Name}");
                return new Response<Guid>(Guid.Empty) { IsSuccess = false };
            }

        }
    }
}
