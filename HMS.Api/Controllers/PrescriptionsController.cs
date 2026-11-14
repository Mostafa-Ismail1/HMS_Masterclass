using Asp.Versioning;
using HMS.Application.Prescriptions.Commands.IssuePrescription;
using HMS.Application.Prescriptions.Queries.GetPrescriptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Api.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class PrescriptionsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetPrescriptionsQuery(), cancellationToken);
        if (result.IsFailure) return BadRequest(new { code = result.Error.Code, description = result.Error.Description });
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Issue(IssuePrescriptionCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        if (result.IsFailure) return BadRequest(new { code = result.Error.Code, description = result.Error.Description });
        return CreatedAtAction(nameof(GetAll), new { id = result.Value }, new { prescriptionId = result.Value });
    }
}
