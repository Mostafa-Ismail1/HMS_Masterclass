using System;
using System.Collections.Generic;

namespace HMS.Api.Data.Entities;

public partial class Staff
{
    public int StaffId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public DateOnly HireDate { get; set; }

    public int DepartmentId { get; set; }

    public string JobTitle { get; set; } = null!;

    public decimal Salary { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual Doctor? Doctor { get; set; }

    public virtual Nurse? Nurse { get; set; }
}
