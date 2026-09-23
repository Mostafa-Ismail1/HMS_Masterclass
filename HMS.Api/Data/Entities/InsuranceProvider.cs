using System;
using System.Collections.Generic;

namespace HMS.Api.Data.Entities;

public partial class InsuranceProvider
{
    public int ProviderId { get; set; }

    public string ProviderName { get; set; } = null!;

    public string ContactPhone { get; set; } = null!;

    public virtual ICollection<PatientInsurance> PatientInsurances { get; set; } = new List<PatientInsurance>();
}
