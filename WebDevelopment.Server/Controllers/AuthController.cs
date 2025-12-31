using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebDevelopment.Application.Commands.Authentication.Login;
using WebDevelopment.Application.Commands.Users.Add;
using WebDevelopment.Application.DTOs;
using WebDevelopment.Application.Queries.Users.RefreshToken;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Server.Controllers;


[ApiController]
[Route("api/auth")]
public class AuthController(ISender sender) : ControllerBase
{

    [HttpPost("register")]
    public async Task<IActionResult> AddUser(NewUserDto user)
    {
        var response = await sender.Send(new AddNewUserCommand(user));

        if (response == null)
            return BadRequest(response);

        //if (response is Response<Dictionary<string, string>> errors)
        //{
        //    return Ok(errors);
        //}
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
        //return Ok(Response<Dictionary<string, string>>.Success([]));
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(UserLoginDto userLoginDto)
    {
        var response = await sender.Send(new LoginCommand(userLoginDto));

        if (!response.IsSuccess)
        {
            return BadRequest(new Response
            {
                IsSuccess = false,
                Message = response.Message
            });
        }

        return Ok(response);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenDto dto)
    {
        var response = await sender.Send(new RefreshTokenQuery(dto.RefreshToken));

        if (response == null)
            return Unauthorized();

        return Ok(new Response<string>(response.Data));
    }
}
