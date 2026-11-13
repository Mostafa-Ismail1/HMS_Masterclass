using HMS.Domain.Common;

namespace HMS.Domain.Entities.Appointments;

public class Appointment : Entity<int>
{
    private Appointment(
        int patientId,
        int doctorId,
        DateTime appointmentDate,
        string? reason)
    {
        PatientId = patientId;
        DoctorId = doctorId;
        AppointmentDate = appointmentDate;
        Reason = reason;
        Status = AppointmentStatus.Scheduled;
        CreatedAtUtc = DateTime.UtcNow;
    }

    private Appointment()
    {
    }

    public int PatientId { get; private set; }
    public int DoctorId { get; private set; }
    public DateTime AppointmentDate { get; private set; }
    public AppointmentStatus Status { get; private set; }
    public string? Reason { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    public static Result<Appointment> Create(
        int patientId,
        int doctorId,
        DateTime appointmentDate,
        string? reason)
    {
        if (appointmentDate <= DateTime.UtcNow)
        {
            return Result.Failure<Appointment>(AppointmentErrors.PastDate);
        }

        var appointment = new Appointment(patientId, doctorId, appointmentDate, reason);
        appointment.RaiseDomainEvent(new AppointmentBookedDomainEvent(patientId, doctorId, appointmentDate));
        return Result.Success(appointment);
    }

    public Result Cancel()
    {
        if (Status == AppointmentStatus.Completed)
        {
            return Result.Failure(AppointmentErrors.CannotCancelCompleted);
        }

        if (Status == AppointmentStatus.Cancelled)
        {
            return Result.Failure(AppointmentErrors.AlreadyCancelled);
        }

        Status = AppointmentStatus.Cancelled;
        return Result.Success();
    }

    public Result Reschedule(DateTime newAppointmentDate)
    {
        if (Status == AppointmentStatus.Completed)
        {
            return Result.Failure(AppointmentErrors.CannotCancelCompleted);
        }

        if (newAppointmentDate <= DateTime.UtcNow)
        {
            return Result.Failure(AppointmentErrors.PastDate);
        }

        AppointmentDate = newAppointmentDate;
        Status = AppointmentStatus.Scheduled;
        return Result.Success();
    }

    public Result Complete()
    {
        if (Status == AppointmentStatus.Cancelled)
        {
            return Result.Failure(AppointmentErrors.AlreadyCancelled);
        }

        Status = AppointmentStatus.Completed;
        return Result.Success();
    }
}
