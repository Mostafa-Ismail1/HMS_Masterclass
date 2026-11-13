using HMS.Domain.Common;

namespace HMS.Domain.Entities.Doctors;

public class Doctor : Entity<int>
{
    private Doctor(int doctorId, string specialization, string licenseNumber)
    {
        Id = doctorId;
        Specialization = specialization;
        LicenseNumber = licenseNumber;
    }

    private Doctor()
    {
    }

    public string Specialization { get; private set; } = null!;
    public string LicenseNumber { get; private set; } = null!;

    public static Result<Doctor> Create(int doctorId, string specialization, string licenseNumber)
    {
        if (string.IsNullOrWhiteSpace(specialization))
        {
            return Result.Failure<Doctor>(DoctorErrors.EmptySpecialization);
        }

        if (string.IsNullOrWhiteSpace(licenseNumber))
        {
            return Result.Failure<Doctor>(DoctorErrors.EmptyLicenseNumber);
        }

        return Result.Success(new Doctor(doctorId, specialization.Trim(), licenseNumber.Trim()));
    }
}
