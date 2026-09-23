using System;
using System.Collections.Generic;

namespace HMS.Api.Data.Entities;

public partial class Nurse
{
    public int NurseId { get; set; }

    public string ShiftType { get; set; } = null!;

    public virtual Staff NurseNavigation { get; set; } = null!;
}
