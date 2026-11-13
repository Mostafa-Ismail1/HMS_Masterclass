using HMS.Domain.Common;

namespace HMS.Domain.Entities.Insurances;

public class PatientInsurance
{
    private PatientInsurance(int patientId, int providerId, string policyNumber, DateOnly expiryDate)
    {
        PatientId = patientId;
        ProviderId = providerId;
        PolicyNumber = policyNumber;
        ExpiryDate = expiryDate;
    }

    private PatientInsurance()
    {
    }

    public int PatientId { get; private set; }
    public int ProviderId { get; private set; }
    public string PolicyNumber { get; private set; } = null!;
    public DateOnly ExpiryDate { get; private set; }

    public static Result<PatientInsurance> Create(
        int patientId,
        int providerId,
        string policyNumber,
        DateOnly expiryDate)
    {
        if (string.IsNullOrWhiteSpace(policyNumber))
        {
            return Result.Failure<PatientInsurance>(InsuranceErrors.EmptyPolicyNumber);
        }

        if (expiryDate < DateOnly.FromDateTime(DateTime.UtcNow))
        {
            return Result.Failure<PatientInsurance>(InsuranceErrors.ExpiredPolicy);
        }

        return Result.Success(new PatientInsurance(patientId, providerId, policyNumber.Trim(), expiryDate));
    }
}
