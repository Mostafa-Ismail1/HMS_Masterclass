using HMS.Api.Data.Repositories;

namespace HMS.Api.BackgroundServices;

public class HospitalMetricsBackgroundService(
    IServiceScopeFactory serviceScopeFactory,
    ILogger<HospitalMetricsBackgroundService> logger) : BackgroundService
{
    private static readonly TimeSpan MonitoringInterval = TimeSpan.FromSeconds(30);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("[HospitalMetricsWorker] Started. Monitoring interval: {IntervalSeconds}s.",
            MonitoringInterval.TotalSeconds);

        using var timer = new PeriodicTimer(MonitoringInterval);

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await CollectMetricsAsync(stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("[HospitalMetricsWorker] Cancellation requested. Shutting down gracefully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "[HospitalMetricsWorker] Fatal error encountered.");
        }
    }

    private async Task CollectMetricsAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = serviceScopeFactory.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var departments = await unitOfWork.Departments.GetAllWithStaffAsync(cancellationToken);
            var departmentList = departments.ToList();

            var totalDepartments = departmentList.Count;
            var totalStaff = departmentList.Sum(d => d.Staff.Count);

            logger.LogInformation(
                "[Hospital Metrics] Departments: {TotalDepartments} | Staff: {TotalStaff} | Time: {Timestamp:HH:mm:ss}",
                totalDepartments,
                totalStaff,
                DateTime.Now);
        }
        catch (Exception ex) when (!cancellationToken.IsCancellationRequested)
        {
            logger.LogWarning(ex, "[HospitalMetricsWorker] Failed to query hospital metrics for current tick.");
        }
    }
}
