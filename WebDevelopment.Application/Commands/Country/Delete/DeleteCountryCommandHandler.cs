using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebDevelopment.Application.Commands.Country.Add;
using WebDevelopment.Infrastructure;
using WebDevelopment.Shared.DTOs.Country;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Application.Commands.Country.Delete;

public record DeleteCountryCommandHandler(
    AppDbContext _dbContext,
    IMapper _mapper,
    ILogger<AddCountryCommandHandler> _logger
    ) : IRequestHandler<DeleteCountryCommand, Response>
{
    public async Task<Response> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Executing {this.GetType().Name} command handler");

            var IsSucces = await _dbContext.Countries
                .Where(i => i.Id == request.id)
                .ExecuteDeleteAsync();

            if (IsSucces == 0)
            {
                _logger.LogError($"Delete for id [{request.id}] failed");
                return new Response() { IsSuccess = false };
            }
            else
            {
                _logger.LogInformation($"Country with id [{request.id}] was succesfully deleted");
                return new Response();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: [{ex.Message}] - occured in {this.GetType().Name}");
            return new Response<List<CountryDto>>([]) { IsSuccess = false };
        }
    }
}
