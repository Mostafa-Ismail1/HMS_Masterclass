using System;
using System.Collections.Generic;

namespace HMS.Api.Data.Entities;

public partial class LabOrder
{
    public int LabOrderId { get; set; }

    public int PatientId { get; set; }

    public int DoctorId { get; set; }

    public int LabTestId { get; set; }

    public DateOnly OrderDate { get; set; }

    public DateOnly? ResultDate { get; set; }

    public string? ResultValue { get; set; }

    public string Status { get; set; } = null!;

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual LabTest LabTest { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;
}
