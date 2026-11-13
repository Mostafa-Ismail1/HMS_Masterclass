using HMS.Application.LabOrders.Commands.CreateLabOrder;
using HMS.Application.LabOrders.Commands.RecordLabResult;
using HMS.Application.LabOrders.Queries.GetLabOrders;
using HMS.Application.LabOrders.Queries.GetLabTests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LabOrdersController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetLabOrdersQuery(), cancellationToken);
        if (result.IsFailure) return BadRequest(new { code = result.Error.Code, description = result.Error.Description });
        return Ok(result.Value);
    }

    [HttpGet("tests")]
    public async Task<IActionResult> GetTests(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetLabTestsQuery(), cancellationToken);
        if (result.IsFailure) return BadRequest(new { code = result.Error.Code, description = result.Error.Description });
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateLabOrderCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        if (result.IsFailure) return BadRequest(new { code = result.Error.Code, description = result.Error.Description });
        return CreatedAtAction(nameof(GetAll), new { id = result.Value }, new { labOrderId = result.Value });
    }

    [HttpPut("{id:int}/result")]
    public async Task<IActionResult> RecordResult(int id, RecordLabResultCommand command, CancellationToken cancellationToken)
    {
        if (id != command.LabOrderId) return BadRequest(new { code = "InvalidId", description = "URL ID does not match body ID." });
        var result = await sender.Send(command, cancellationToken);
        if (result.IsFailure) return BadRequest(new { code = result.Error.Code, description = result.Error.Description });
        return NoContent();
    }
}
