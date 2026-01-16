#nullable enable

using System.ComponentModel;

namespace BAUERGROUP.Shared.Core.PeriodicExecution;

/// <summary>
/// Base class for scheduled jobs.
/// </summary>
[Serializable]
public class SchedulerObject : ISchedulerObject
{
    private bool _isCanceled;
    private readonly object _isCanceledLock = new();

    /// <summary>
    /// Creates a new scheduler object with default values.
    /// </summary>
    public SchedulerObject()
    {
        Uid = Guid.NewGuid();
        Interval = TimeSpan.FromMinutes(1);
        Enabled = true;
        LastExecution = null;
        SuccessfulExecution = false;
        LastChange = DateTime.UtcNow;
        IsCanceled = false;
    }

    /// <inheritdoc />
    [Browsable(false)]
    public Guid Uid { get; set; }

    /// <inheritdoc />
    [Browsable(false)]
    public object? Parameters { get; set; }

    /// <inheritdoc />
    [Browsable(false)]
    public virtual void Execute()
    {
        // Override in derived classes for periodic execution
    }

#if NETSTANDARD2_0
    /// <inheritdoc />
    [Browsable(false)]
    public virtual Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return Task.Run(Execute, cancellationToken);
    }
#endif

    /// <inheritdoc />
    [Category("Steuerung")]
    [DisplayName("Ausführungsintervall")]
    public TimeSpan Interval { get; set; }

    /// <inheritdoc />
    [Category("Steuerung")]
    [DisplayName("Aktiviert")]
    public bool Enabled { get; set; }

    /// <inheritdoc />
    [Category("Informationen")]
    [DisplayName("Letzte Ausführung")]
    [Browsable(false)]
    public DateTime? LastExecution { get; set; }

    /// <inheritdoc />
    [Category("Informationen")]
    [DisplayName("Erfolgreiche Ausführung")]
    [Browsable(false)]
    public bool SuccessfulExecution { get; set; }

    /// <inheritdoc />
    [Category("Informationen")]
    [DisplayName("Letzte Konfigurationsänderung")]
    [Browsable(false)]
    public DateTime? LastChange { get; set; }

    /// <inheritdoc />
    [Browsable(false)]
    public bool IsCanceled
    {
        get { lock (_isCanceledLock) { return _isCanceled; } }
        set { lock (_isCanceledLock) { _isCanceled = value; } }
    }

    /// <inheritdoc />
    [Browsable(false)]
    public virtual void Start()
    {
        // Override in derived classes for initialization
    }

    /// <inheritdoc />
    [Browsable(false)]
    public virtual void Stop()
    {
        // Override in derived classes for cleanup
    }
}
