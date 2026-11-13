using HMS.Domain.Common;

namespace HMS.Domain.Entities.Patients;

public class Patient : Entity<int>
{
    private Patient(
        string firstName,
        string lastName,
        DateOnly dob,
        string phone,
        string gender,
        string? email,
        string? address,
        string? bloodType)
    {
        FirstName = firstName;
        LastName = lastName;
        Dob = dob;
        Phone = phone;
        Gender = gender;
        Email = email;
        Address = address;
        BloodType = bloodType;
    }

    private Patient()
    {
    }

    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public DateOnly Dob { get; private set; }
    public string Phone { get; private set; } = null!;
    public string Gender { get; private set; } = null!;
    public string? Email { get; private set; }
    public string? Address { get; private set; }
    public string? BloodType { get; private set; }

    public static Result<Patient> Create(
        string firstName,
        string lastName,
        DateOnly dob,
        string phone,
        string gender,
        string? email = null,
        string? address = null,
        string? bloodType = null)
    {
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        {
            return Result.Failure<Patient>(PatientErrors.EmptyName);
        }

        if (dob > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            return Result.Failure<Patient>(PatientErrors.DobInFuture);
        }

        var normalizedGender = gender?.Trim().ToUpperInvariant();
        if (normalizedGender != "M" && normalizedGender != "F")
        {
            return Result.Failure<Patient>(PatientErrors.InvalidGender);
        }

        var patient = new Patient(
            firstName.Trim(),
            lastName.Trim(),
            dob,
            phone.Trim(),
            normalizedGender,
            email?.Trim(),
            address?.Trim(),
            bloodType?.Trim());

        return Result.Success(patient);
    }

    public Result UpdateContactDetails(string phone, string? email, string? address)
    {
        Phone = phone.Trim();
        Email = email?.Trim();
        Address = address?.Trim();
        return Result.Success();
    }

    public Result UpdateMedicalDetails(string? bloodType)
    {
        BloodType = bloodType?.Trim();
        return Result.Success();
    }
}
