using HMS.Domain.Common;
using MediatR;

namespace HMS.Application.Patients.Commands.RegisterPatient;

public record RegisterPatientCommand(
    string FirstName,
    string LastName,
    DateOnly Dob,
    string Phone,
    string Gender,
    string? Email = null,
    string? Address = null,
    string? BloodType = null) : IRequest<Result<int>>;
