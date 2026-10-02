using HMS.Api.Data.Entities;

namespace HMS.Api.Data.Repositories;

public interface IPatientRepository : IGenericRepository<Patient>
{
    Task<IReadOnlyList<Patient>> GetPatientsAsync(
        string? bloodType,
        string? gender,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<Patient?> GetByIdWithAppointmentsAsync(int id, CancellationToken cancellationToken = default);
}
