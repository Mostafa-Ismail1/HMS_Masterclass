using HMS.Api.Data;
using HMS.Api.Data.Entities;
using HMS.Api.DTOs.Doctors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController(HmsDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DoctorResponseDto>>> GetAll([FromQuery] string? specialization)
    {
        var query = context.Doctors
            .AsNoTracking()
            .Include(d => d.DoctorNavigation)
            .ThenInclude(s => s.Department)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(specialization))
        {
            query = query.Where(d => d.Specialization.Contains(specialization));
        }

        var doctors = await query
            .Select(d => new DoctorResponseDto
            {
                DoctorId = d.DoctorId,
                FullName = $"Dr. {d.DoctorNavigation.FirstName} {d.DoctorNavigation.LastName}",
                Specialization = d.Specialization,
                LicenseNumber = d.LicenseNumber,
                DepartmentName = d.DoctorNavigation.Department.DepartmentName,
                Email = d.DoctorNavigation.Email,
                Phone = d.DoctorNavigation.Phone
            })
            .ToListAsync();

        return Ok(doctors);
    }

    [HttpGet("{id:int}", Name = nameof(GetDoctorById))]
    public async Task<ActionResult<DoctorResponseDto>> GetDoctorById(int id)
    {
        var doctor = await context.Doctors
            .AsNoTracking()
            .Where(d => d.DoctorId == id)
            .Select(d => new DoctorResponseDto
            {
                DoctorId = d.DoctorId,
                FullName = $"Dr. {d.DoctorNavigation.FirstName} {d.DoctorNavigation.LastName}",
                Specialization = d.Specialization,
                LicenseNumber = d.LicenseNumber,
                DepartmentName = d.DoctorNavigation.Department.DepartmentName,
                Email = d.DoctorNavigation.Email,
                Phone = d.DoctorNavigation.Phone
            })
            .FirstOrDefaultAsync();

        if (doctor is null)
        {
            return NotFound(new { message = $"Doctor with ID {id} was not found." });
        }

        return Ok(doctor);
    }

    [HttpHead("{id:int}")]
    public async Task<IActionResult> CheckDoctorExists(int id)
    {
        var exists = await context.Doctors.AnyAsync(d => d.DoctorId == id);
        return exists ? Ok() : NotFound();
    }

    [HttpOptions]
    public IActionResult GetDoctorOptions()
    {
        Response.Headers.Append("Allow", "GET, POST, PUT, DELETE, HEAD, OPTIONS");
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult<DoctorResponseDto>> Create(CreateDoctorRequestDto request)
    {
        var department = await context.Departments.FindAsync(request.DepartmentId);
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

        context.Staff.Add(staff);
        await context.SaveChangesAsync();

        var doctor = new Doctor
        {
            DoctorId = staff.StaffId,
            Specialization = request.Specialization,
            LicenseNumber = request.LicenseNumber
        };

        context.Doctors.Add(doctor);
        await context.SaveChangesAsync();

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
    public async Task<IActionResult> Update(int id, UpdateDoctorRequestDto request)
    {
        var doctor = await context.Doctors
            .Include(d => d.DoctorNavigation)
            .FirstOrDefaultAsync(d => d.DoctorId == id);

        if (doctor is null)
        {
            return NotFound(new { message = $"Doctor with ID {id} was not found." });
        }

        doctor.Specialization = request.Specialization;
        doctor.DoctorNavigation.Phone = request.Phone;
        doctor.DoctorNavigation.Email = request.Email;

        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var doctor = await context.Doctors
            .Include(d => d.Appointments)
            .Include(d => d.DoctorNavigation)
            .FirstOrDefaultAsync(d => d.DoctorId == id);

        if (doctor is null)
        {
            return NotFound(new { message = $"Doctor with ID {id} was not found." });
        }

        if (doctor.Appointments.Any())
        {
            return Conflict(new { message = "Cannot delete doctor with assigned appointments." });
        }

        context.Doctors.Remove(doctor);
        context.Staff.Remove(doctor.DoctorNavigation);
        await context.SaveChangesAsync();

        return NoContent();
    }
}
