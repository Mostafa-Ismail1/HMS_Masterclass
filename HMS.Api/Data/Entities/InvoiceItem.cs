using System;
using System.Collections.Generic;

namespace HMS.Api.Data.Entities;

public partial class InvoiceItem
{
    public int InvoiceItemId { get; set; }

    public int InvoiceId { get; set; }

    public DateOnly IssueDate { get; set; }

    public string Description { get; set; } = null!;

    public decimal Amount { get; set; }

    public virtual Invoice Invoice { get; set; } = null!;
}
