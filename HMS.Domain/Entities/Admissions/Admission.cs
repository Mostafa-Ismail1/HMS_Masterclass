using HMS.Domain.Common;

namespace HMS.Domain.Entities.Admissions;

public class Admission : Entity<int>
{
    private Admission(int patientId, int roomId, int admittingDoctorId, DateTime admissionDate)
    {
        PatientId = patientId;
        RoomId = roomId;
        AdmittingDoctorId = admittingDoctorId;
        AdmissionDate = admissionDate;
    }

    private Admission()
    {
    }

    public int PatientId { get; private set; }
    public int RoomId { get; private set; }
    public int AdmittingDoctorId { get; private set; }
    public DateTime AdmissionDate { get; private set; }
    public DateTime? DischargeDate { get; private set; }

    public static Result<Admission> Create(int patientId, int roomId, int admittingDoctorId, DateTime admissionDate)
    {
        return Result.Success(new Admission(patientId, roomId, admittingDoctorId, admissionDate));
    }

    public Result Discharge(DateTime dischargeDate)
    {
        if (DischargeDate.HasValue)
        {
            return Result.Failure(AdmissionErrors.AlreadyDischarged);
        }

        if (dischargeDate < AdmissionDate)
        {
            return Result.Failure(AdmissionErrors.InvalidDates);
        }

        DischargeDate = dischargeDate;
        return Result.Success();
    }
}
