using HMS.Application.Common.Interfaces;
using HMS.Domain.Entities.Prescriptions;
using Microsoft.EntityFrameworkCore;

namespace HMS.Infrastructure.Persistence.Repositories;

public class PrescriptionRepository(ApplicationDbContext context) : IPrescriptionRepository
{
    public async Task<int> AddAsync(Prescription prescription, IEnumerable<PrescriptionDetail> details, CancellationToken cancellationToken = default)
    {
        await context.Prescriptions.AddAsync(prescription, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        foreach (var detail in details)
        {
            var detailEntity = PrescriptionDetail.Create(prescription.Id, detail.MedicationId, detail.Dosage, detail.Duration).Value;
            await context.PrescriptionDetails.AddAsync(detailEntity, cancellationToken);
        }
        await context.SaveChangesAsync(cancellationToken);

        return prescription.Id;
    }

    public async Task<IReadOnlyList<PrescriptionDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Prescriptions
            .AsNoTracking()
            .OrderByDescending(p => p.Id)
            .Select(p => new PrescriptionDto(p.Id, p.AppointmentId, p.DoctorId, p.PatientId, p.DatePrescribed))
            .ToListAsync(cancellationToken);
    }
}
