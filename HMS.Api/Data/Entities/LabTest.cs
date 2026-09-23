using System;
using System.Collections.Generic;

namespace HMS.Api.Data.Entities;

public partial class LabTest
{
    public int LabTestId { get; set; }

    public string TestName { get; set; } = null!;

    public decimal Cost { get; set; }

    public virtual ICollection<LabOrder> LabOrders { get; set; } = new List<LabOrder>();
}
