using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using HMS.Api;

namespace HMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HospitalController : ControllerBase
{
    private readonly HospitalSettingsOptions _options;

    public HospitalController(IOptions<HospitalSettingsOptions> options)
    {
        _options = options.Value;
    }

    [HttpGet("info")]
    public IActionResult GetHospitalInfo()
    {
        return Ok(new
        {
            Hospital = _options.HospitalName,
            Capacity = _options.MaxBedCapacity,
            Fee = _options.DefaultConsultationFee
        });
    }
}