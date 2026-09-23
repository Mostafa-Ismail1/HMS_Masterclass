using System;
using System.Collections.Generic;

namespace HMS.Api.Data.Entities;

public partial class DoctorSchedule
{
    public int DoctorId { get; set; }

    public int DepartmentId { get; set; }

    public string DayOfWeek { get; set; } = null!;

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual Doctor Doctor { get; set; } = null!;
}
