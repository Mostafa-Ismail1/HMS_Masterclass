namespace HMS.Api.DTOs.Patients;

public class UpdatePatientRequestDto
{
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Address { get; set; }
}
