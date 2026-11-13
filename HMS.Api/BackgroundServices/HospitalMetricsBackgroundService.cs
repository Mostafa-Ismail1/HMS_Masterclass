using HMS.Application.Common.Interfaces;

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
            var departmentRepository = scope.ServiceProvider.GetRequiredService<IDepartmentRepository>();
            var staffRepository = scope.ServiceProvider.GetRequiredService<IStaffRepository>();

            var departments = await departmentRepository.GetAllAsync(cancellationToken);
            var staff = await staffRepository.GetAllStaffAsync(cancellationToken);

            logger.LogInformation(
                "[Hospital Metrics] Departments: {TotalDepartments} | Staff: {TotalStaff} | Time: {Timestamp:HH:mm:ss}",
                departments.Count,
                staff.Count,
                DateTime.Now);
        }
        catch (Exception ex) when (!cancellationToken.IsCancellationRequested)
        {
            logger.LogWarning(ex, "[HospitalMetricsWorker] Failed to query hospital metrics for current tick.");
        }
    }
}
