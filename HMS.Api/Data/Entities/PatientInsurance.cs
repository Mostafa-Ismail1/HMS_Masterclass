using System;
using System.Collections.Generic;

namespace HMS.Api.Data.Entities;

public partial class PatientInsurance
{
    public int PatientId { get; set; }

    public int ProviderId { get; set; }

    public string PolicyNumber { get; set; } = null!;

    public DateOnly ExpiryDate { get; set; }

    public virtual Patient Patient { get; set; } = null!;

    public virtual InsuranceProvider Provider { get; set; } = null!;
}
