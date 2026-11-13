using Asp.Versioning;
using HMS.Application.Appointments.Commands.BookAppointment;
using HMS.Application.Appointments.Commands.CancelAppointment;
using HMS.Application.Appointments.Commands.RescheduleAppointment;
using HMS.Application.Appointments.Queries.GetAppointments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Api.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Route("api/[controller]")]
public class AppointmentsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAppointments(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAppointmentsQuery(), cancellationToken);

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
    public async Task<IActionResult> BookAppointment(
        BookAppointmentCommand command,
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
            nameof(BookAppointment),
            new { id = result.Value },
            new { appointmentId = result.Value });
    }

    [HttpPut("{id:int}/cancel")]
    public async Task<IActionResult> CancelAppointment(int id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CancelAppointmentCommand(id), cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new
            {
                code = result.Error.Code,
                description = result.Error.Description
            });
        }

        return NoContent();
    }

    [HttpPut("{id:int}/reschedule")]
    public async Task<IActionResult> RescheduleAppointment(
        int id,
        RescheduleAppointmentRequest request,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new RescheduleAppointmentCommand(id, request.NewAppointmentDate),
            cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new
            {
                code = result.Error.Code,
                description = result.Error.Description
            });
        }

        return NoContent();
    }
}

public record RescheduleAppointmentRequest(DateTime NewAppointmentDate);
