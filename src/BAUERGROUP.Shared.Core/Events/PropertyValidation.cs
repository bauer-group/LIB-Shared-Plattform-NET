#nullable enable

using System.Text;

namespace BAUERGROUP.Shared.Core.Events;

/// <summary>
/// Base class providing validation and IsBusy property with change notification.
/// </summary>
public class PropertyValidation : PropertyChangedBase
{
    private readonly List<string> _errors;
    private bool _isBusy;

    /// <summary>
    /// Creates a new instance and runs initial validation.
    /// </summary>
    public PropertyValidation()
    {
        _errors = new List<string>();
        _isBusy = false;
        Validate();
    }

    /// <summary>
    /// Runs validation logic. Override in derived classes.
    /// </summary>
    protected virtual void Validate()
    {
        OnIsValidChanged();
        OnIsBusyChanged();
    }

    /// <summary>
    /// Validates a property and adds/removes error message accordingly.
    /// </summary>
    /// <param name="hasError">Function that returns true if validation fails.</param>
    /// <param name="errorMessage">The error message to add/remove.</param>
    protected virtual void ValidateProperty(Func<bool> hasError, string errorMessage)
    {
        if (hasError())
        {
            if (!Errors.Contains(errorMessage))
            {
                Errors.Add(errorMessage);
                OnPropertyChanged(nameof(Errors));
            }
        }
        else
        {
            Errors.Remove(errorMessage);
            OnPropertyChanged(nameof(Errors));
        }
    }

    /// <summary>
    /// Gets the list of validation errors.
    /// </summary>
    protected List<string> Errors => _errors;

    /// <summary>
    /// Gets all validation errors as a single string.
    /// </summary>
    public virtual string Error => _errors.Aggregate(new StringBuilder(), (b, s) => b.AppendLine(s)).ToString().Trim();

    /// <summary>
    /// Gets whether the object is valid (no validation errors).
    /// </summary>
    public bool IsValid => _errors.Count == 0;

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

    /// <summary>
    /// Occurs when the IsBusy property changes.
    /// </summary>
    public event EventHandler? IsBusyChanged;

    /// <summary>
    /// Occurs when the IsValid property changes.
    /// </summary>
    public event EventHandler? IsValidChanged;

    /// <summary>
    /// Raises the IsBusyChanged event.
    /// </summary>
    protected virtual void OnIsBusyChanged()
    {
        OnPropertyChanged(nameof(IsBusy));
        IsBusyChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Raises the IsValidChanged event.
    /// </summary>
    protected virtual void OnIsValidChanged()
    {
        OnPropertyChanged(nameof(IsValid));
        IsValidChanged?.Invoke(this, EventArgs.Empty);
    }
}
