using HMS.Application.Admissions.Commands.AdmitPatient;
using HMS.Application.Admissions.Commands.DischargePatient;
using HMS.Application.Admissions.Queries.GetAdmissions;
using HMS.Application.Admissions.Queries.GetRooms;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdmissionsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAdmissionsQuery(), cancellationToken);
        if (result.IsFailure) return BadRequest(new { code = result.Error.Code, description = result.Error.Description });
        return Ok(result.Value);
    }

    [HttpGet("rooms")]
    public async Task<IActionResult> GetRooms(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetRoomsQuery(), cancellationToken);
        if (result.IsFailure) return BadRequest(new { code = result.Error.Code, description = result.Error.Description });
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Admit(AdmitPatientCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        if (result.IsFailure) return BadRequest(new { code = result.Error.Code, description = result.Error.Description });
        return CreatedAtAction(nameof(GetAll), new { id = result.Value }, new { admissionId = result.Value });
    }

    [HttpPut("{id:int}/discharge")]
    public async Task<IActionResult> Discharge(int id, DischargePatientCommand command, CancellationToken cancellationToken)
    {
        if (id != command.AdmissionId) return BadRequest(new { code = "InvalidId", description = "URL ID does not match body ID." });
        var result = await sender.Send(command, cancellationToken);
        if (result.IsFailure) return BadRequest(new { code = result.Error.Code, description = result.Error.Description });
        return NoContent();
    }
}
