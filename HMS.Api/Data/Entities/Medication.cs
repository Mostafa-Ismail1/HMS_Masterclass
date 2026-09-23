using System;
using System.Collections.Generic;

namespace HMS.Api.Data.Entities;

public partial class Medication
{
    public int MedicationId { get; set; }

    public string Name { get; set; } = null!;

    public string? Manufacturer { get; set; }

    public decimal UnitPrice { get; set; }

    public virtual ICollection<PrescriptionDetail> PrescriptionDetails { get; set; } = new List<PrescriptionDetail>();
}
