using Asp.Versioning;
using HMS.Application.Staff.Commands.CreateStaff;
using HMS.Application.Staff.Queries.GetNurses;
using HMS.Application.Staff.Queries.GetStaff;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Api.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class StaffController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetStaffQuery(), cancellationToken);
        if (result.IsFailure) return BadRequest(new { code = result.Error.Code, description = result.Error.Description });
        return Ok(result.Value);
    }

    [HttpGet("nurses")]
    public async Task<IActionResult> GetNurses(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetNursesQuery(), cancellationToken);
        if (result.IsFailure) return BadRequest(new { code = result.Error.Code, description = result.Error.Description });
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateStaffCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        if (result.IsFailure) return BadRequest(new { code = result.Error.Code, description = result.Error.Description });
        return CreatedAtAction(nameof(GetAll), new { id = result.Value }, new { staffId = result.Value });
    }
}
