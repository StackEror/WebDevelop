using Microsoft.AspNetCore.Mvc;
using WebDevelopment.Domain.Entities;

namespace WebDevelopment.Server.Controllers;

[ApiController]
[Route("api/country")]
public class CountryController : ControllerBase
{
    [HttpPost("add")]
    public async Task<IActionResult> AddCountry(Country country)
    {
        return Ok();
    }
}
