using Asp.Versioning;
using HMS.Application.Medications.Commands.CreateMedication;
using HMS.Application.Medications.Queries.GetMedications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Api.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class MedicationsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetMedicationsQuery(), cancellationToken);
        if (result.IsFailure) return BadRequest(new { code = result.Error.Code, description = result.Error.Description });
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateMedicationCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        if (result.IsFailure) return BadRequest(new { code = result.Error.Code, description = result.Error.Description });
        return CreatedAtAction(nameof(GetAll), new { id = result.Value }, new { medicationId = result.Value });
    }
}
