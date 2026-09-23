using System;
using System.Collections.Generic;

namespace HMS.Api.Data.Entities;

public partial class VwPatientFullHistory
{
    public int PatientId { get; set; }

    public string PatientName { get; set; } = null!;

    public DateTime? AppointmentDate { get; set; }

    public string? SeenBy { get; set; }

    public int? PrescriptionId { get; set; }

    public string? Medication { get; set; }

    public string? Dosage { get; set; }

    public string? LabResult { get; set; }
}
