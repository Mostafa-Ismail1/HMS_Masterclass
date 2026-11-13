using HMS.Domain.Common;

namespace HMS.Domain.Entities.LabOrders;

public static class LabOrderErrors
{
    public static readonly Error EmptyTestName = Error.Validation(
        "LabTest.EmptyName",
        "Lab test name cannot be empty.");

    public static readonly Error InvalidCost = Error.Validation(
        "LabTest.InvalidCost",
        "Lab test cost must be non-negative.");

    public static readonly Error OrderAlreadyCompleted = Error.Conflict(
        "LabOrder.AlreadyCompleted",
        "Lab order results have already been finalized.");

    public static readonly Error NotFound = Error.NotFound(
        "LabOrder.NotFound",
        "Lab order was not found.");
}
