using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebDevelopment.Application.Commands.Country.Add;
using WebDevelopment.Infrastructure;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Application.Commands.Country.Update
{
    public class UpdateCountryCommandHandler(
        AppDbContext _dbContext,
        IMapper _mapper,
        ILogger<AddCountryCommandHandler> _logger
        ) : IRequestHandler<UpdateCountryCommand, Response>
    {
        public async Task<Response> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Executing {this.GetType().Name} command handler");

                var dbEntity = await _dbContext.Countries.FirstOrDefaultAsync(i => i.Id == request.country.Id);
                if (dbEntity != null)
                {
                    _mapper.Map(request.country, dbEntity);
                    dbEntity.UpdatedAt = DateTime.Now;
                    await _dbContext.SaveChangesAsync();
                    return new Response();
                }

                _logger.LogError($"Entity with id [{request.country.Id}] was not found");
                return new Response() { IsSuccess = false };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: [{ex.Message}] - occured in {this.GetType().Name}");
                return new Response() { IsSuccess = false };
            }
        }
    }
}
