#nullable enable

using System.Windows;
using System.Windows.Input;

namespace BAUERGROUP.Shared.Desktop.Browser.Internet;

/// <summary>
/// Embedded IE browser window.
/// </summary>
internal partial class IEEmbeddedWebbrowser : Window, IDisposable
{
    private bool _isDisposed;

    /// <summary>
    /// Creates a new IE embedded browser window.
    /// </summary>
    public IEEmbeddedWebbrowser()
    {
        InitializeComponent();
        Initialize();
    }

    /// <summary>
    /// Creates a new IE embedded browser window with title and URL.
    /// </summary>
    /// <param name="title">The window title.</param>
    /// <param name="url">The URL to navigate to.</param>
    public IEEmbeddedWebbrowser(string title, string url) : this(title)
    {
        ucWebrowser.Navigate(url);
    }

    /// <summary>
    /// Creates a new IE embedded browser window with title.
    /// </summary>
    /// <param name="title">The window title.</param>
    public IEEmbeddedWebbrowser(string title) : this()
    {
        Title = title;
    }

    private void Initialize()
    {
        PreviewKeyDown += CloseOnEscape_Event;
        ucWebrowser.CloseWindowByJavascript += ucWebrowser_CloseWindowByJavascript;
        ucWebrowser.CloseWindowByUserInterface += ucWebrowser_CloseWindowByUserInterface;
        CloseWindowByUserInterfaceIsDisabled = false;
    }

    private void ucWebrowser_CloseWindowByUserInterface(object? sender, EventArgs e)
    {
        Close();
    }

    private void ucWebrowser_CloseWindowByJavascript(object? sender, EventArgs e)
    {
        CloseWindowByUserInterfaceIsDisabled = false;
        Close();
    }

    private void SetupWindow()
    {
#if !DEBUG
        WindowState = WindowState.Maximized;
#endif
        ucWebrowser.Focus();
    }

    private void CloseOnEscape_Event(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
            Close();
    }

    /// <summary>
    /// Gets or sets whether closing by user interface is disabled.
    /// </summary>
    public bool CloseWindowByUserInterfaceIsDisabled { get; set; }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        SetupWindow();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the browser resources.
    /// </summary>
    /// <param name="disposing">True if disposing managed resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
            return;

        if (disposing)
        {
            ucWebrowser.Dispose();
        }

        _isDisposed = true;
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        if (CloseWindowByUserInterfaceIsDisabled)
        {
            e.Cancel = true;
            MessageBox.Show(
                "Das Fenster darf nicht manuell geschlossen werden! Diese Funktion wurde deaktiviert.",
                "Benutzerhinweis",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}
