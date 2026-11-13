using HMS.Domain.Common;

namespace HMS.Domain.Entities.Departments;

public static class DepartmentErrors
{
    public static readonly Error EmptyName = Error.Validation(
        "Department.EmptyName",
        "Department name cannot be empty.");

    public static readonly Error NotFound = Error.NotFound(
        "Department.NotFound",
        "The department was not found.");
}
