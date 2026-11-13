using HMS.Application.Invoices.Commands.CreateInvoice;
using HMS.Application.Invoices.Commands.PayInvoice;
using HMS.Application.Invoices.Queries.GetInvoices;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvoicesController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetInvoicesQuery(), cancellationToken);
        if (result.IsFailure) return BadRequest(new { code = result.Error.Code, description = result.Error.Description });
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateInvoiceCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        if (result.IsFailure) return BadRequest(new { code = result.Error.Code, description = result.Error.Description });
        return CreatedAtAction(nameof(GetAll), new { id = result.Value }, new { invoiceId = result.Value });
    }

    [HttpPut("{id:int}/pay")]
    public async Task<IActionResult> Pay(int id, [FromQuery] DateOnly issueDate, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new PayInvoiceCommand(id, issueDate), cancellationToken);
        if (result.IsFailure) return BadRequest(new { code = result.Error.Code, description = result.Error.Description });
        return NoContent();
    }
}
