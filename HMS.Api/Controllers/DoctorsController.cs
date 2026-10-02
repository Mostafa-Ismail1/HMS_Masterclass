using HMS.Api.Data.Entities;
using HMS.Api.Data.Repositories;
using HMS.Api.DTOs.Doctors;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController(IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DoctorResponseDto>>> GetAll(
        [FromQuery] string? specialization,
        CancellationToken cancellationToken)
    {
        var doctors = await unitOfWork.Doctors.GetAllWithDetailsAsync(specialization, cancellationToken);

        var dtos = doctors.Select(d => new DoctorResponseDto
        {
            DoctorId = d.DoctorId,
            FullName = $"Dr. {d.DoctorNavigation.FirstName} {d.DoctorNavigation.LastName}",
            Specialization = d.Specialization,
            LicenseNumber = d.LicenseNumber,
            DepartmentName = d.DoctorNavigation.Department.DepartmentName,
            Email = d.DoctorNavigation.Email,
            Phone = d.DoctorNavigation.Phone
        });

        return Ok(dtos);
    }

    [HttpGet("{id:int}", Name = nameof(GetDoctorById))]
    public async Task<ActionResult<DoctorResponseDto>> GetDoctorById(int id, CancellationToken cancellationToken)
    {
        var doctor = await unitOfWork.Doctors.GetByIdWithDetailsAsync(id, cancellationToken);
        if (doctor is null)
        {
            return NotFound(new { message = $"Doctor with ID {id} was not found." });
        }

        var responseDto = new DoctorResponseDto
        {
            DoctorId = doctor.DoctorId,
            FullName = $"Dr. {doctor.DoctorNavigation.FirstName} {doctor.DoctorNavigation.LastName}",
            Specialization = doctor.Specialization,
            LicenseNumber = doctor.LicenseNumber,
            DepartmentName = doctor.DoctorNavigation.Department.DepartmentName,
            Email = doctor.DoctorNavigation.Email,
            Phone = doctor.DoctorNavigation.Phone
        };

        return Ok(responseDto);
    }

    [HttpHead("{id:int}")]
    public async Task<IActionResult> CheckDoctorExists(int id, CancellationToken cancellationToken)
    {
        var exists = await unitOfWork.Doctors.AnyAsync(d => d.DoctorId == id, cancellationToken);
        return exists ? Ok() : NotFound();
    }

    [HttpOptions]
    public IActionResult GetDoctorOptions()
    {
        Response.Headers.Append("Allow", "GET, POST, PUT, DELETE, HEAD, OPTIONS");
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult<DoctorResponseDto>> Create(CreateDoctorRequestDto request, CancellationToken cancellationToken)
    {
        var department = await unitOfWork.Departments.GetByIdAsync(request.DepartmentId, cancellationToken);
        if (department is null)
        {
            return BadRequest(new { message = $"Department with ID {request.DepartmentId} does not exist." });
        }

        var staff = new Staff
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Phone = request.Phone,
            DepartmentId = request.DepartmentId,
            JobTitle = "Doctor",
            HireDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Salary = request.Salary
        };

        await unitOfWork.Staff.AddAsync(staff, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var doctor = new Doctor
        {
            DoctorId = staff.StaffId,
            Specialization = request.Specialization,
            LicenseNumber = request.LicenseNumber
        };

        await unitOfWork.Doctors.AddAsync(doctor, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var responseDto = new DoctorResponseDto
        {
            DoctorId = doctor.DoctorId,
            FullName = $"Dr. {staff.FirstName} {staff.LastName}",
            Specialization = doctor.Specialization,
            LicenseNumber = doctor.LicenseNumber,
            DepartmentName = department.DepartmentName,
            Email = staff.Email,
            Phone = staff.Phone
        };

        return CreatedAtAction(nameof(GetDoctorById), new { id = doctor.DoctorId }, responseDto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateDoctorRequestDto request, CancellationToken cancellationToken)
    {
        var doctor = await unitOfWork.Doctors.GetByIdWithDetailsAsync(id, cancellationToken);
        if (doctor is null)
        {
            return NotFound(new { message = $"Doctor with ID {id} was not found." });
        }

        doctor.Specialization = request.Specialization;
        doctor.DoctorNavigation.Phone = request.Phone;
        doctor.DoctorNavigation.Email = request.Email;

        unitOfWork.Doctors.Update(doctor);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var doctor = await unitOfWork.Doctors.GetByIdWithDetailsAsync(id, cancellationToken);
        if (doctor is null)
        {
            return NotFound(new { message = $"Doctor with ID {id} was not found." });
        }

        if (doctor.Appointments.Count > 0)
        {
            return Conflict(new { message = "Cannot delete doctor with assigned appointments." });
        }

        unitOfWork.Doctors.Delete(doctor);
        unitOfWork.Staff.Delete(doctor.DoctorNavigation);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}
