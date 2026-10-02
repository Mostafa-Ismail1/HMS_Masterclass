using HMS.Api.Data.Entities;

namespace HMS.Api.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly HmsDbContext _context;
    private IDepartmentRepository? _departments;
    private IPatientRepository? _patients;
    private IDoctorRepository? _doctors;
    private IGenericRepository<Staff>? _staff;
    private IGenericRepository<Appointment>? _appointments;

    public UnitOfWork(HmsDbContext context)
    {
        _context = context;
    }

    public IDepartmentRepository Departments => _departments ??= new DepartmentRepository(_context);

    public IPatientRepository Patients => _patients ??= new PatientRepository(_context);

    public IDoctorRepository Doctors => _doctors ??= new DoctorRepository(_context);

    public IGenericRepository<Staff> Staff => _staff ??= new GenericRepository<Staff>(_context);

    public IGenericRepository<Appointment> Appointments => _appointments ??= new GenericRepository<Appointment>(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
