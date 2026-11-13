using HMS.Application.Common.Interfaces;
using StaffEntity = HMS.Domain.Entities.Staff.Staff;
using Microsoft.EntityFrameworkCore;

namespace HMS.Infrastructure.Persistence.Repositories;

public class StaffRepository(ApplicationDbContext context) : IStaffRepository
{
    public async Task<int> AddStaffAsync(StaffEntity staff, CancellationToken cancellationToken = default)
    {
        await context.Staff.AddAsync(staff, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return staff.Id;
    }

    public async Task<IReadOnlyList<StaffDto>> GetAllStaffAsync(CancellationToken cancellationToken = default)
    {
        return await context.Staff
            .AsNoTracking()
            .OrderBy(s => s.Id)
            .Select(s => new StaffDto(s.Id, $"{s.FirstName} {s.LastName}", s.Email, s.Phone, s.HireDate, s.DepartmentId, s.JobTitle, s.Salary))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<NurseDto>> GetAllNursesAsync(CancellationToken cancellationToken = default)
    {
        return await context.Nurses
            .AsNoTracking()
            .OrderBy(n => n.Id)
            .Select(n => new NurseDto(n.Id, n.ShiftType))
            .ToListAsync(cancellationToken);
    }
}
