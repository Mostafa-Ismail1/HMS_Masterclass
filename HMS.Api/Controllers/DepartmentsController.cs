using HMS.Api.Data;
using HMS.Api.Data.Entities;
using HMS.Api.DTOs.Departments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController(HmsDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DepartmentResponseDto>>> GetAll()
    {
        var departments = await context.Departments
            .AsNoTracking()
            .Select(d => new DepartmentResponseDto
            {
                DepartmentId = d.DepartmentId,
                DepartmentName = d.DepartmentName,
                Location = d.Location,
                TotalStaffCount = d.Staff.Count
            })
            .ToListAsync();

        return Ok(departments);
    }

    [HttpGet("{id:int}", Name = nameof(GetDepartmentById))]
    public async Task<ActionResult<DepartmentResponseDto>> GetDepartmentById(int id)
    {
        var department = await context.Departments
            .AsNoTracking()
            .Where(d => d.DepartmentId == id)
            .Select(d => new DepartmentResponseDto
            {
                DepartmentId = d.DepartmentId,
                DepartmentName = d.DepartmentName,
                Location = d.Location,
                TotalStaffCount = d.Staff.Count
            })
            .FirstOrDefaultAsync();

        if (department is null)
        {
            return NotFound(new { message = $"Department with ID {id} was not found." });
        }

        return Ok(department);
    }

    [HttpHead("{id:int}")]
    public async Task<IActionResult> CheckDepartmentExists(int id)
    {
        var exists = await context.Departments.AnyAsync(d => d.DepartmentId == id);
        if (!exists)
        {
            return NotFound();
        }

        return Ok();
    }

    [HttpOptions]
    public IActionResult GetDepartmentOptions()
    {
        Response.Headers.Append("Allow", "GET, POST, PUT, DELETE, HEAD, OPTIONS");
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult<DepartmentResponseDto>> Create(CreateDepartmentRequestDto request)
    {
        var department = new Department
        {
            DepartmentName = request.DepartmentName,
            Location = request.Location
        };

        context.Departments.Add(department);
        await context.SaveChangesAsync();

        var responseDto = new DepartmentResponseDto
        {
            DepartmentId = department.DepartmentId,
            DepartmentName = department.DepartmentName,
            Location = department.Location,
            TotalStaffCount = 0
        };

        return CreatedAtAction(nameof(GetDepartmentById), new { id = department.DepartmentId }, responseDto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateDepartmentRequestDto request)
    {
        var department = await context.Departments.FindAsync(id);
        if (department is null)
        {
            return NotFound(new { message = $"Department with ID {id} was not found." });
        }

        department.DepartmentName = request.DepartmentName;
        department.Location = request.Location;

        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var department = await context.Departments
            .Include(d => d.Staff)
            .FirstOrDefaultAsync(d => d.DepartmentId == id);

        if (department is null)
        {
            return NotFound(new { message = $"Department with ID {id} was not found." });
        }

        if (department.Staff.Any())
        {
            return Conflict(new { message = "Cannot delete department because it still has assigned staff members." });
        }

        context.Departments.Remove(department);
        await context.SaveChangesAsync();

        return NoContent();
    }
}
