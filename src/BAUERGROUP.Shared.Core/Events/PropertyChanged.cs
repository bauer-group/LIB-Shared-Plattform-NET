#nullable enable

namespace BAUERGROUP.Shared.Core.Events;

/// <summary>
/// Base class providing IsBusy property with change notification.
/// </summary>
public class PropertyChanged : PropertyChangedBase
{
    private bool _isBusy;

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public PropertyChanged()
    {
        _isBusy = false;
        OnIsBusyChanged();
    }

    /// <summary>
    /// Occurs when the IsBusy property changes.
    /// </summary>
    public event EventHandler? IsBusyChanged;

    /// <summary>
    /// Raises the IsBusyChanged event.
    /// </summary>
    protected virtual void OnIsBusyChanged()
    {
        OnPropertyChanged(nameof(IsBusy));
        IsBusyChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Gets or sets whether the object is busy.
    /// </summary>
    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            if (_isBusy != value)
            {
                _isBusy = value;
                OnIsBusyChanged();
            }
        }
    }
}
