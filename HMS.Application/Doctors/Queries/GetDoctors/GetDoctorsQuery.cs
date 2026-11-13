using HMS.Application.Common.Interfaces;
using HMS.Domain.Common;
using MediatR;

namespace HMS.Application.Doctors.Queries.GetDoctors;

public record GetDoctorsQuery() : IRequest<Result<IReadOnlyList<DoctorDto>>>;
