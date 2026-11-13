using HMS.Domain.Common;

namespace HMS.Domain.Entities.Appointments;

public static class AppointmentErrors
{
    public static readonly Error PastDate =
        Error.Validation("Appointment.PastDate", "Appointment date cannot be in the past.");

    public static readonly Error CannotCancelCompleted =
        Error.Conflict("Appointment.CannotCancelCompleted", "Cannot cancel an appointment that is already completed.");

    public static readonly Error AlreadyCancelled =
        Error.Conflict("Appointment.AlreadyCancelled", "Appointment is already cancelled.");

    public static readonly Error NotFound =
        Error.NotFound("Appointment.NotFound", "The appointment with the specified identifier was not found.");
}
