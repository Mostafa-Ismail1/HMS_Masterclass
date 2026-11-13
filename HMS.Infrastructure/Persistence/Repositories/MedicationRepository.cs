using HMS.Application.Common.Interfaces;
using HMS.Domain.Entities.Medications;
using Microsoft.EntityFrameworkCore;

namespace HMS.Infrastructure.Persistence.Repositories;

public class MedicationRepository(ApplicationDbContext context) : IMedicationRepository
{
    public async Task<int> AddAsync(Medication medication, CancellationToken cancellationToken = default)
    {
        await context.Medications.AddAsync(medication, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return medication.Id;
    }

    public async Task<IReadOnlyList<MedicationDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Medications
            .AsNoTracking()
            .OrderBy(m => m.Name)
            .Select(m => new MedicationDto(m.Id, m.Name, m.Manufacturer, m.UnitPrice))
            .ToListAsync(cancellationToken);
    }
}
