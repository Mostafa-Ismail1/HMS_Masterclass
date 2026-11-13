using HMS.Application.Common.Interfaces;
using HMS.Domain.Entities.Admissions;
using Microsoft.EntityFrameworkCore;

namespace HMS.Infrastructure.Persistence.Repositories;

public class AdmissionRepository(ApplicationDbContext context) : IAdmissionRepository
{
    public async Task<int> AddAdmissionAsync(Admission admission, CancellationToken cancellationToken = default)
    {
        await context.Admissions.AddAsync(admission, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return admission.Id;
    }

    public async Task<Admission?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Admissions.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task UpdateAdmissionAsync(Admission admission, CancellationToken cancellationToken = default)
    {
        context.Admissions.Update(admission);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AdmissionDto>> GetAllAdmissionsAsync(CancellationToken cancellationToken = default)
    {
        return await context.Admissions
            .AsNoTracking()
            .OrderByDescending(a => a.Id)
            .Select(a => new AdmissionDto(a.Id, a.PatientId, a.RoomId, a.AdmittingDoctorId, a.AdmissionDate, a.DischargeDate))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<RoomDto>> GetAllRoomsAsync(CancellationToken cancellationToken = default)
    {
        return await context.Rooms
            .AsNoTracking()
            .OrderBy(r => r.RoomNumber)
            .Select(r => new RoomDto(r.Id, r.RoomNumber, r.RoomType, r.DepartmentId, r.DailyRate))
            .ToListAsync(cancellationToken);
    }
}
