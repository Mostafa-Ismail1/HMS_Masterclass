using HMS.Application.Common.Interfaces;
using HMS.Domain.Entities.Appointments;
using Microsoft.EntityFrameworkCore;

namespace HMS.Infrastructure.Persistence.Repositories;

public class AppointmentRepository(ApplicationDbContext context) : IAppointmentRepository
{
    public async Task<int> AddAsync(Appointment appointment, CancellationToken cancellationToken = default)
    {
        await context.Appointments.AddAsync(appointment, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return appointment.Id;
    }

    public async Task<Appointment?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Appointments
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(Appointment appointment, CancellationToken cancellationToken = default)
    {
        context.Appointments.Update(appointment);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<AppointmentDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Appointments
            .AsNoTracking()
            .OrderByDescending(a => a.AppointmentDate)
            .Select(a => new AppointmentDto(
                a.Id,
                a.PatientId,
                a.DoctorId,
                a.AppointmentDate,
                a.Status.ToString(),
                a.Reason))
            .ToListAsync(cancellationToken);
    }
}
