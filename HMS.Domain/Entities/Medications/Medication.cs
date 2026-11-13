using HMS.Domain.Common;

namespace HMS.Domain.Entities.Medications;

public class Medication : Entity<int>
{
    private Medication(string name, string? manufacturer, decimal unitPrice)
    {
        Name = name;
        Manufacturer = manufacturer;
        UnitPrice = unitPrice;
    }

    private Medication()
    {
    }

    public string Name { get; private set; } = null!;
    public string? Manufacturer { get; private set; }
    public decimal UnitPrice { get; private set; }

    public static Result<Medication> Create(string name, string? manufacturer, decimal unitPrice)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Medication>(MedicationErrors.EmptyName);
        }

        if (unitPrice <= 0)
        {
            return Result.Failure<Medication>(MedicationErrors.InvalidPrice);
        }

        return Result.Success(new Medication(name.Trim(), manufacturer?.Trim(), unitPrice));
    }
}
