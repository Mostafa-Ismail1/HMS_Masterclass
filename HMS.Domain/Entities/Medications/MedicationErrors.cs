using HMS.Domain.Common;

namespace HMS.Domain.Entities.Medications;

public static class MedicationErrors
{
    public static readonly Error EmptyName = Error.Validation(
        "Medication.EmptyName",
        "Medication name cannot be empty.");

    public static readonly Error InvalidPrice = Error.Validation(
        "Medication.InvalidPrice",
        "Medication unit price must be greater than zero.");

    public static readonly Error NotFound = Error.NotFound(
        "Medication.NotFound",
        "Medication was not found.");
}
