using HMS.Application.Common.Interfaces;
using HMS.Domain.Common;
using MediatR;

namespace HMS.Application.Patients.Queries.GetPatients;

public record GetPatientsQuery(int Page = 1, int PageSize = 10) : IRequest<Result<IReadOnlyList<PatientDto>>>;
