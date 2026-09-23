using System;
using System.Collections.Generic;

namespace HMS.Api.Data.Entities;

public partial class Admission
{
    public int AdmissionId { get; set; }

    public int PatientId { get; set; }

    public int RoomId { get; set; }

    public int AdmittingDoctorId { get; set; }

    public DateTime AdmissionDate { get; set; }

    public DateTime? DischargeDate { get; set; }

    public virtual Doctor AdmittingDoctor { get; set; } = null!;

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual Patient Patient { get; set; } = null!;

    public virtual Room Room { get; set; } = null!;
}
