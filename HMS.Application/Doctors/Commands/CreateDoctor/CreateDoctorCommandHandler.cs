using HMS.Application.Common.Interfaces;
using HMS.Domain.Common;
using HMS.Domain.Entities.Doctors;
using MediatR;

namespace HMS.Application.Doctors.Commands.CreateDoctor;

public class CreateDoctorCommandHandler(IDoctorRepository doctorRepository)
    : IRequestHandler<CreateDoctorCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
    {
        var doctorResult = Doctor.Create(request.DoctorId, request.Specialization, request.LicenseNumber);
        if (doctorResult.IsFailure)
        {
            return Result.Failure<int>(doctorResult.Error);
        }

        var doctorId = await doctorRepository.AddAsync(doctorResult.Value, cancellationToken);
        return Result.Success(doctorId);
    }
}
