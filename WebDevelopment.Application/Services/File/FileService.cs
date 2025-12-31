using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebDevelopment.Infrastructure;
using WebDevelopment.Shared.DTOs;
using WebDevelopment.Shared.Interfaces;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Application.Services.File;

public class FileService(
    AppDbContext _dbContext,
    IMapper _mapper,
    ILogger<FileService> _logger
    ) : IFileService
{
    public async Task<Response<Guid>> Add(FileDto fileDto)
    {
        try
        {
            if (fileDto == null)
                return new Response<Guid>(Guid.Empty) { IsSuccess = false };

            var entityFile = new Domain.Entities.File
            {
                Name = fileDto.Name,
                Content = fileDto.Content,
                ContentType = fileDto.ContentType,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };
            await _dbContext.Files.AddAsync(entityFile);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Sucesfuly added file with Id [{entityFile.Id}]");
            return new Response<Guid>(entityFile.Id);

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: [{ex.Message}] - occured in {this.GetType().Name}");
            return new Response<Guid>(Guid.Empty) { IsSuccess = false };
        }
    }

    public async Task<Response<FileDto>> GetById(Guid Id)
    {
        try
        {
            var result = await _dbContext.Files.FirstOrDefaultAsync(i => i.Id == Id);

            if (result != null)
            {
                var file = new FileDto
                {
                    Id = result.Id,
                    Name = result.Name,
                    ContentType = result.ContentType,
                    Content = result.Content,
                };
                return new Response<FileDto>(file);
            }

            _logger.LogError($"Country with id [{Id}] was not found");
            return new Response<FileDto>(new()) { IsSuccess = false };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: [{ex.Message}] - occured in {this.GetType().Name}");
            return new Response<FileDto>(new()) { IsSuccess = false };
        }
    }
}
