using HMS.Domain.Entities.Admissions;
using HMS.Domain.Entities.Appointments;
using HMS.Domain.Entities.Departments;
using HMS.Domain.Entities.Doctors;
using HMS.Domain.Entities.Insurances;
using HMS.Domain.Entities.Invoices;
using HMS.Domain.Entities.LabOrders;
using HMS.Domain.Entities.Medications;
using HMS.Domain.Entities.Nurses;
using HMS.Domain.Entities.Patients;
using HMS.Domain.Entities.Prescriptions;
using HMS.Domain.Entities.Rooms;
using HMS.Domain.Entities.Staff;
using Microsoft.EntityFrameworkCore;

namespace HMS.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Admission> Admissions => Set<Admission>();
    public DbSet<Medication> Medications => Set<Medication>();
    public DbSet<Prescription> Prescriptions => Set<Prescription>();
    public DbSet<PrescriptionDetail> PrescriptionDetails => Set<PrescriptionDetail>();
    public DbSet<LabTest> LabTests => Set<LabTest>();
    public DbSet<LabOrder> LabOrders => Set<LabOrder>();

    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Staff> Staff => Set<Staff>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<DoctorSchedule> DoctorSchedules => Set<DoctorSchedule>();
    public DbSet<Nurse> Nurses => Set<Nurse>();
    public DbSet<Room> Rooms => Set<Room>();

    public DbSet<InsuranceProvider> InsuranceProviders => Set<InsuranceProvider>();
    public DbSet<PatientInsurance> PatientInsurances => Set<PatientInsurance>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceItem> InvoiceItems => Set<InvoiceItem>();
    public DbSet<InvoiceStatusLog> InvoiceStatusLogs => Set<InvoiceStatusLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
