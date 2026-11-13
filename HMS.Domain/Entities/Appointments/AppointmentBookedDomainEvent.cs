using HMS.Domain.Common;

namespace HMS.Domain.Entities.Appointments;

public record AppointmentBookedDomainEvent(int PatientId, int DoctorId, DateTime AppointmentDate) : IDomainEvent;
