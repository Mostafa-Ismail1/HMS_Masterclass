using HMS.Domain.Common;

namespace HMS.Domain.Entities.Doctors;

public class DoctorSchedule
{
    private DoctorSchedule(int doctorId, int departmentId, string dayOfWeek, TimeOnly startTime, TimeOnly endTime)
    {
        DoctorId = doctorId;
        DepartmentId = departmentId;
        DayOfWeek = dayOfWeek;
        StartTime = startTime;
        EndTime = endTime;
    }

    private DoctorSchedule()
    {
    }

    public int DoctorId { get; private set; }
    public int DepartmentId { get; private set; }
    public string DayOfWeek { get; private set; } = null!;
    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }

    public static Result<DoctorSchedule> Create(
        int doctorId,
        int departmentId,
        string dayOfWeek,
        TimeOnly startTime,
        TimeOnly endTime)
    {
        if (endTime <= startTime)
        {
            return Result.Failure<DoctorSchedule>(DoctorErrors.InvalidScheduleTime);
        }

        return Result.Success(new DoctorSchedule(
            doctorId,
            departmentId,
            dayOfWeek.Trim(),
            startTime,
            endTime));
    }
}
