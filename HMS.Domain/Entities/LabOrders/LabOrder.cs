using HMS.Domain.Common;

namespace HMS.Domain.Entities.LabOrders;

public class LabOrder : Entity<int>
{
    private LabOrder(int patientId, int doctorId, int labTestId, DateOnly orderDate)
    {
        PatientId = patientId;
        DoctorId = doctorId;
        LabTestId = labTestId;
        OrderDate = orderDate;
        Status = "Pending";
    }

    private LabOrder()
    {
    }

    public int PatientId { get; private set; }
    public int DoctorId { get; private set; }
    public int LabTestId { get; private set; }
    public DateOnly OrderDate { get; private set; }
    public DateOnly? ResultDate { get; private set; }
    public string? ResultValue { get; private set; }
    public string Status { get; private set; } = null!;

    public static Result<LabOrder> Create(int patientId, int doctorId, int labTestId, DateOnly orderDate)
    {
        return Result.Success(new LabOrder(patientId, doctorId, labTestId, orderDate));
    }

    public Result RecordResult(string resultValue, DateOnly resultDate)
    {
        if (Status == "Completed")
        {
            return Result.Failure(LabOrderErrors.OrderAlreadyCompleted);
        }

        ResultValue = resultValue.Trim();
        ResultDate = resultDate;
        Status = "Completed";
        return Result.Success();
    }
}
