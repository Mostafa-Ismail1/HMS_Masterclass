using HMS.Application.Common.Interfaces;
using HMS.Domain.Common;
using MediatR;

namespace HMS.Application.Departments.Queries.GetDepartments;

public class GetDepartmentsQueryHandler(IDepartmentRepository departmentRepository)
    : IRequestHandler<GetDepartmentsQuery, Result<IReadOnlyList<DepartmentDto>>>
{
    public async Task<Result<IReadOnlyList<DepartmentDto>>> Handle(GetDepartmentsQuery request, CancellationToken cancellationToken)
    {
        var departments = await departmentRepository.GetAllAsync(cancellationToken);
        return Result.Success(departments);
    }
}
