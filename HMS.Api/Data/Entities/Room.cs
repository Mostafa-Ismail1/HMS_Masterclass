using System;
using System.Collections.Generic;

namespace HMS.Api.Data.Entities;

public partial class Room
{
    public int RoomId { get; set; }

    public string RoomNumber { get; set; } = null!;

    public string RoomType { get; set; } = null!;

    public int DepartmentId { get; set; }

    public decimal DailyRate { get; set; }

    public virtual ICollection<Admission> Admissions { get; set; } = new List<Admission>();

    public virtual Department Department { get; set; } = null!;
}
