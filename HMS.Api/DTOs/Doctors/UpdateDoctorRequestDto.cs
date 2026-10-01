namespace HMS.Api.DTOs.Doctors;

public class UpdateDoctorRequestDto
{
    public string Specialization { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
