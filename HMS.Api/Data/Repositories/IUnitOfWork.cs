using HMS.Api.Data.Entities;

namespace HMS.Api.Data.Repositories;

public interface IUnitOfWork : IDisposable
{
    IDepartmentRepository Departments { get; }

    IPatientRepository Patients { get; }

    IDoctorRepository Doctors { get; }

    IGenericRepository<Staff> Staff { get; }

    IGenericRepository<Appointment> Appointments { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
