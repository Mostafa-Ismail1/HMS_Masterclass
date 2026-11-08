using FluentValidation;
using HMS.Api.Data.Entities;
using HMS.Api.Data.Repositories;
using HMS.Api.DTOs.Departments;
using HMS.Api.Filters;
using HMS.Api.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Caching.Memory;

namespace HMS.Api.Controllers;

[EnableRateLimiting("fixed-window")]
[ServiceFilter(typeof(LogActionActivityFilter))]
[ApiController]
[Route("api/[controller]")]
public class DepartmentsController(IUnitOfWork unitOfWork, IMemoryCache memoryCache) : ControllerBase
{
    private const string DepartmentsCacheKey = "departments-all";

    /// <summary>
    /// Retrieves all hospital departments along with their staff count.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DepartmentResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DepartmentResponseDto>>> GetAll(CancellationToken cancellationToken)
    {
        var dtos = await memoryCache.GetOrCreateAsync(DepartmentsCacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            entry.SlidingExpiration = TimeSpan.FromMinutes(1);

            var departments = await unitOfWork.Departments.GetAllWithStaffAsync(cancellationToken);

            return departments.Select(d => new DepartmentResponseDto
            {
                DepartmentId = d.DepartmentId,
                DepartmentName = d.DepartmentName,
                Location = d.Location,
                TotalStaffCount = d.Staff.Count
            }).ToList();
        });

        return Ok(dtos);
    }

    /// <summary>
    /// Retrieves details of a specific department by its unique ID.
    /// </summary>
    /// <param name="id">The unique identifier of the department.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    [HttpGet("{id:int}", Name = nameof(GetDepartmentById))]
    [ProducesResponseType(typeof(DepartmentResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Creates a new hospital department.
    /// </summary>
    /// <param name="request">The department details to create.</param>
    /// <param name="validator">Validator instance for request validation.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    [HttpPost]
    [ProducesResponseType(typeof(DepartmentResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DepartmentResponseDto>> Create(
        CreateDepartmentRequestDto request,
        IValidator<CreateDepartmentRequestDto> validator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        var department = new Department
        {
            DepartmentName = request.DepartmentName,
            Location = request.Location
        };

        await unitOfWork.Departments.AddAsync(department, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        memoryCache.Remove(DepartmentsCacheKey);

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

        memoryCache.Remove(DepartmentsCacheKey);

        return NoContent();
    }

    /// <summary>
    /// Deletes a hospital department (Admin role required).
    /// </summary>
    /// <param name="id">The department ID to delete.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var department = await unitOfWork.Departments.GetByIdWithStaffAsync(id, cancellationToken);
        if (department is null)
        {
            return NotFound();
        }

        if (department.Staff.Count > 0)
        {
            return Conflict(new { message = "Cannot delete department because it still has assigned staff members." });
        }

        unitOfWork.Departments.Delete(department);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        memoryCache.Remove(DepartmentsCacheKey);

        return NoContent();
    }
}
