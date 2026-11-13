using HMS.Domain.Entities.Admissions;

namespace HMS.Application.Common.Interfaces;

public record RoomDto(int RoomId, string RoomNumber, string RoomType, int DepartmentId, decimal DailyRate);
public record AdmissionDto(int AdmissionId, int PatientId, int RoomId, int AdmittingDoctorId, DateTime AdmissionDate, DateTime? DischargeDate);

public interface IAdmissionRepository
{
    Task<int> AddAdmissionAsync(Admission admission, CancellationToken cancellationToken = default);
    Task<Admission?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task UpdateAdmissionAsync(Admission admission, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AdmissionDto>> GetAllAdmissionsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RoomDto>> GetAllRoomsAsync(CancellationToken cancellationToken = default);
}
