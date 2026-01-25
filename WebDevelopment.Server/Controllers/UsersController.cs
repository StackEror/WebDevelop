using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebDevelopment.Application.Commands.Authentication.ChangePassword;
using WebDevelopment.Application.Commands.Authentication.UpdateAccesToken;
using WebDevelopment.Application.Commands.Users.Add;
using WebDevelopment.Application.DTOs;
using WebDevelopment.Application.Queries.Users.GetAllRoles;
using WebDevelopment.Application.Security;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Server.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController(
    ISender sender,
    UserContext userContext
    ) : ControllerBase
{
    [HttpPost("add")]
    public async Task<IActionResult> AddUser(NewUserDto user)
    {
        var response = await sender.Send(new AddNewUserCommand(user));

        if (response == null)
            return BadRequest(response);

        if (response is Response<Guid> result)
        {
            if (result.IsSuccess && result.Data != Guid.Empty)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(response);
            }
        }
        return BadRequest(response);
    }

    [HttpGet("all-roles")]
    public async Task<IActionResult> GetRolesAsDictionary()
    {
        var response = await sender.Send(new GetAllRolesQuery());
        if (response == null) return BadRequest(response);
        return Ok(response);
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
    {
        var userId = userContext.GetId();

        var response = await sender.Send(new ChangePasswordCommand(model, userId.Value));

        if (response == null) return BadRequest(response);

        if (response.IsSuccess == false)
        {
            return BadRequest(response);
        }
         
        return Ok(response);
    }

    [HttpGet("update-access-token")]
    public async Task<IActionResult> UpdateAccessToken()
    {
        var response = await sender.Send(new UpdateAccesTokenCommand());

        if (!response.IsSuccess)
        {
            return BadRequest(new Response
            {
                IsSuccess = false,
                Message = "An error occurred while update acces token"
            });
        }

        return Ok(response);
    }
}
