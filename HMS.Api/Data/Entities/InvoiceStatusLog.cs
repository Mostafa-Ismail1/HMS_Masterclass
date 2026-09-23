using System;
using System.Collections.Generic;

namespace HMS.Api.Data.Entities;

public partial class InvoiceStatusLog
{
    public int LogId { get; set; }

    public int InvoiceId { get; set; }

    public string? OldStatus { get; set; }

    public string? NewStatus { get; set; }

    public DateTime? ChangedAt { get; set; }
}
