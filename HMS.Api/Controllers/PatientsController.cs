using Asp.Versioning;
using HMS.Application.Patients.Commands.RegisterPatient;
using HMS.Application.Patients.Queries.GetPatients;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Api.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class PatientsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetPatientsQuery(page, pageSize), cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new
            {
                code = result.Error.Code,
                description = result.Error.Description
            });
        }

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Register(
        RegisterPatientCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new
            {
                code = result.Error.Code,
                description = result.Error.Description
            });
        }

        return CreatedAtAction(
            nameof(GetAll),
            new { id = result.Value },
            new { patientId = result.Value });
    }
}
