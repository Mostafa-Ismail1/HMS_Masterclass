using HMS.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HMS.Api.Data.Repositories;

public class DepartmentRepository(HmsDbContext context) : GenericRepository<Department>(context), IDepartmentRepository
{
    public async Task<IReadOnlyList<Department>> GetAllWithStaffAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(d => d.Staff)
            .ToListAsync(cancellationToken);
    }

    public async Task<Department?> GetByIdWithStaffAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(d => d.Staff)
            .FirstOrDefaultAsync(d => d.DepartmentId == id, cancellationToken);
    }
}
