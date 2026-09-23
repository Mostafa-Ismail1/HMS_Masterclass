using System;
using System.Collections.Generic;

namespace HMS.Api.Data.Entities;

public partial class Patient
{
    public int PatientId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly Dob { get; set; }

    public string Phone { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? BloodType { get; set; }

    public virtual ICollection<Admission> Admissions { get; set; } = new List<Admission>();

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<LabOrder> LabOrders { get; set; } = new List<LabOrder>();

    public virtual ICollection<PatientInsurance> PatientInsurances { get; set; } = new List<PatientInsurance>();

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}
