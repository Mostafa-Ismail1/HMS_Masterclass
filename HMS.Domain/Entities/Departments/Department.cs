using HMS.Domain.Common;

namespace HMS.Domain.Entities.Departments;

public class Department : Entity<int>
{
    private Department(string departmentName, string location)
    {
        DepartmentName = departmentName;
        Location = location;
    }

    private Department()
    {
    }

    public string DepartmentName { get; private set; } = null!;
    public string Location { get; private set; } = null!;

    public static Result<Department> Create(string departmentName, string location)
    {
        if (string.IsNullOrWhiteSpace(departmentName))
        {
            return Result.Failure<Department>(DepartmentErrors.EmptyName);
        }

        return Result.Success(new Department(departmentName.Trim(), location.Trim()));
    }

    public Result UpdateDetails(string departmentName, string location)
    {
        if (string.IsNullOrWhiteSpace(departmentName))
        {
            return Result.Failure(DepartmentErrors.EmptyName);
        }

        DepartmentName = departmentName.Trim();
        Location = location.Trim();
        return Result.Success();
    }
}
