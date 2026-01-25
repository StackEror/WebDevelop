using WebDevelopment.Shared.DTOs.File;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Shared.Interfaces;

public interface IFileService
{
    Task<Response<Guid>> Add(FileDto fileDto);
    Task<Response<FileDto>> GetById(Guid Id);
}
