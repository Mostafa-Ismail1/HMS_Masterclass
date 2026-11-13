using StaffEntity = HMS.Domain.Entities.Staff.Staff;
using HMS.Domain.Entities.Nurses;

namespace HMS.Application.Common.Interfaces;

public record StaffDto(int StaffId, string FullName, string Email, string Phone, DateOnly HireDate, int DepartmentId, string JobTitle, decimal Salary);
public record NurseDto(int NurseId, string ShiftType);

public interface IStaffRepository
{
    Task<int> AddStaffAsync(StaffEntity staff, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StaffDto>> GetAllStaffAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<NurseDto>> GetAllNursesAsync(CancellationToken cancellationToken = default);
}
