using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebDevelopment.Infrastructure;
using WebDevelopment.Shared.DTOs;
using WebDevelopment.Shared.Interfaces;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Application.Services.Country;

public class CountryService(
    AppDbContext _dbContext,
    IMapper _mapper,
    ILogger<CountryService> _logger
    ) : ICountryService
{
    public async Task<Response<Guid>> Add(CountryDto country)
    {
        try
        {
            _logger.LogInformation($"Executing {this.GetType().Name}.{nameof(Add)} service");

            var countryEntity = _mapper.Map<Domain.Entities.Country>(country);

            countryEntity.CreatedAt = DateTime.Now;
            countryEntity.ModifiedAt = DateTime.Now;
            await _dbContext.AddAsync(countryEntity);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Sucesfuly added country with Id [{countryEntity.Id}]");
            return new Response<Guid>(countryEntity.Id) { IsSuccess = true };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: [{ex.Message}] - occured in {this.GetType().Name}.{nameof(Add)}");
            return new Response<Guid>(Guid.Empty) { IsSuccess = false };
        }
    }
    public async Task<Response> Update(CountryDto country)
    {
        try
        {
            _logger.LogInformation($"Executing {this.GetType().Name}.{nameof(Update)} service");

            var dbEntity = await _dbContext.Countries.FirstOrDefaultAsync(i => i.Id == country.Id);
            if (dbEntity != null)
            {
                _mapper.Map(country, dbEntity);
                dbEntity.ModifiedAt = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                return new Response();
            }

            _logger.LogError($"Entity with id [{country.Id}] was not found");
            return new Response() { IsSuccess = false };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: [{ex.Message}] - occured in {this.GetType().Name}.{nameof(Update)}");
            return new Response() { IsSuccess = false };
        }
    }
    public async Task<Response<List<CountryDto>>> GetList()
    {
        try
        {
            _logger.LogInformation($"Executing {this.GetType().Name}.{nameof(GetList)} service");
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
            _logger.LogError($"Error: [{ex.Message}] - occured in {this.GetType().Name}.{nameof(GetList)}");
            return new Response<List<CountryDto>>([]) { IsSuccess = false };
        }
    }
    public async Task<Response<CountryDto>> GetById(Guid id)
    {
        try
        {
            _logger.LogInformation($"Executing {this.GetType().Name}.{nameof(GetById)} service");

            var result = await _dbContext.Countries.FirstOrDefaultAsync(i => i.Id == id);

            if (result != null)
            {
                var country = _mapper.Map<CountryDto>(result);
                return new Response<CountryDto>(country);
            }

            _logger.LogError($"Country with id [{id}] was not found");
            return new Response<CountryDto>(new()) { IsSuccess = false };

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: [{ex.Message}] - occured in {this.GetType().Name}.{nameof(GetById)}");
            return new Response<CountryDto>(new()) { IsSuccess = false };
        }
    }
    public async Task<Response> Delete(Guid id)
    {
        try
        {
            _logger.LogInformation($"Executing {this.GetType().Name}.{nameof(Delete)} service");

            var IsSucces = await _dbContext.Countries
                .Where(i => i.Id == id)
                .ExecuteDeleteAsync();

            if (IsSucces == 0)
            {
                _logger.LogError($"Delete for id [{id}] failed");
                return new Response() { IsSuccess = false};
            }
            else
            {
                _logger.LogInformation($"Country with id [{id}] was succesfully deleted");
                return new Response();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: [{ex.Message}] - occured in {this.GetType().Name}.{nameof(Delete)}");
            return new Response<List<CountryDto>>([]) { IsSuccess = false };
        }
    }
}
