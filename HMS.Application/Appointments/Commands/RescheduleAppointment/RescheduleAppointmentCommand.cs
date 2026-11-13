using HMS.Domain.Common;
using MediatR;

namespace HMS.Application.Appointments.Commands.RescheduleAppointment;

public record RescheduleAppointmentCommand(int AppointmentId, DateTime NewAppointmentDate) : IRequest<Result>;
