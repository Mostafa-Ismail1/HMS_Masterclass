using HMS.Api.Data.Entities;

namespace HMS.Api.Data.Repositories;

public interface IDepartmentRepository : IGenericRepository<Department>
{
    Task<IReadOnlyList<Department>> GetAllWithStaffAsync(CancellationToken cancellationToken = default);

    Task<Department?> GetByIdWithStaffAsync(int id, CancellationToken cancellationToken = default);
}
