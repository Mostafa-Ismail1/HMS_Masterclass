using HMS.Domain.Entities.Patients;

namespace HMS.Application.Common.Interfaces;

public class PatientDto
{
    public PatientDto() { }

    public PatientDto(
        int patientId,
        string fullName,
        DateOnly dob,
        string phone,
        string gender,
        string? email,
        string? address,
        string? bloodType)
    {
        PatientId = patientId;
        FullName = fullName;
        Dob = dob;
        Phone = phone;
        Gender = gender;
        Email = email;
        Address = address;
        BloodType = bloodType;
    }

    public int PatientId { get; init; }
    public string FullName { get; init; } = string.Empty;
    public DateOnly Dob { get; init; }
    public string Phone { get; init; } = string.Empty;
    public string Gender { get; init; } = string.Empty;
    public string? Email { get; init; }
    public string? Address { get; init; }
    public string? BloodType { get; init; }
}

public interface IPatientRepository
{
    Task<int> AddAsync(Patient patient, CancellationToken cancellationToken = default);
    Task<Patient?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task UpdateAsync(Patient patient, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PatientDto>> GetAllAsync(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default);
}
