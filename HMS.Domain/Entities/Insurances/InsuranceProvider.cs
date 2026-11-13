using HMS.Domain.Common;

namespace HMS.Domain.Entities.Insurances;

public class InsuranceProvider : Entity<int>
{
    private InsuranceProvider(string providerName, string contactPhone)
    {
        ProviderName = providerName;
        ContactPhone = contactPhone;
    }

    private InsuranceProvider()
    {
    }

    public string ProviderName { get; private set; } = null!;
    public string ContactPhone { get; private set; } = null!;

    public static Result<InsuranceProvider> Create(string providerName, string contactPhone)
    {
        if (string.IsNullOrWhiteSpace(providerName))
        {
            return Result.Failure<InsuranceProvider>(InsuranceErrors.EmptyProviderName);
        }

        return Result.Success(new InsuranceProvider(providerName.Trim(), contactPhone.Trim()));
    }
}
