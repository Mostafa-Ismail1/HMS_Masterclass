using HMS.Domain.Entities.Medications;

namespace HMS.Application.Common.Interfaces;

public record MedicationDto(int MedicationId, string Name, string? Manufacturer, decimal UnitPrice);

public interface IMedicationRepository
{
    Task<int> AddAsync(Medication medication, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<MedicationDto>> GetAllAsync(CancellationToken cancellationToken = default);
}
