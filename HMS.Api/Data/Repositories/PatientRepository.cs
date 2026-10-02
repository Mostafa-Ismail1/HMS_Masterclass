using HMS.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HMS.Api.Data.Repositories;

public class PatientRepository(HmsDbContext context) : GenericRepository<Patient>(context), IPatientRepository
{
    public async Task<IReadOnlyList<Patient>> GetPatientsAsync(
        string? bloodType,
        string? gender,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .AsNoTracking()
            .Include(p => p.Appointments)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(bloodType))
        {
            query = query.Where(p => p.BloodType == bloodType);
        }

        if (!string.IsNullOrWhiteSpace(gender))
        {
            query = query.Where(p => p.Gender == gender);
        }

        return await query
            .OrderBy(p => p.PatientId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Patient?> GetByIdWithAppointmentsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Appointments)
            .FirstOrDefaultAsync(p => p.PatientId == id, cancellationToken);
    }
}
