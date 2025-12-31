using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using WebDevelopment.Shared.DTOs;
using WebDevelopment.Shared.Interfaces;

namespace WebDevelopment.Server.Controllers;

[ApiController]
[Route("api/file")]
public class FileController(
    IFileService fileService
    ) : ControllerBase
{
    [HttpPost("add")]
    public async Task<IActionResult> AddFile([FromBody] FileDto fileDto)
    {
        if (fileDto == null)
        {
            return BadRequest("File is null");
        }
        else
        {
            var response = await fileService.Add(fileDto);
            return Ok(response);
        }
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid Id)
    {
        if (Id == Guid.Empty)
        {
            return BadRequest("File is null");
        }
        else
        {
            var response = await fileService.GetById(Id);
            return Ok(response);
        }
    }
}
