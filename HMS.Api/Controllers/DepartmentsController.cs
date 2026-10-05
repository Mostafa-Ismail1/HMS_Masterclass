using HMS.Api.Data.Entities;
using HMS.Api.Data.Repositories;
using HMS.Api.DTOs.Departments;
using HMS.Api.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Api.Controllers;

[ServiceFilter(typeof(LogActionActivityFilter))]
[ApiController]
[Route("api/[controller]")]
public class DepartmentsController(IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DepartmentResponseDto>>> GetAll(CancellationToken cancellationToken)
    {
        var departments = await unitOfWork.Departments.GetAllWithStaffAsync(cancellationToken);

        var dtos = departments.Select(d => new DepartmentResponseDto
        {
            DepartmentId = d.DepartmentId,
            DepartmentName = d.DepartmentName,
            Location = d.Location,
            TotalStaffCount = d.Staff.Count
        });

        return Ok(dtos);
    }

    [HttpGet("{id:int}", Name = nameof(GetDepartmentById))]
    public async Task<ActionResult<DepartmentResponseDto>> GetDepartmentById(int id, CancellationToken cancellationToken)
    {
        var department = await unitOfWork.Departments.GetByIdWithStaffAsync(id, cancellationToken);
        if (department is null)
        {
            return NotFound();
        }

        var responseDto = new DepartmentResponseDto
        {
            DepartmentId = department.DepartmentId,
            DepartmentName = department.DepartmentName,
            Location = department.Location,
            TotalStaffCount = department.Staff.Count
        };

        return Ok(responseDto);
    }

    [HttpHead("{id:int}")]
    public async Task<IActionResult> CheckDepartmentExists(int id, CancellationToken cancellationToken)
    {
        var exists = await unitOfWork.Departments.AnyAsync(d => d.DepartmentId == id, cancellationToken);
        return exists ? Ok() : NotFound();
    }

    [HttpOptions]
    public IActionResult GetDepartmentOptions()
    {
        Response.Headers.Append("Allow", "GET, POST, PUT, DELETE, HEAD, OPTIONS");
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult<DepartmentResponseDto>> Create(CreateDepartmentRequestDto request, CancellationToken cancellationToken)
    {
        var department = new Department
        {
            DepartmentName = request.DepartmentName,
            Location = request.Location
        };

        await unitOfWork.Departments.AddAsync(department, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

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
    public async Task<IActionResult> Update(int id, UpdateDepartmentRequestDto request, CancellationToken cancellationToken)
    {
        var department = await unitOfWork.Departments.GetByIdAsync(id, cancellationToken);
        if (department is null)
        {
            return NotFound();
        }

        department.DepartmentName = request.DepartmentName;
        department.Location = request.Location;

        unitOfWork.Departments.Update(department);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var department = await unitOfWork.Departments.GetByIdWithStaffAsync(id, cancellationToken);
        if (department is null)
        {
            return NotFound(new { message = $"Department with ID {id} was not found." });
        }

        if (department.Staff.Count > 0)
        {
            return Conflict(new { message = "Cannot delete department because it still has assigned staff members." });
        }

        unitOfWork.Departments.Delete(department);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}
