using HMS.Domain.Common;

namespace HMS.Domain.Entities.Insurances;

public static class InsuranceErrors
{
    public static readonly Error EmptyProviderName = Error.Validation(
        "Insurance.EmptyProviderName",
        "Insurance provider name cannot be empty.");

    public static readonly Error EmptyPolicyNumber = Error.Validation(
        "Insurance.EmptyPolicyNumber",
        "Insurance policy number cannot be empty.");

    public static readonly Error ExpiredPolicy = Error.Validation(
        "Insurance.ExpiredPolicy",
        "Insurance policy is expired.");

    public static readonly Error NotFound = Error.NotFound(
        "Insurance.NotFound",
        "Insurance record was not found.");
}
