using WebDevelopment.Client.Models.Authentication;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Client.Interfaces;

public interface IAuthService
{
    Task<Response> Register(RegisterUserModel userModel);
    Task<Response<LoginResponseModel>> Login(LoginModel loginModel);
    Task LogOut();
}
