using System;
using System.Collections.Generic;
using HMS.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HMS.Api.Data;

public partial class HmsDbContext : DbContext
{
    public HmsDbContext(DbContextOptions<HmsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admission> Admissions { get; set; }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<DoctorSchedule> DoctorSchedules { get; set; }

    public virtual DbSet<InsuranceProvider> InsuranceProviders { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<InvoiceItem> InvoiceItems { get; set; }

    public virtual DbSet<InvoiceStatusLog> InvoiceStatusLogs { get; set; }

    public virtual DbSet<LabOrder> LabOrders { get; set; }

    public virtual DbSet<LabTest> LabTests { get; set; }

    public virtual DbSet<Medication> Medications { get; set; }

    public virtual DbSet<Nurse> Nurses { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<PatientInsurance> PatientInsurances { get; set; }

    public virtual DbSet<Prescription> Prescriptions { get; set; }

    public virtual DbSet<PrescriptionDetail> PrescriptionDetails { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<VwDoctorWorkload> VwDoctorWorkloads { get; set; }

    public virtual DbSet<VwInvoiceTotal> VwInvoiceTotals { get; set; }

    public virtual DbSet<VwPatientFullHistory> VwPatientFullHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasSequence<int>("InvoiceRefSeq", "billing").StartsAt(1000L);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HmsDbContext).Assembly);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
