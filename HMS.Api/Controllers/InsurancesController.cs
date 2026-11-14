using Asp.Versioning;
using HMS.Application.Insurances.Commands.CreateInsuranceProvider;
using HMS.Application.Insurances.Queries.GetInsuranceProviders;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Api.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class InsurancesController(ISender sender) : ControllerBase
{
    [HttpGet("providers")]
    public async Task<IActionResult> GetProviders(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetInsuranceProvidersQuery(), cancellationToken);
        if (result.IsFailure) return BadRequest(new { code = result.Error.Code, description = result.Error.Description });
        return Ok(result.Value);
    }

    [HttpPost("providers")]
    public async Task<IActionResult> CreateProvider(CreateInsuranceProviderCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        if (result.IsFailure) return BadRequest(new { code = result.Error.Code, description = result.Error.Description });
        return CreatedAtAction(nameof(GetProviders), new { id = result.Value }, new { providerId = result.Value });
    }
}
