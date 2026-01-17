#nullable enable

using BAUERGROUP.Shared.Core.Application;
using BAUERGROUP.Shared.Desktop.Exceptions;
using BAUERGROUP.Shared.Desktop.Browser.Internet.Handlers;
using CefSharp;
using CefSharp.Wpf;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BAUERGROUP.Shared.Desktop.Browser.Internet;

/// <summary>
/// Chrome browser user control.
/// </summary>
internal partial class ChromeWebbrowserControl : UserControl
{
    private bool _isDisposed;

    /// <summary>
    /// Creates a new Chrome browser control.
    /// </summary>
    public ChromeWebbrowserControl()
    {
        EarlyBrowserInitialization();
        InitializeComponent();
        Initialize();
    }

    /// <summary>
    /// Creates a new Chrome browser control and navigates to URL.
    /// </summary>
    /// <param name="url">The URL to navigate to.</param>
    public ChromeWebbrowserControl(string url) : this()
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

    /// <summary>
    /// Occurs when a popup is requested.
    /// </summary>
    public event Action<string>? PopupRequest;

    private static string BrowserCachePath
    {
        get
        {
            var appDataFolder = ApplicationFolders.ExecutionAutomaticApplicationDataFolder;
            return System.IO.Path.Combine(appDataFolder, "Cache", "Browser");
        }
    }

    private static void EarlyBrowserInitialization()
    {
        if (Cef.IsInitialized == true)
            return;

        var chromeSettings = new CefSettings
        {
            CachePath = BrowserCachePath,
            Locale = "de",
            LogSeverity = LogSeverity.Disable
        };

        chromeSettings.CefCommandLineArgs.Add("enable-media-stream");
        chromeSettings.CefCommandLineArgs.Add("use-fake-ui-for-media-stream");
        chromeSettings.CefCommandLineArgs.Add("enable-usermedia-screen-capturing");

        var result = Cef.Initialize(chromeSettings, performDependencyCheck: true, browserProcessHandler: null);

        if (!result)
            throw new ChromeException("Unable to initialize Chrome Embedded Framework.");
    }

    private void Initialize()
    {
        PreviewKeyDown += CloseOnEscape_Event;
        mainBrowser.LifeSpanHandler = new ChromeLifeSpanHandler(this);
        mainBrowser.DisplayHandler = new ChromeDisplayHandler();
        mainBrowser.MenuHandler = new ChromeMenuHandler();
        mainBrowser.DownloadHandler = new ChromeDownloadHandler();
        mainBrowser.Focus();
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
            mainBrowser.Load(url);
        }
        catch (UriFormatException ex)
        {
            mainBrowser.Load($"about:<h1>{WebUtility.HtmlEncode(ex.Message)}</h1>");
            return false;
        }

        return true;
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
            return;
        }

        if (e.Key == Key.F11)
        {
            mainBrowser.EvaluateScriptAsync("document.documentElement.requestFullscreen();");
        }
    }

    private void CloseByUserInterface()
    {
        CloseWindowByUserInterface?.Invoke(this, EventArgs.Empty);
    }

    private void ButtonClose_Click(object sender, RoutedEventArgs e)
    {
        CloseByUserInterface();
        Close();
    }

    /// <summary>
    /// Disposes browser resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
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
            mainBrowser.Load("about:blank");
            mainBrowser.Dispose();
            mainBrowser = null!;
        }

        _isDisposed = true;
    }

    private class ChromeLifeSpanHandler : ILifeSpanHandler
    {
        private ChromeWebbrowserControl BrowserWindow { get; }

        internal ChromeLifeSpanHandler(ChromeWebbrowserControl browserWindow)
        {
            BrowserWindow = browserWindow;
        }

        public bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            BrowserWindow.Dispatcher.Invoke(() =>
            {
                BrowserWindow.CloseByJavascript();
            });

            return true;
        }

        public void OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
        }

        public void OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
        }

        public bool OnBeforePopup(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser? newBrowser)
        {
            BrowserWindow.Dispatcher.BeginInvoke(() =>
            {
                BrowserWindow.PopupRequest?.Invoke(targetUrl);
            });

            newBrowser = null;
            return true;
        }
    }

    private void urlTextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        if (e.OriginalSource is TextBox textBox)
            textBox.SelectAll();
        else if (e.OriginalSource is PasswordBox passwordBox)
            passwordBox.SelectAll();
    }

    private void urlTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is not FrameworkElement element || element.IsKeyboardFocusWithin)
            return;

        e.Handled = true;
        element.Focus();
    }
}
