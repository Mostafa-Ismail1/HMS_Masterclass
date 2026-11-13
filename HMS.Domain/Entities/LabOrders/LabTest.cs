using HMS.Domain.Common;

namespace HMS.Domain.Entities.LabOrders;

public class LabTest : Entity<int>
{
    private LabTest(string testName, decimal cost)
    {
        TestName = testName;
        Cost = cost;
    }

    private LabTest()
    {
    }

    public string TestName { get; private set; } = null!;
    public decimal Cost { get; private set; }

    public static Result<LabTest> Create(string testName, decimal cost)
    {
        if (string.IsNullOrWhiteSpace(testName))
        {
            return Result.Failure<LabTest>(LabOrderErrors.EmptyTestName);
        }

        if (cost < 0)
        {
            return Result.Failure<LabTest>(LabOrderErrors.InvalidCost);
        }

        return Result.Success(new LabTest(testName.Trim(), cost));
    }
}
