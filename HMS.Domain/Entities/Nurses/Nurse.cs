using HMS.Domain.Common;

namespace HMS.Domain.Entities.Nurses;

public class Nurse : Entity<int>
{
    private Nurse(int nurseId, string shiftType)
    {
        Id = nurseId;
        ShiftType = shiftType;
    }

    private Nurse()
    {
    }

    public string ShiftType { get; private set; } = null!;

    public static Result<Nurse> Create(int nurseId, string shiftType)
    {
        return Result.Success(new Nurse(nurseId, shiftType.Trim()));
    }
}
