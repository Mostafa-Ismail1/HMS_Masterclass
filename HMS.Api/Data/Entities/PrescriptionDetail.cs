using System;
using System.Collections.Generic;

namespace HMS.Api.Data.Entities;

public partial class PrescriptionDetail
{
    public int PrescriptionId { get; set; }

    public int MedicationId { get; set; }

    public string Dosage { get; set; } = null!;

    public string Duration { get; set; } = null!;

    public virtual Medication Medication { get; set; } = null!;

    public virtual Prescription Prescription { get; set; } = null!;
}
