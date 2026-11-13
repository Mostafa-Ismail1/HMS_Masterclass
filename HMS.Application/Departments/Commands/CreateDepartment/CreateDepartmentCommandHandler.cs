using HMS.Application.Common.Interfaces;
using HMS.Domain.Common;
using HMS.Domain.Entities.Departments;
using MediatR;

namespace HMS.Application.Departments.Commands.CreateDepartment;

public class CreateDepartmentCommandHandler(IDepartmentRepository departmentRepository)
    : IRequestHandler<CreateDepartmentCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var departmentResult = Department.Create(request.DepartmentName, request.Location);
        if (departmentResult.IsFailure)
        {
            return Result.Failure<int>(departmentResult.Error);
        }

        var departmentId = await departmentRepository.AddAsync(departmentResult.Value, cancellationToken);
        return Result.Success(departmentId);
    }
}
