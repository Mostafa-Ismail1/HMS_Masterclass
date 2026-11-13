using HMS.Domain.Common;
using MediatR;

namespace HMS.Application.Departments.Commands.CreateDepartment;

public record CreateDepartmentCommand(string DepartmentName, string Location) : IRequest<Result<int>>;
