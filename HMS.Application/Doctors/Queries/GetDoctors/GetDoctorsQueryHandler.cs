using HMS.Application.Common.Interfaces;
using HMS.Domain.Common;
using MediatR;

namespace HMS.Application.Doctors.Queries.GetDoctors;

public class GetDoctorsQueryHandler(IDoctorRepository doctorRepository)
    : IRequestHandler<GetDoctorsQuery, Result<IReadOnlyList<DoctorDto>>>
{
    public async Task<Result<IReadOnlyList<DoctorDto>>> Handle(GetDoctorsQuery request, CancellationToken cancellationToken)
    {
        var doctors = await doctorRepository.GetAllAsync(cancellationToken);
        return Result.Success(doctors);
    }
}
