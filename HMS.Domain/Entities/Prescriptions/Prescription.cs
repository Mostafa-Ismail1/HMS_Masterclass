using HMS.Domain.Common;

namespace HMS.Domain.Entities.Prescriptions;

public class Prescription : Entity<int>
{
    private Prescription(int appointmentId, int doctorId, int patientId, DateOnly datePrescribed)
    {
        AppointmentId = appointmentId;
        DoctorId = doctorId;
        PatientId = patientId;
        DatePrescribed = datePrescribed;
    }

    private Prescription()
    {
    }

    public int AppointmentId { get; private set; }
    public int DoctorId { get; private set; }
    public int PatientId { get; private set; }
    public DateOnly DatePrescribed { get; private set; }

    public static Result<Prescription> Create(int appointmentId, int doctorId, int patientId, DateOnly datePrescribed)
    {
        return Result.Success(new Prescription(appointmentId, doctorId, patientId, datePrescribed));
    }
}
