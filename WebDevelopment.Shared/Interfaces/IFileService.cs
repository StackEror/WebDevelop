using WebDevelopment.Shared.DTOs;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Shared.Interfaces;

public interface IFileService
{
    Task<Response<Guid>> Add(FileDto fileDto);
    Task<Response<FileDto>> GetById(Guid Id);
}
