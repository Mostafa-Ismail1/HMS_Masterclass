using HMS.Domain.Entities.Departments;

namespace HMS.Application.Common.Interfaces;

public record DepartmentDto(int DepartmentId, string DepartmentName, string Location);

public interface IDepartmentRepository
{
    Task<int> AddAsync(Department department, CancellationToken cancellationToken = default);
    Task<Department?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DepartmentDto>> GetAllAsync(CancellationToken cancellationToken = default);
}
