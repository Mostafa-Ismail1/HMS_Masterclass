using HMS.Api.Data;
using HMS.Api.Data.Entities;
using HMS.Api.DTOs.Patients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController(HmsDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PatientResponseDto>>> GetAll(
        [FromQuery] string? bloodType,
        [FromQuery] string? gender,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = context.Patients.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(bloodType))
        {
            query = query.Where(p => p.BloodType == bloodType);
        }

        if (!string.IsNullOrWhiteSpace(gender))
        {
            query = query.Where(p => p.Gender == gender);
        }

        var patients = await query
            .OrderBy(p => p.PatientId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new PatientResponseDto
            {
                PatientId = p.PatientId,
                FullName = $"{p.FirstName} {p.LastName}",
                Dob = p.Dob,
                Phone = p.Phone,
                Gender = p.Gender,
                Email = p.Email,
                Address = p.Address,
                BloodType = p.BloodType,
                ActiveAppointmentsCount = p.Appointments.Count
            })
            .ToListAsync();

        return Ok(patients);
    }

    [HttpGet("{id:int}", Name = nameof(GetPatientById))]
    public async Task<ActionResult<PatientResponseDto>> GetPatientById(int id)
    {
        var patient = await context.Patients
            .AsNoTracking()
            .Where(p => p.PatientId == id)
            .Select(p => new PatientResponseDto
            {
                PatientId = p.PatientId,
                FullName = $"{p.FirstName} {p.LastName}",
                Dob = p.Dob,
                Phone = p.Phone,
                Gender = p.Gender,
                Email = p.Email,
                Address = p.Address,
                BloodType = p.BloodType,
                ActiveAppointmentsCount = p.Appointments.Count
            })
            .FirstOrDefaultAsync();

        if (patient is null)
        {
            return NotFound(new { message = $"Patient with ID {id} was not found." });
        }

        return Ok(patient);
    }

    [HttpHead("{id:int}")]
    public async Task<IActionResult> CheckPatientExists(int id)
    {
        var exists = await context.Patients.AnyAsync(p => p.PatientId == id);
        return exists ? Ok() : NotFound();
    }

    [HttpOptions]
    public IActionResult GetPatientOptions()
    {
        Response.Headers.Append("Allow", "GET, POST, PUT, DELETE, HEAD, OPTIONS");
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult<PatientResponseDto>> Create(CreatePatientRequestDto request)
    {
        var patient = new Patient
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Dob = request.Dob,
            Phone = request.Phone,
            Gender = request.Gender,
            Email = request.Email,
            Address = request.Address,
            BloodType = request.BloodType
        };

        context.Patients.Add(patient);
        await context.SaveChangesAsync();

        var responseDto = new PatientResponseDto
        {
            PatientId = patient.PatientId,
            FullName = $"{patient.FirstName} {patient.LastName}",
            Dob = patient.Dob,
            Phone = patient.Phone,
            Gender = patient.Gender,
            Email = patient.Email,
            Address = patient.Address,
            BloodType = patient.BloodType,
            ActiveAppointmentsCount = 0
        };

        return CreatedAtAction(nameof(GetPatientById), new { id = patient.PatientId }, responseDto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdatePatientRequestDto request)
    {
        var patient = await context.Patients.FindAsync(id);
        if (patient is null)
        {
            return NotFound(new { message = $"Patient with ID {id} was not found." });
        }

        patient.Phone = request.Phone;
        patient.Email = request.Email;
        patient.Address = request.Address;

        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var patient = await context.Patients
            .Include(p => p.Appointments)
            .FirstOrDefaultAsync(p => p.PatientId == id);

        if (patient is null)
        {
            return NotFound(new { message = $"Patient with ID {id} was not found." });
        }

        if (patient.Appointments.Any())
        {
            return Conflict(new { message = "Cannot delete patient with active appointments on file." });
        }

        context.Patients.Remove(patient);
        await context.SaveChangesAsync();

        return NoContent();
    }
}
