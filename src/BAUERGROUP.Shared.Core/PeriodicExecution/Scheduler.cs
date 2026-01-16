#nullable enable

using BAUERGROUP.Shared.Core.Logging;

namespace BAUERGROUP.Shared.Core.PeriodicExecution;

/// <summary>
/// A scheduler that executes jobs at specified intervals using async/await patterns.
/// </summary>
public class Scheduler : IScheduler
{
    private readonly List<ISchedulerObject> _jobs = [];
    private readonly List<Task> _runningTasks = [];
    private CancellationTokenSource _cts = new();
    private readonly object _lock = new();
    private bool _disposed;

    /// <inheritdoc />
    public IReadOnlyList<ISchedulerObject> Jobs => _jobs.AsReadOnly();

    /// <inheritdoc />
    public void Start()
    {
        StartAsync().GetAwaiter().GetResult();
    }

    /// <inheritdoc />
    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        await StopAsync(cancellationToken);

        _cts = new CancellationTokenSource();
        var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, cancellationToken);

        lock (_lock)
        {
            foreach (var job in _jobs.Where(j => j.Enabled))
            {
                var task = RunJobAsync(job, linkedCts.Token);
                _runningTasks.Add(task);
            }
        }
    }

    private async Task RunJobAsync(ISchedulerObject job, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                job.LastExecution = DateTime.UtcNow;
                await job.ExecuteAsync(cancellationToken);
                job.SuccessfulExecution = true;
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                job.SuccessfulExecution = false;
                BGLogger.Error(ex, $"Job {job.Uid} failed");
            }

            try
            {
                await Task.Delay(job.Interval, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }

        lock (_lock)
        {
            _runningTasks.Remove(Task.CurrentId != null
                ? _runningTasks.FirstOrDefault(t => t.Id == Task.CurrentId) ?? Task.CompletedTask
                : Task.CompletedTask);
        }
    }

    /// <inheritdoc />
    public void Stop()
    {
        StopAsync().GetAwaiter().GetResult();
    }

    /// <inheritdoc />
    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        _cts.Cancel();

        Task[] tasksToWait;
        lock (_lock)
        {
            tasksToWait = [.. _runningTasks];
        }

        if (tasksToWait.Length > 0)
        {
            try
            {
                await Task.WhenAll(tasksToWait).WaitAsync(TimeSpan.FromSeconds(30), cancellationToken);
            }
            catch (TimeoutException)
            {
                BGLogger.Warn("Scheduler stop timed out waiting for jobs to complete");
            }
            catch (OperationCanceledException)
            {
                // Expected when cancellation is requested
            }
        }

        lock (_lock)
        {
            _runningTasks.Clear();
        }
    }

    /// <inheritdoc />
    public void Restart()
    {
        RestartAsync().GetAwaiter().GetResult();
    }

    /// <inheritdoc />
    public async Task RestartAsync(CancellationToken cancellationToken = default)
    {
        await StopAsync(cancellationToken);
        await StartAsync(cancellationToken);
    }

    /// <inheritdoc />
    public void RegisterJob(ISchedulerObject job)
    {
        ArgumentNullException.ThrowIfNull(job);

        lock (_lock)
        {
            if (!_jobs.Contains(job))
            {
                _jobs.Add(job);
            }
        }
    }

    /// <inheritdoc />
    public void UnregisterJob(ISchedulerObject job)
    {
        ArgumentNullException.ThrowIfNull(job);

        lock (_lock)
        {
            _jobs.Remove(job);
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes managed resources.
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            Stop();
            _cts.Dispose();
        }

        _disposed = true;
    }
}
