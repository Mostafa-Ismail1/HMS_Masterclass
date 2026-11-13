using Dapper;
using HMS.Application.Common.Interfaces;
using HMS.Domain.Common;
using MediatR;

namespace HMS.Application.Appointments.Queries.GetAppointments;

public record GetAppointmentsQuery : IRequest<Result<List<AppointmentDto>>>;

public class GetAppointmentsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    : IRequestHandler<GetAppointmentsQuery, Result<List<AppointmentDto>>>
{
    public async Task<Result<List<AppointmentDto>>> Handle(
        GetAppointmentsQuery request,
        CancellationToken cancellationToken)
    {
        using var connection = sqlConnectionFactory.CreateConnection();

        const string sql = """
            SELECT 
                AppointmentID AS Id,
                PatientID AS PatientId,
                DoctorID AS DoctorId,
                AppointmentDate,
                Status,
                Reason
            FROM clinical.Appointments
            ORDER BY AppointmentDate DESC
            """;

        var appointments = await connection.QueryAsync<AppointmentDto>(sql);

        return Result.Success(appointments.ToList());
    }
}
