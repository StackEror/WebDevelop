using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebDevelopment.Application.Commands.Country.Add;
using WebDevelopment.Application.Commands.Country.Delete;
using WebDevelopment.Application.Commands.Country.Update;
using WebDevelopment.Application.Queries.Country.GetById;
using WebDevelopment.Application.Queries.Country.GetList;
using WebDevelopment.Shared.DTOs;
using WebDevelopment.Shared.Interfaces;
using WebDevelopment.Shared.Responses;

namespace WebDevelopment.Server.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/country")]
public class CountryController(
    ICountryService _countryService,
    ISender _sender
    ) : ControllerBase
{
    [HttpPost("add")]
    public async Task<IActionResult> AddCountry([FromBody] CountryDto country)
    {
        /* With mediatR 
         */
        var command = new AddCountryCommand(country);
        var result = await _sender.Send(command);

        /* With services
        var result = await _countryService.Add(country);
         */


        if (result.IsSuccess)
            return Ok(result.Data);
        else
            return BadRequest(result.Data);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCountry([FromBody] CountryDto country)
    {
        /* With mediatR 
         */
        var command = new UpdateCountryCommand(country);
        var result = await _sender.Send(command);

        /* With services
        var result = await _countryService.Update(country);
         */

        if (result.IsSuccess)
            return Ok();
        else
            return BadRequest();
    }
    [HttpGet]
    public async Task<IActionResult> GetListCountries()
    {
        /* With mediatR 
        var command = new GetCountriesListQuery();
        var result = await _sender.Send(command);
         */

        /* With services
         */
        var result = await _countryService.GetList();

        if (result.IsSuccess)
            return Ok(result.Data);
        else
            return BadRequest(result.Data);
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetCountryById(string Id)
    {
        if (!Guid.TryParse(Id, out var guid))
        {
            return BadRequest("Invalid Id format");
        }
        /* With mediatR 
         */
        var command = new GetCountryByIdQuery(guid);
        var result = await _sender.Send(command);

        /* With services
        var result = await _countryService.GetById(guid);
         */

        if (result.IsSuccess)
            return Ok(result);
        else
            return BadRequest(result);
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteCountryById(string Id)
    {
        if (!Guid.TryParse(Id, out var guid))
        {
            return BadRequest("Invalid Id format");
        }
        /* With mediatR 
         */
        var command = new DeleteCountryCommand(guid);
        var result = await _sender.Send(command);

        /* With services
        var result = await _countryService.Delete(guid);
         */

        if (result.IsSuccess)
            return Ok();
        else
            return BadRequest(result);
    }
}
