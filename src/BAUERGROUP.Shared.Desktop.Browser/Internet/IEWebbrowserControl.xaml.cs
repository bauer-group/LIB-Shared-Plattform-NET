#nullable enable

using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BAUERGROUP.Shared.Desktop.Browser.Internet;

/// <summary>
/// IE browser user control.
/// </summary>
internal partial class IEWebbrowserControl : UserControl, IDisposable
{
    private bool _isDisposed;

    /// <summary>
    /// Creates a new IE browser control.
    /// </summary>
    public IEWebbrowserControl()
    {
        InternetExplorerBrowserEmulation.SetBrowserEmulationVersion();
        InitializeComponent();
        Initialize();
    }

    /// <summary>
    /// Creates a new IE browser control and navigates to URL.
    /// </summary>
    /// <param name="url">The URL to navigate to.</param>
    public IEWebbrowserControl(string url) : this()
    {
        Navigate(url);
    }

    /// <summary>
    /// Occurs when the window should close.
    /// </summary>
    public event EventHandler? CloseWindow;

    /// <summary>
    /// Occurs when closing is requested by JavaScript.
    /// </summary>
    public event EventHandler? CloseWindowByJavascript;

    /// <summary>
    /// Occurs when closing is requested by user interface.
    /// </summary>
    public event EventHandler? CloseWindowByUserInterface;

    private void Initialize()
    {
        PreviewKeyDown += CloseOnEscape_Event;
        mainBrowser.MessageHook += mainBrowser_MessageHook;
        mainBrowser.Focus();
    }

    private IntPtr mainBrowser_MessageHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        switch (msg)
        {
            case 0x0010: // WM_CLOSE -> javascript:window.close();
                handled = true;
                CloseByJavascript();
                Close();
                break;
        }

        return IntPtr.Zero;
    }

    private void CloseByJavascript()
    {
        CloseWindowByJavascript?.Invoke(this, EventArgs.Empty);
    }

    private void Close()
    {
        CloseWindow?.Invoke(this, EventArgs.Empty);
    }

    private void CloseOnEscape_Event(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            CloseByUserInterface();
            Close();
        }
    }

    private void CloseByUserInterface()
    {
        CloseWindowByUserInterface?.Invoke(this, EventArgs.Empty);
    }

    private void urlTextBox_KeyUp(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
            Navigate(urlTextBox.Text);
    }

    /// <summary>
    /// Navigates to the specified URL.
    /// </summary>
    /// <param name="url">The URL to navigate to.</param>
    /// <returns>True if navigation was successful.</returns>
    public bool Navigate(string url)
    {
        try
        {
            mainBrowser.Navigate(url);
        }
        catch (UriFormatException ex)
        {
            mainBrowser.Navigate($"about:<h1>{WebUtility.HtmlEncode(ex.Message)}</h1>");
            return false;
        }

        return true;
    }

    private void ButtonClose_Click(object sender, RoutedEventArgs e)
    {
        CloseByUserInterface();
        Close();
    }

    private void Print()
    {
        mainBrowser.InvokeScript("execScript", new object[] { "window.print();", "JavaScript" });
    }

    private void NavigateBack_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        mainBrowser.GoBack();
    }

    private void NavigateForward_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        mainBrowser.GoForward();
    }

    private void Navigate_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        Navigate(urlTextBox.Text);
    }

    private void NavigateBack_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = mainBrowser != null && mainBrowser.CanGoBack;
    }

    private void NavigateForward_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = mainBrowser != null && mainBrowser.CanGoForward;
    }

    private void Navigate_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    private void mainBrowser_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
    {
        urlTextBox.Text = e.Uri.OriginalString;
    }

    private void Print_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        Print();
    }

    private void Print_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = !string.IsNullOrWhiteSpace(urlTextBox.Text);
    }

    private void mainBrowser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
    {
        // Site is completely loaded
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes browser resources.
    /// </summary>
    /// <param name="disposing">True if disposing managed resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
            return;

        if (disposing)
        {
            // BGSHIP-126, Crash der Anwendung seit 16.08.2016
            mainBrowser.Navigate("about:blank");
            mainBrowser.Dispose();
            mainBrowser = null!;
        }

        _isDisposed = true;
    }
}
