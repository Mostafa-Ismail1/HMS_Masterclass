using HMS.Domain.Common;

namespace HMS.Domain.Entities.Staff;

public class Staff : Entity<int>
{
    private Staff(
        string firstName,
        string lastName,
        string email,
        string phone,
        DateOnly hireDate,
        int departmentId,
        string jobTitle,
        decimal salary)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        HireDate = hireDate;
        DepartmentId = departmentId;
        JobTitle = jobTitle;
        Salary = salary;
    }

    private Staff()
    {
    }

    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Phone { get; private set; } = null!;
    public DateOnly HireDate { get; private set; }
    public int DepartmentId { get; private set; }
    public string JobTitle { get; private set; } = null!;
    public decimal Salary { get; private set; }

    public static Result<Staff> Create(
        string firstName,
        string lastName,
        string email,
        string phone,
        DateOnly hireDate,
        int departmentId,
        string jobTitle,
        decimal salary)
    {
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        {
            return Result.Failure<Staff>(StaffErrors.EmptyName);
        }

        if (salary <= 0)
        {
            return Result.Failure<Staff>(StaffErrors.InvalidSalary);
        }

        return Result.Success(new Staff(
            firstName.Trim(),
            lastName.Trim(),
            email.Trim(),
            phone.Trim(),
            hireDate,
            departmentId,
            jobTitle.Trim(),
            salary));
    }
}
