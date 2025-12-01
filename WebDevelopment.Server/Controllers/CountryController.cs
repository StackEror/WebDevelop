using Microsoft.AspNetCore.Mvc;
using WebDevelopment.Shared.DTO;
using WebDevelopment.Shared.Interfaces;

namespace WebDevelopment.Server.Controllers;

[ApiController]
[Route("api/country")]
public class CountryController(ICountryService _countryService) : ControllerBase
{
    [HttpPost("add")]
    public async Task<IActionResult> AddCountry([FromBody] CountryDto country)
    {
        await _countryService.AddCountry(country);
        return Ok();
    }
}
