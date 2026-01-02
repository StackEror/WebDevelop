using System.Text;
using System.Text.Json;
using WebDevelopment.Client.Interfaces;
using WebDevelopment.Client.Models.User;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Client.Services;

public class UserService(
    HttpClient httpClient
    ) : IUserService
{
    public async Task<Response> AddNewUser(AddUserModel userModel)
    {
        var jsonContent = GetJson(userModel);
        var response = await httpClient.PostAsync("api/users/add", jsonContent);

        var result = await response.Content.ReadFromJsonAsync<Response<Guid>>();
        if (!response.IsSuccessStatusCode)
        {
            return new Response() { IsSuccess = false, Message = result.Message };
        }

        if (result == null)
            return new Response() { IsSuccess = false, Message = result.Message };

        //Dictionary<string, string>() { { "Error", "Failed" } };

        return new Response() { IsSuccess = true, Message = "Succes" };
    }

    public async Task<Dictionary<string, string>> GetRolesAsDictionary()
    {
        var roles = await httpClient.GetFromJsonAsync<Dictionary<string, string>>("api/users/all-roles");

        if (roles != null && roles.Any())
            return roles;

        return [];
    }
    private static StringContent GetJson<T>(T model) =>
        new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
}
