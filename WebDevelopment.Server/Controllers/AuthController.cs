using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebDevelopment.Application.Commands.Authentication.Login;
using WebDevelopment.Application.Commands.Authentication.Register;
using WebDevelopment.Application.DTOs;
using WebDevelopment.Application.Queries.Users.RefreshToken;
using WebDevelopment.Shared.DTOs.Authentication;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Server.Controllers;


[ApiController]
[Route("api/auth")]
public class AuthController(ISender sender) : ControllerBase
{

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterUserDto user)
    {
        var response = await sender.Send(new RegisterCommand(user));

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
        var response = await sender.Send(new RefreshTokenQuery(dto));

        if (response == null)
            return Unauthorized();

        return Ok(new Response<string>(response.Data));
    }
}
