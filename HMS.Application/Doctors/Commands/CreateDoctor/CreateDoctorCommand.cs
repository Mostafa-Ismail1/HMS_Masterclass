using HMS.Domain.Common;
using MediatR;

namespace HMS.Application.Doctors.Commands.CreateDoctor;

public record CreateDoctorCommand(int DoctorId, string Specialization, string LicenseNumber) : IRequest<Result<int>>;
