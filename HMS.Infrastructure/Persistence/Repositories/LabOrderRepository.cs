using HMS.Application.Common.Interfaces;
using HMS.Domain.Entities.LabOrders;
using Microsoft.EntityFrameworkCore;

namespace HMS.Infrastructure.Persistence.Repositories;

public class LabOrderRepository(ApplicationDbContext context) : ILabOrderRepository
{
    public async Task<int> AddOrderAsync(LabOrder order, CancellationToken cancellationToken = default)
    {
        await context.LabOrders.AddAsync(order, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return order.Id;
    }

    public async Task<LabOrder?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.LabOrders.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task UpdateOrderAsync(LabOrder order, CancellationToken cancellationToken = default)
    {
        context.LabOrders.Update(order);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<LabOrderDto>> GetAllOrdersAsync(CancellationToken cancellationToken = default)
    {
        return await context.LabOrders
            .AsNoTracking()
            .OrderByDescending(o => o.Id)
            .Select(o => new LabOrderDto(o.Id, o.PatientId, o.DoctorId, o.LabTestId, o.OrderDate, o.ResultDate, o.ResultValue, o.Status))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<LabTestDto>> GetAllTestsAsync(CancellationToken cancellationToken = default)
    {
        return await context.LabTests
            .AsNoTracking()
            .OrderBy(t => t.TestName)
            .Select(t => new LabTestDto(t.Id, t.TestName, t.Cost))
            .ToListAsync(cancellationToken);
    }
}
