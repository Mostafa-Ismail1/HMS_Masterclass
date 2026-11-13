using HMS.Domain.Common;
using MediatR;

namespace HMS.Application.Appointments.Commands.BookAppointment;

public record BookAppointmentCommand(
    int PatientId,
    int DoctorId,
    DateTime AppointmentDate,
    string? Reason) : IRequest<Result<int>>;
