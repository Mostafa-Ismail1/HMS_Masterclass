namespace HMS.Api;

public class HospitalSettingsOptions
{
    public const string SectionName = "HospitalSettings";
    public string HospitalName { get; set; } = string.Empty;
    public int MaxBedCapacity { get; set; }
    public decimal DefaultConsultationFee { get; set; }

}
