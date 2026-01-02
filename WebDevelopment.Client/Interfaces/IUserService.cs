using WebDevelopment.Client.Models.User;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Client.Interfaces;

public interface IUserService
{
    //Task<Dictionary<string, string>> AddNewUser(AddUserModel userModel);
    Task<Response> AddNewUser(AddUserModel userModel);
    Task<Dictionary<string, string>> GetRolesAsDictionary();
}
