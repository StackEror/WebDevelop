using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebDevelopment.Application.Helpers;
using WebDevelopment.Application.Interfaces.Caching;
using WebDevelopment.Infrastructure;
using WebDevelopment.Shared.DTOs.Country;
using WebDevelopment.Shared.Interfaces;
using WebDevelopment.Shared.Page;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Application.Services.Country;

public class CountryService(
    AppDbContext _dbContext,
    IMapper _mapper,
    IDataRequestHandlerService<Domain.Entities.Country, CountryDto> dataRequest,
    ICacheService cacheService,
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
    public async Task<Response<PaginatedCollection<CountryDto>>> GetList(PageFilter<CountryFilterDto> filter)
    {
        try
        {
            _logger.LogInformation($"Executing {this.GetType().Name}.{nameof(GetList)} service");

            var query = _dbContext.Countries.AsQueryable();

            if (!string.IsNullOrEmpty(filter.SearchKeyword))
            {
                query = QueryProcessorExtension.WhereAnyStringPropertyContains(query, filter.SearchKeyword);
            }
            //if (!string.IsNullOrEmpty(filter.SortColumn)) { }
            //if(filter.SortDirection != SortDirection.None) { }

            int totalCounts = query.Count();
            int count = 0;
            if (filter.PageSize > 0)
            {
                count = filter.PageNumber * filter.PageSize;
            }

            query = query.Skip(count).Take(filter.PageSize);

            if (query != null)
            {
                var list = _mapper.Map<List<CountryDto>>(query);
                var collection = new PaginatedCollection<CountryDto>
                {
                    TotalRecords = query.Count(),
                    TotalPages = totalCounts / filter.PageSize,
                    PageNumber = filter.PageNumber,
                    Pages = list,
                    PageSize = filter.PageSize
                };

                return new Response<PaginatedCollection<CountryDto>>(collection);
            }
            /*
                if (!string.IsNullOrEmpty(searchKeyword))
                {
                    //query = query.Where(r => EF.Functions.Like(r.Name, $"%{searchKeyword}%"));
                    query = QueryProcessorExtension.WhereAnyStringPropertyContains(query, searchKeyword);
                }
             */

            //  V3
            /*
                var instance = Activator.CreateInstance<CountryDto>();
                var query = _dbContext.Countries.Take(30).AsQueryable();

                var dataRequestFilter = new Shared.RequestFeatures.DataRequest()
                {
                    Filter = null,
                    Keyword = string.Empty,
                    Page = 1,
                    PageSize = 10,
                };

                var result = await Task.FromResult(dataRequest.HandleRequest(query, dataRequestFilter));
                if (result != null && result.Data.Count() > 0)
                {
                    var inter = result.Data.ToList();
                    return new Response<List<CountryDto>>(inter);
                }
             */

            _logger.LogError($"Result is null");
            return new Response<PaginatedCollection<CountryDto>>(default) { IsSuccess = false };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: [{ex.Message}] - occured in {this.GetType().Name}.{nameof(GetList)}");
            return new Response<PaginatedCollection<CountryDto>>(default) { IsSuccess = false };
        }
    }
    public async Task<Response<CountryDto>> GetById(Guid id)
    {
        try
        {
            var cacheKey = $"country:id-{id}";

            var cachedEntity = await cacheService.GetAsync<CountryDto>(cacheKey);
            if (cachedEntity is not null)
            {
                return new Response<CountryDto>(cachedEntity);
            }
            _logger.LogInformation($"Executing {this.GetType().Name}.{nameof(GetById)} service");

            var result = await _dbContext.Countries.FirstOrDefaultAsync(i => i.Id == id);

            if (result != null)
            {
                var country = _mapper.Map<CountryDto>(result);

                await cacheService.SetAsync(cacheKey, country);

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
                return new Response() { IsSuccess = false };
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
