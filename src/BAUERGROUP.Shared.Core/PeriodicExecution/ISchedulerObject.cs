#nullable enable

namespace BAUERGROUP.Shared.Core.PeriodicExecution;

/// <summary>
/// Represents a scheduled job that can be executed periodically.
/// </summary>
public interface ISchedulerObject
{
    /// <summary>
    /// Gets or sets the unique identifier for this job.
    /// </summary>
    Guid Uid { get; set; }

    /// <summary>
    /// Gets or sets optional parameters for the job.
    /// </summary>
    object? Parameters { get; set; }

    /// <summary>
    /// Executes the job synchronously.
    /// </summary>
    void Execute();

    /// <summary>
    /// Executes the job asynchronously.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
#if NETSTANDARD2_0
    Task ExecuteAsync(CancellationToken cancellationToken = default);
#else
    Task ExecuteAsync(CancellationToken cancellationToken = default) => Task.Run(Execute, cancellationToken);
#endif

    /// <summary>
    /// Gets or sets the interval between executions.
    /// </summary>
    TimeSpan Interval { get; set; }

    /// <summary>
    /// Gets or sets whether this job is enabled.
    /// </summary>
    bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets the last execution time.
    /// </summary>
    DateTime? LastExecution { get; set; }

    /// <summary>
    /// Gets or sets whether the last execution was successful.
    /// </summary>
    bool SuccessfulExecution { get; set; }

    /// <summary>
    /// Gets or sets the last change time.
    /// </summary>
    DateTime? LastChange { get; set; }

    /// <summary>
    /// Gets or sets whether this job is canceled.
    /// </summary>
    bool IsCanceled { get; set; }

    /// <summary>
    /// Starts the job.
    /// </summary>
    void Start();

    /// <summary>
    /// Stops the job.
    /// </summary>
    void Stop();
}
