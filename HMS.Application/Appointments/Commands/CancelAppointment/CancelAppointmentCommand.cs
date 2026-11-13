using HMS.Domain.Common;
using MediatR;

namespace HMS.Application.Appointments.Commands.CancelAppointment;

public record CancelAppointmentCommand(int AppointmentId) : IRequest<Result>;
