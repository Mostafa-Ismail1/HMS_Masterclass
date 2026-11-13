using HMS.Domain.Entities.Doctors;

namespace HMS.Application.Common.Interfaces;

public record DoctorDto(int DoctorId, string Specialization, string LicenseNumber);

public interface IDoctorRepository
{
    Task<int> AddAsync(Doctor doctor, CancellationToken cancellationToken = default);
    Task<Doctor?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DoctorDto>> GetAllAsync(CancellationToken cancellationToken = default);
}
