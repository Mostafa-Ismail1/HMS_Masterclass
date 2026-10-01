namespace HMS.Api.DTOs.Patients;

public class PatientResponseDto
{
    public int PatientId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateOnly Dob { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? BloodType { get; set; }
    public int ActiveAppointmentsCount { get; set; }
}
