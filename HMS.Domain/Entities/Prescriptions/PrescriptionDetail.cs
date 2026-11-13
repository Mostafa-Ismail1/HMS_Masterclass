using HMS.Domain.Common;

namespace HMS.Domain.Entities.Prescriptions;

public class PrescriptionDetail
{
    private PrescriptionDetail(int prescriptionId, int medicationId, string dosage, string duration)
    {
        PrescriptionId = prescriptionId;
        MedicationId = medicationId;
        Dosage = dosage;
        Duration = duration;
    }

    private PrescriptionDetail()
    {
    }

    public int PrescriptionId { get; private set; }
    public int MedicationId { get; private set; }
    public string Dosage { get; private set; } = null!;
    public string Duration { get; private set; } = null!;

    public static Result<PrescriptionDetail> Create(int prescriptionId, int medicationId, string dosage, string duration)
    {
        if (string.IsNullOrWhiteSpace(dosage))
        {
            return Result.Failure<PrescriptionDetail>(PrescriptionErrors.EmptyDosage);
        }

        return Result.Success(new PrescriptionDetail(prescriptionId, medicationId, dosage.Trim(), duration.Trim()));
    }
}
