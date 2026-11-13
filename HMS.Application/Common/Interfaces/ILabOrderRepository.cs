using HMS.Domain.Entities.LabOrders;

namespace HMS.Application.Common.Interfaces;

public record LabTestDto(int LabTestId, string TestName, decimal Cost);
public record LabOrderDto(int LabOrderId, int PatientId, int DoctorId, int LabTestId, DateOnly OrderDate, DateOnly? ResultDate, string? ResultValue, string Status);

public interface ILabOrderRepository
{
    Task<int> AddOrderAsync(LabOrder order, CancellationToken cancellationToken = default);
    Task<LabOrder?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task UpdateOrderAsync(LabOrder order, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<LabOrderDto>> GetAllOrdersAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<LabTestDto>> GetAllTestsAsync(CancellationToken cancellationToken = default);
}
