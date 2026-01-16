#nullable enable

namespace BAUERGROUP.Shared.Core.PeriodicExecution;

/// <summary>
/// Interface for a job scheduler that manages periodic task execution.
/// </summary>
public interface IScheduler : IDisposable
{
    /// <summary>
    /// Gets the list of registered jobs.
    /// </summary>
    IReadOnlyList<ISchedulerObject> Jobs { get; }

    /// <summary>
    /// Starts all enabled jobs.
    /// </summary>
    void Start();

    /// <summary>
    /// Starts all enabled jobs asynchronously.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task StartAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Stops all running jobs.
    /// </summary>
    void Stop();

    /// <summary>
    /// Stops all running jobs asynchronously.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task StopAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Restarts all jobs.
    /// </summary>
    void Restart();

    /// <summary>
    /// Restarts all jobs asynchronously.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task RestartAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Registers a job with the scheduler.
    /// </summary>
    /// <param name="job">The job to register.</param>
    void RegisterJob(ISchedulerObject job);

    /// <summary>
    /// Unregisters a job from the scheduler.
    /// </summary>
    /// <param name="job">The job to unregister.</param>
    void UnregisterJob(ISchedulerObject job);
}
