using System;
using System.Collections.Generic;

namespace HMS.Api.Data.Entities;

public partial class Invoice
{
    public int InvoiceId { get; set; }

    public int PatientId { get; set; }

    public int? AdmissionId { get; set; }

    public decimal TotalAmount { get; set; }

    public DateOnly IssueDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual Admission? Admission { get; set; }

    public virtual ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();

    public virtual Patient Patient { get; set; } = null!;
}
