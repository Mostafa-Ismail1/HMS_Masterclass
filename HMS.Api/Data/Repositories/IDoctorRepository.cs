using HMS.Api.Data.Entities;

namespace HMS.Api.Data.Repositories;

public interface IDoctorRepository : IGenericRepository<Doctor>
{
    Task<IReadOnlyList<Doctor>> GetAllWithDetailsAsync(
        string? specialization,
        CancellationToken cancellationToken = default);

    Task<Doctor?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
}
