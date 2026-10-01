namespace HMS.Api.DTOs.Departments;

public class DepartmentResponseDto
{
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public int TotalStaffCount { get; set; }
}
