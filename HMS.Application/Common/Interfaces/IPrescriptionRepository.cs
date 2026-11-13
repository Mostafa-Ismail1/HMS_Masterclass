using HMS.Domain.Entities.Prescriptions;

namespace HMS.Application.Common.Interfaces;

public record PrescriptionItemDto(int MedicationId, string Dosage, string Duration);
public record PrescriptionDto(int PrescriptionId, int AppointmentId, int DoctorId, int PatientId, DateOnly DatePrescribed);

public interface IPrescriptionRepository
{
    Task<int> AddAsync(Prescription prescription, IEnumerable<PrescriptionDetail> details, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PrescriptionDto>> GetAllAsync(CancellationToken cancellationToken = default);
}
