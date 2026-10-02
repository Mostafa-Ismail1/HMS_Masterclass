using HMS.Api.Data.Entities;
using HMS.Api.Data.Repositories;
using HMS.Api.DTOs.Patients;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController(IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PatientResponseDto>>> GetAll(
        [FromQuery] string? bloodType,
        [FromQuery] string? gender,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var patients = await unitOfWork.Patients.GetPatientsAsync(bloodType, gender, page, pageSize, cancellationToken);

        var dtos = patients.Select(p => new PatientResponseDto
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
        });

        return Ok(dtos);
    }

    [HttpGet("{id:int}", Name = nameof(GetPatientById))]
    public async Task<ActionResult<PatientResponseDto>> GetPatientById(int id, CancellationToken cancellationToken)
    {
        var patient = await unitOfWork.Patients.GetByIdWithAppointmentsAsync(id, cancellationToken);
        if (patient is null)
        {
            return NotFound(new { message = $"Patient with ID {id} was not found." });
        }

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
            ActiveAppointmentsCount = patient.Appointments.Count
        };

        return Ok(responseDto);
    }

    [HttpHead("{id:int}")]
    public async Task<IActionResult> CheckPatientExists(int id, CancellationToken cancellationToken)
    {
        var exists = await unitOfWork.Patients.AnyAsync(p => p.PatientId == id, cancellationToken);
        return exists ? Ok() : NotFound();
    }

    [HttpOptions]
    public IActionResult GetPatientOptions()
    {
        Response.Headers.Append("Allow", "GET, POST, PUT, DELETE, HEAD, OPTIONS");
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult<PatientResponseDto>> Create(CreatePatientRequestDto request, CancellationToken cancellationToken)
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

        await unitOfWork.Patients.AddAsync(patient, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

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
    public async Task<IActionResult> Update(int id, UpdatePatientRequestDto request, CancellationToken cancellationToken)
    {
        var patient = await unitOfWork.Patients.GetByIdAsync(id, cancellationToken);
        if (patient is null)
        {
            return NotFound(new { message = $"Patient with ID {id} was not found." });
        }

        patient.Phone = request.Phone;
        patient.Email = request.Email;
        patient.Address = request.Address;

        unitOfWork.Patients.Update(patient);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var patient = await unitOfWork.Patients.GetByIdWithAppointmentsAsync(id, cancellationToken);
        if (patient is null)
        {
            return NotFound(new { message = $"Patient with ID {id} was not found." });
        }

        if (patient.Appointments.Count > 0)
        {
            return Conflict(new { message = "Cannot delete patient with active appointments on file." });
        }

        unitOfWork.Patients.Delete(patient);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}
