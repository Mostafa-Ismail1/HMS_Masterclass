using System;
using System.Collections.Generic;

namespace HMS.Api.Data.Entities;

public partial class VwInvoiceTotal
{
    public int InvoiceId { get; set; }

    public DateOnly IssueDate { get; set; }

    public int PatientId { get; set; }

    public string Status { get; set; } = null!;

    public decimal? ComputedTotal { get; set; }
}
