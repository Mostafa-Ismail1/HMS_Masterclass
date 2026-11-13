using HMS.Domain.Entities.Appointments;

namespace HMS.Application.Common.Interfaces;

public interface IAppointmentRepository
{
    Task<int> AddAsync(Appointment appointment, CancellationToken cancellationToken = default);
    Task<Appointment?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task UpdateAsync(Appointment appointment, CancellationToken cancellationToken = default);
    Task<List<AppointmentDto>> GetAllAsync(CancellationToken cancellationToken = default);
}

public record AppointmentDto(
    int AppointmentId,
    int PatientId,
    int DoctorId,
    DateTime AppointmentDate,
    string Status,
    string? Reason);
