using HMS.Application.Common.Interfaces;
using HMS.Domain.Entities.Patients;
using Microsoft.EntityFrameworkCore;

namespace HMS.Infrastructure.Persistence.Repositories;

public class PatientRepository(ApplicationDbContext context) : IPatientRepository
{
    public async Task<int> AddAsync(Patient patient, CancellationToken cancellationToken = default)
    {
        await context.Patients.AddAsync(patient, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return patient.Id;
    }

    public async Task<Patient?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Patients
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(Patient patient, CancellationToken cancellationToken = default)
    {
        context.Patients.Update(patient);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PatientDto>> GetAllAsync(
        int page = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        return await context.Patients
            .AsNoTracking()
            .OrderBy(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new PatientDto(
                p.Id,
                $"{p.FirstName} {p.LastName}",
                p.Dob,
                p.Phone,
                p.Gender,
                p.Email,
                p.Address,
                p.BloodType))
            .ToListAsync(cancellationToken);
    }
}
