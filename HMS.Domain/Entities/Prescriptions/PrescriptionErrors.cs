using HMS.Domain.Common;

namespace HMS.Domain.Entities.Prescriptions;

public static class PrescriptionErrors
{
    public static readonly Error EmptyDosage = Error.Validation(
        "Prescription.EmptyDosage",
        "Dosage instructions cannot be empty.");

    public static readonly Error NotFound = Error.NotFound(
        "Prescription.NotFound",
        "Prescription was not found.");
}
