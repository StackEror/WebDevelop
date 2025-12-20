using System.Net;
using System.Net.Http;
using WebDevelopment.Shared.DTO;
using WebDevelopment.Shared.Interfaces;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Client.Services;

public class FileService(
    HttpClient httpClient
    ) : IFileService
{
    public async Task<Response<Guid>> Add(FileDto fileDto)
    {
        var result = await httpClient.PostAsJsonAsync("api/file/add", fileDto);
        Response<Guid> id = new Response<Guid>(Guid.Empty);
        if (result.IsSuccessStatusCode)
        {
            if (result.StatusCode == HttpStatusCode.OK)
            {
                id = await result.Content.ReadFromJsonAsync<Response<Guid>>();
            }
        }

        return id;
    }
    public async Task<Response<FileDto>> GetById(Guid Id)
    {
        var response = await httpClient.GetFromJsonAsync<Response<FileDto>>($"api/file/{Id}");

        if (response == null || response.Data == null)
            return new Response<FileDto>(new()) { IsSuccess = false };
        else
            return new Response<FileDto>(response.Data);
    }
}
