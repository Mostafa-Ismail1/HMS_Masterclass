using HMS.Domain.Entities.Patients;

namespace HMS.Application.Common.Interfaces;

public record PatientDto(
    int PatientId,
    string FullName,
    DateOnly Dob,
    string Phone,
    string Gender,
    string? Email,
    string? Address,
    string? BloodType);

public interface IPatientRepository
{
    Task<int> AddAsync(Patient patient, CancellationToken cancellationToken = default);
    Task<Patient?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task UpdateAsync(Patient patient, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PatientDto>> GetAllAsync(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default);
}
