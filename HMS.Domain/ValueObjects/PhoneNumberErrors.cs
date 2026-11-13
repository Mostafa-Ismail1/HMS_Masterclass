using HMS.Domain.Common;

namespace HMS.Domain.ValueObjects;

public static class PhoneNumberErrors
{
    public static readonly Error Empty =
        Error.Validation("PhoneNumber.Empty", "Phone number is required.");

    public static readonly Error InvalidFormat =
        Error.Validation("PhoneNumber.InvalidFormat", "Phone number must be a valid 11-digit Egyptian mobile number.");
}
