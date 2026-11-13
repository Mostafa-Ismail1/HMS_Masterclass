using HMS.Application.Common.Interfaces;
using HMS.Domain.Common;
using MediatR;

namespace HMS.Application.Departments.Queries.GetDepartments;

public record GetDepartmentsQuery() : IRequest<Result<IReadOnlyList<DepartmentDto>>>;
