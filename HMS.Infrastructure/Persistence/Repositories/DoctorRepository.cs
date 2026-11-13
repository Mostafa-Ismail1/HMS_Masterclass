using HMS.Application.Common.Interfaces;
using HMS.Domain.Entities.Doctors;
using Microsoft.EntityFrameworkCore;

namespace HMS.Infrastructure.Persistence.Repositories;

public class DoctorRepository(ApplicationDbContext context) : IDoctorRepository
{
    public async Task<int> AddAsync(Doctor doctor, CancellationToken cancellationToken = default)
    {
        await context.Doctors.AddAsync(doctor, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return doctor.Id;
    }

    public async Task<Doctor?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Doctors.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<DoctorDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Doctors
            .AsNoTracking()
            .OrderBy(d => d.Id)
            .Select(d => new DoctorDto(d.Id, d.Specialization, d.LicenseNumber))
            .ToListAsync(cancellationToken);
    }
}
