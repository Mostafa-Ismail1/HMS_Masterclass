using Dapper;
using HMS.Application.Common.Interfaces;
using HMS.Domain.Common;
using MediatR;

namespace HMS.Application.Patients.Queries.GetPatients;

public class GetPatientsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    : IRequestHandler<GetPatientsQuery, Result<IReadOnlyList<PatientDto>>>
{
    public async Task<Result<IReadOnlyList<PatientDto>>> Handle(
        GetPatientsQuery request,
        CancellationToken cancellationToken)
    {
        using var connection = sqlConnectionFactory.CreateConnection();

        const string sql = """
            SELECT 
                PatientID AS PatientId,
                CONCAT(FirstName, ' ', LastName) AS FullName,
                DOB AS Dob,
                Phone,
                Gender,
                Email,
                Address,
                BloodType
            FROM clinical.Patients
            ORDER BY PatientID
            OFFSET @Offset ROWS
            FETCH NEXT @PageSize ROWS ONLY
            """;

        var patients = await connection.QueryAsync<PatientDto>(sql, new
        {
            Offset = (request.Page - 1) * request.PageSize,
            PageSize = request.PageSize
        });

        return Result.Success<IReadOnlyList<PatientDto>>(patients.ToList());
    }
}
