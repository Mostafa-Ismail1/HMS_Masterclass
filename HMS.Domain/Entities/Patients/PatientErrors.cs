using HMS.Domain.Common;

namespace HMS.Domain.Entities.Patients;

public static class PatientErrors
{
    public static readonly Error DobInFuture = Error.Validation(
        "Patient.DobInFuture",
        "Date of birth cannot be in the future.");

    public static readonly Error EmptyName = Error.Validation(
        "Patient.EmptyName",
        "First name and last name are required and cannot be empty.");

    public static readonly Error InvalidGender = Error.Validation(
        "Patient.InvalidGender",
        "Gender must be either 'M' (Male) or 'F' (Female).");

    public static readonly Error NotFound = Error.NotFound(
        "Patient.NotFound",
        "The patient with the specified ID was not found.");
}
