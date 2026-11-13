using HMS.Application.Common.Interfaces;
using HMS.Domain.Common;
using HMS.Domain.Entities.Patients;
using MediatR;

namespace HMS.Application.Patients.Commands.RegisterPatient;

public class RegisterPatientCommandHandler(IPatientRepository patientRepository)
    : IRequestHandler<RegisterPatientCommand, Result<int>>
{
    public async Task<Result<int>> Handle(
        RegisterPatientCommand request,
        CancellationToken cancellationToken)
    {
        var patientResult = Patient.Create(
            request.FirstName,
            request.LastName,
            request.Dob,
            request.Phone,
            request.Gender,
            request.Email,
            request.Address,
            request.BloodType);

        if (patientResult.IsFailure)
        {
            return Result.Failure<int>(patientResult.Error);
        }

        var patientId = await patientRepository.AddAsync(patientResult.Value, cancellationToken);

        return Result.Success(patientId);
    }
}
