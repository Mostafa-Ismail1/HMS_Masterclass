using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using HMS.Api;

namespace HMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HospitalController(IOptions<HospitalSettingsOptions> options) : ControllerBase
{
    [HttpGet("info")]
    public IActionResult GetHospitalInfo()
    {
        return Ok(new
        {
            Hospital = options.Value.HospitalName,
            Capacity = options.Value.MaxBedCapacity,
            Fee = options.Value.DefaultConsultationFee
        });
    }
}
