using HMS.Domain.Common;
using MediatR;

namespace HMS.Application.Medications.Commands.CreateMedication;

public record CreateMedicationCommand(string Name, string? Manufacturer, decimal UnitPrice) : IRequest<Result<int>>;
