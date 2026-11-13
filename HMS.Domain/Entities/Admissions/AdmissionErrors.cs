using HMS.Domain.Common;

namespace HMS.Domain.Entities.Admissions;

public static class AdmissionErrors
{
    public static readonly Error InvalidDates = Error.Validation(
        "Admission.InvalidDates",
        "Discharge date cannot be earlier than admission date.");

    public static readonly Error AlreadyDischarged = Error.Conflict(
        "Admission.AlreadyDischarged",
        "The patient is already discharged from this admission.");

    public static readonly Error NotFound = Error.NotFound(
        "Admission.NotFound",
        "The admission record was not found.");
}
