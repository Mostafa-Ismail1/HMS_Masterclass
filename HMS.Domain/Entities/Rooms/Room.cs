using HMS.Domain.Common;

namespace HMS.Domain.Entities.Rooms;

public class Room : Entity<int>
{
    private Room(string roomNumber, string roomType, int departmentId, decimal dailyRate)
    {
        RoomNumber = roomNumber;
        RoomType = roomType;
        DepartmentId = departmentId;
        DailyRate = dailyRate;
    }

    private Room()
    {
    }

    public string RoomNumber { get; private set; } = null!;
    public string RoomType { get; private set; } = null!;
    public int DepartmentId { get; private set; }
    public decimal DailyRate { get; private set; }

    public static Result<Room> Create(string roomNumber, string roomType, int departmentId, decimal dailyRate)
    {
        return Result.Success(new Room(roomNumber.Trim(), roomType.Trim(), departmentId, dailyRate));
    }
}
