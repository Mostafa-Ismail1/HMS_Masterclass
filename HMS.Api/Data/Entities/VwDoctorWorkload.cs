using System;
using System.Collections.Generic;

namespace HMS.Api.Data.Entities;

public partial class VwDoctorWorkload
{
    public int DoctorId { get; set; }

    public string DoctorName { get; set; } = null!;

    public string Specialization { get; set; } = null!;

    public string DepartmentName { get; set; } = null!;

    public int? TotalAppointments { get; set; }
}
