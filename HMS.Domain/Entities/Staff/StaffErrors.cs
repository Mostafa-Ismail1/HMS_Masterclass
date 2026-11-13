using HMS.Domain.Common;

namespace HMS.Domain.Entities.Staff;

public static class StaffErrors
{
    public static readonly Error EmptyName = Error.Validation(
        "Staff.EmptyName",
        "Staff member name cannot be empty.");

    public static readonly Error InvalidSalary = Error.Validation(
        "Staff.InvalidSalary",
        "Staff salary must be greater than zero.");

    public static readonly Error NotFound = Error.NotFound(
        "Staff.NotFound",
        "Staff member was not found.");
}
