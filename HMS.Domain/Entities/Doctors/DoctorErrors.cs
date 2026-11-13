using HMS.Domain.Common;

namespace HMS.Domain.Entities.Doctors;

public static class DoctorErrors
{
    public static readonly Error EmptySpecialization = Error.Validation(
        "Doctor.EmptySpecialization",
        "Doctor specialization cannot be empty.");

    public static readonly Error EmptyLicenseNumber = Error.Validation(
        "Doctor.EmptyLicenseNumber",
        "Doctor license number cannot be empty.");

    public static readonly Error InvalidScheduleTime = Error.Validation(
        "DoctorSchedule.InvalidTime",
        "Schedule end time must be later than start time.");

    public static readonly Error NotFound = Error.NotFound(
        "Doctor.NotFound",
        "Doctor was not found.");
}
