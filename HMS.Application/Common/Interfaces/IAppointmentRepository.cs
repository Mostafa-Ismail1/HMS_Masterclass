using HMS.Domain.Entities.Appointments;

namespace HMS.Application.Common.Interfaces;

public interface IAppointmentRepository
{
    Task<int> AddAsync(Appointment appointment, CancellationToken cancellationToken = default);
    Task<Appointment?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task UpdateAsync(Appointment appointment, CancellationToken cancellationToken = default);
    Task<List<AppointmentDto>> GetAllAsync(CancellationToken cancellationToken = default);
}

public class AppointmentDto
{
    public AppointmentDto() { }

    public AppointmentDto(
        int id,
        int patientId,
        int doctorId,
        DateTime appointmentDate,
        string status,
        string? reason)
    {
        Id = id;
        PatientId = patientId;
        DoctorId = doctorId;
        AppointmentDate = appointmentDate;
        Status = status;
        Reason = reason;
    }

    public int Id { get; init; }
    public int PatientId { get; init; }
    public int DoctorId { get; init; }
    public DateTime AppointmentDate { get; init; }
    public string Status { get; init; } = string.Empty;
    public string? Reason { get; init; }
}
