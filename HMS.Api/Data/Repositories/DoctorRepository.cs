using HMS.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HMS.Api.Data.Repositories;

public class DoctorRepository(HmsDbContext context) : GenericRepository<Doctor>(context), IDoctorRepository
{
    public async Task<IReadOnlyList<Doctor>> GetAllWithDetailsAsync(
        string? specialization,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .AsNoTracking()
            .Include(d => d.DoctorNavigation)
                .ThenInclude(s => s.Department)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(specialization))
        {
            query = query.Where(d => d.Specialization.Contains(specialization));
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<Doctor?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(d => d.DoctorNavigation)
                .ThenInclude(s => s.Department)
            .Include(d => d.Appointments)
            .FirstOrDefaultAsync(d => d.DoctorId == id, cancellationToken);
    }
}
