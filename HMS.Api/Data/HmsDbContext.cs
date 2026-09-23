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
        modelBuilder.Entity<Admission>(entity =>
        {
            entity.HasKey(e => e.AdmissionId).HasName("PK__Admissio__C97EEFA293764032");

            entity.ToTable("Admissions", "clinical");

            entity.Property(e => e.AdmissionId).HasColumnName("AdmissionID");
            entity.Property(e => e.AdmissionDate).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.AdmittingDoctorId).HasColumnName("AdmittingDoctorID");
            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");

            entity.HasOne(d => d.AdmittingDoctor).WithMany(p => p.Admissions)
                .HasForeignKey(d => d.AdmittingDoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Adm_Doctor");

            entity.HasOne(d => d.Patient).WithMany(p => p.Admissions)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Admissions_Patient");

            entity.HasOne(d => d.Room).WithMany(p => p.Admissions)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Admissions_Room");
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCA2B3D6CDE6");

            entity.ToTable("Appointments", "clinical", tb => tb.HasTrigger("trg_PreventOverbooking"));

            entity.HasIndex(e => e.AppointmentDate, "IX_Appointments_Date");

            entity.HasIndex(e => e.AppointmentDate, "IX_Appointments_Scheduled_Filtered").HasFilter("([Status]='Scheduled')");

            entity.HasIndex(e => new { e.DoctorId, e.AppointmentDate }, "UQ_Doctor_Slot").IsUnique();

            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.Reason)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasDefaultValue("Scheduled");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_App_Doctor");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appointments_Patient");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__B2079BCD245E440B");

            entity.ToTable("Departments", "hr");

            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.DoctorId).HasName("PK__Doctors__2DC00EDFE926E959");

            entity.ToTable("Doctors", "hr");

            entity.HasIndex(e => e.LicenseNumber, "UQ__Doctors__E88901669ECF2ACC").IsUnique();

            entity.Property(e => e.DoctorId)
                .ValueGeneratedNever()
                .HasColumnName("DoctorID");
            entity.Property(e => e.LicenseNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Specialization)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.DoctorNavigation).WithOne(p => p.Doctor)
                .HasForeignKey<Doctor>(d => d.DoctorId)
                .HasConstraintName("FK_Doctors_Staff");
        });

        modelBuilder.Entity<DoctorSchedule>(entity =>
        {
            entity.HasKey(e => new { e.DoctorId, e.DepartmentId, e.DayOfWeek });

            entity.ToTable("DoctorSchedules", "hr");

            entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.DayOfWeek)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.Department).WithMany(p => p.DoctorSchedules)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DoctorSch__Depar__1DB06A4F");

            entity.HasOne(d => d.Doctor).WithMany(p => p.DoctorSchedules)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DoctorSch__Docto__1CBC4616");
        });

        modelBuilder.Entity<InsuranceProvider>(entity =>
        {
            entity.HasKey(e => e.ProviderId).HasName("PK__Insuranc__B54C689DD6937BB9");

            entity.ToTable("InsuranceProviders", "billing");

            entity.HasIndex(e => e.ProviderName, "UQ__Insuranc__7D057CE57B49CCD0").IsUnique();

            entity.Property(e => e.ProviderId).HasColumnName("ProviderID");
            entity.Property(e => e.ContactPhone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ProviderName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => new { e.InvoiceId, e.IssueDate });

            entity.ToTable("Invoices", "billing", tb => tb.HasTrigger("trg_LogInvoiceStatusChange"));

            entity.HasIndex(e => new { e.PatientId, e.IssueDate }, "IX_Invoices_Patient");

            entity.Property(e => e.InvoiceId)
                .ValueGeneratedOnAdd()
                .HasColumnName("InvoiceID");
            entity.Property(e => e.IssueDate).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.AdmissionId).HasColumnName("AdmissionID");
            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("Unpaid");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Admission).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.AdmissionId)
                .HasConstraintName("FK_Inv_Admission");

            entity.HasOne(d => d.Patient).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inv_Patient");
        });

        modelBuilder.Entity<InvoiceItem>(entity =>
        {
            entity.HasKey(e => e.InvoiceItemId).HasName("PK__InvoiceI__478FE0FCB15C9B86");

            entity.ToTable("InvoiceItems", "billing");

            entity.Property(e => e.InvoiceItemId).HasColumnName("InvoiceItemID");
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");

            entity.HasOne(d => d.Invoice).WithMany(p => p.InvoiceItems)
                .HasForeignKey(d => new { d.InvoiceId, d.IssueDate })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Items_Invoice");
        });

        modelBuilder.Entity<InvoiceStatusLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__InvoiceS__5E5499A8ADCF5CEC");

            entity.ToTable("InvoiceStatusLog", "billing");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.ChangedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");
            entity.Property(e => e.NewStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.OldStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LabOrder>(entity =>
        {
            entity.HasKey(e => e.LabOrderId).HasName("PK__LabOrder__9CBC009E8AC1ED8D");

            entity.ToTable("LabOrders", "clinical");

            entity.HasIndex(e => new { e.DoctorId, e.Status }, "IX_LabOrders_Doctor");

            entity.Property(e => e.LabOrderId).HasColumnName("LabOrderID");
            entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
            entity.Property(e => e.LabTestId).HasColumnName("LabTestID");
            entity.Property(e => e.OrderDate).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.ResultValue)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Doctor).WithMany(p => p.LabOrders)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lab_Doctor");

            entity.HasOne(d => d.LabTest).WithMany(p => p.LabOrders)
                .HasForeignKey(d => d.LabTestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lab_Test");

            entity.HasOne(d => d.Patient).WithMany(p => p.LabOrders)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lab_Patient");
        });

        modelBuilder.Entity<LabTest>(entity =>
        {
            entity.HasKey(e => e.LabTestId).HasName("PK__LabTests__64D339C51D216F08");

            entity.ToTable("LabTests", "clinical");

            entity.HasIndex(e => e.TestName, "UQ__LabTests__2AF07A7D2B799BB3").IsUnique();

            entity.Property(e => e.LabTestId).HasColumnName("LabTestID");
            entity.Property(e => e.Cost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TestName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Medication>(entity =>
        {
            entity.HasKey(e => e.MedicationId).HasName("PK__Medicati__62EC1ADAEDF5FA50");

            entity.ToTable("Medications", "clinical");

            entity.Property(e => e.MedicationId).HasColumnName("MedicationID");
            entity.Property(e => e.Manufacturer)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Nurse>(entity =>
        {
            entity.HasKey(e => e.NurseId).HasName("PK__Nurses__43847869AB73EFFE");

            entity.ToTable("Nurses", "hr");

            entity.Property(e => e.NurseId)
                .ValueGeneratedNever()
                .HasColumnName("NurseID");
            entity.Property(e => e.ShiftType)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.NurseNavigation).WithOne(p => p.Nurse)
                .HasForeignKey<Nurse>(d => d.NurseId)
                .HasConstraintName("FK_Nurses_Staff");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("PK__Patients__970EC346BA77CA92");

            entity
                .ToTable("Patients", "clinical")
                .ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("Patients_History", "clinical");
                        ttb
                            .HasPeriodStart("ValidFrom")
                            .HasColumnName("ValidFrom");
                        ttb
                            .HasPeriodEnd("ValidTo")
                            .HasColumnName("ValidTo");
                    }));

            entity.HasIndex(e => new { e.LastName, e.FirstName }, "IX_Patients_Name");

            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.BloodType)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Dob).HasColumnName("DOB");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("M")
                .IsFixedLength();
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PatientInsurance>(entity =>
        {
            entity.HasKey(e => new { e.PatientId, e.ProviderId });

            entity.ToTable("PatientInsurance", "billing");

            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.ProviderId).HasColumnName("ProviderID");
            entity.Property(e => e.PolicyNumber)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.Patient).WithMany(p => p.PatientInsurances)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PatientIn__Patie__0C85DE4D");

            entity.HasOne(d => d.Provider).WithMany(p => p.PatientInsurances)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PatientIn__Provi__0D7A0286");
        });

        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.HasKey(e => e.PrescriptionId).HasName("PK_PrescriptionID");

            entity.ToTable("Prescriptions", "clinical");

            entity.HasIndex(e => e.AppointmentId, "UQ__Prescrip__8ECDFCA39E2C5272").IsUnique();

            entity.Property(e => e.PrescriptionId).HasColumnName("PrescriptionID");
            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.DatePrescribed).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
            entity.Property(e => e.PatientId).HasColumnName("PatientID");

            entity.HasOne(d => d.Appointment).WithOne(p => p.Prescription)
                .HasForeignKey<Prescription>(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Presc_Appointment");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Prescriptions)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Presc_Doctor");

            entity.HasOne(d => d.Patient).WithMany(p => p.Prescriptions)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Presc_Patient");
        });

        modelBuilder.Entity<PrescriptionDetail>(entity =>
        {
            entity.HasKey(e => new { e.PrescriptionId, e.MedicationId }).HasName("PK_PrescDetails");

            entity.ToTable("PrescriptionDetails", "clinical");

            entity.Property(e => e.PrescriptionId).HasColumnName("PrescriptionID");
            entity.Property(e => e.MedicationId).HasColumnName("MedicationID");
            entity.Property(e => e.Dosage)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Duration)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Medication).WithMany(p => p.PrescriptionDetails)
                .HasForeignKey(d => d.MedicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PD_Medication");

            entity.HasOne(d => d.Prescription).WithMany(p => p.PrescriptionDetails)
                .HasForeignKey(d => d.PrescriptionId)
                .HasConstraintName("FK_PD_Prescription");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PK__Rooms__328639190A5B149A");

            entity.ToTable("Rooms", "hr");

            entity.HasIndex(e => e.RoomNumber, "UQ__Rooms__AE10E07A21D8398C").IsUnique();

            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.DailyRate).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.RoomNumber)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.RoomType)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Department).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rooms_Department");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__Staff__96D4AAF70D7F424A");

            entity.ToTable("Staff", "hr");

            entity.HasIndex(e => e.Email, "UQ__Staff__A9D105342096F3E6").IsUnique();

            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.HireDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.JobTitle)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Salary).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Department).WithMany(p => p.Staff)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Staff_Department");
        });

        modelBuilder.Entity<VwDoctorWorkload>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_DoctorWorkload", "hr");

            entity.Property(e => e.DepartmentName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
            entity.Property(e => e.DoctorName)
                .HasMaxLength(101)
                .IsUnicode(false);
            entity.Property(e => e.Specialization)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwInvoiceTotal>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_InvoiceTotals", "billing");

            entity.Property(e => e.ComputedTotal).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");
            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwPatientFullHistory>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_PatientFullHistory", "clinical");

            entity.Property(e => e.Dosage)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LabResult)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Medication)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.PatientName)
                .HasMaxLength(101)
                .IsUnicode(false);
            entity.Property(e => e.PrescriptionId).HasColumnName("PrescriptionID");
            entity.Property(e => e.SeenBy)
                .HasMaxLength(101)
                .IsUnicode(false);
        });
        modelBuilder.HasSequence<int>("InvoiceRefSeq", "billing").StartsAt(1000L);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
