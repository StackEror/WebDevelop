using System.Text;
using System.Text.Json;
using WebDevelopment.Client.Interfaces;
using WebDevelopment.Client.Models.Authentication;
using WebDevelopment.Client.Models.User;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Client.Services;

public class UserService(
    HttpClient httpClient,
    ApiClient apiClient,
    CustomAuthenticationStateProvider customAuthenticationStateProvider
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
    public async Task<Response> ChangePassword(ChangePasswordModel model)
    {
        var response = await apiClient.PostAsync<Response, ChangePasswordModel>("api/users/change-password", model);
        if (response.IsSuccess)
        {
            return response;
        }
        return response;
    }

    public async Task<Response<LoginResponseModel>> UpdateAccesToken()
    {
        var response = await apiClient.GetFromJsonAsync<Response<LoginResponseModel>>("api/users/update-access-token");

        if (response == null)
        {
            return Response<LoginResponseModel>.Failure(response?.Message ?? "An error occurred while processing the request");
        }

        if (response.IsSuccess == true && response.Data is not null)
        {
            await customAuthenticationStateProvider.MarkUserAsAuthenticated(response.Data.TokenResponse);
            return Response<LoginResponseModel>.Success(response.Data);
        }

        return response;
    }

    private static StringContent GetJson<T>(T model) =>
        new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

}
