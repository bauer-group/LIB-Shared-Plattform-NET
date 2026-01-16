#nullable enable

using BAUERGROUP.Shared.Core.Application;
using BAUERGROUP.Shared.Desktop.Exceptions;
using CefSharp;
using CefSharp.DevTools.Page;
using CefSharp.OffScreen;
using System.IO;

namespace BAUERGROUP.Shared.Desktop.Browser.Internet;

/// <summary>
/// Provides offscreen Chrome browser for headless operations like screenshots.
/// </summary>
public class ChromeOffscreenWebbrowser : IDisposable
{
    private static readonly Lazy<ChromeOffscreenWebbrowser> SingletonInstance = new();

    /// <summary>
    /// Gets the singleton runtime instance.
    /// </summary>
    public static ChromeOffscreenWebbrowser Runtime => SingletonInstance.Value;

    /// <summary>
    /// Creates a new offscreen browser with default settings.
    /// </summary>
    public ChromeOffscreenWebbrowser()
        : this(null)
    {
    }

    /// <summary>
    /// Creates a new offscreen browser with custom settings.
    /// </summary>
    /// <param name="settings">The CefSettings to use.</param>
    public ChromeOffscreenWebbrowser(CefSettings? settings)
    {
        Initialize(settings);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Deinitialize();
        GC.SuppressFinalize(this);
    }

    private static void Deinitialize()
    {
        Cef.WaitForBrowsersToClose();
        Cef.Shutdown();
    }

    private static void Initialize(CefSettings? settings)
    {
        if (Cef.IsInitialized == true)
            return;

        var chromeSettings = settings ?? GetDefaultConfiguration();

        Cef.EnableWaitForBrowsersToClose();

        var result = Cef.Initialize(chromeSettings, performDependencyCheck: true, browserProcessHandler: null);

        if (!result)
            throw new ChromeException("Unable to initialize Chrome Embedded Framework.");
    }

    private static CefSettings GetDefaultConfiguration()
    {
        var chromeSettings = new CefSettings
        {
            CachePath = BrowserCachePath,
            Locale = "de",
            LogSeverity = LogSeverity.Disable,
            WindowlessRenderingEnabled = true
        };

        chromeSettings.CefCommandLineArgs.Add("enable-media-stream");
        chromeSettings.CefCommandLineArgs.Add("use-fake-ui-for-media-stream");
        chromeSettings.CefCommandLineArgs.Add("enable-usermedia-screen-capturing");
        chromeSettings.SetOffScreenRenderingBestPerformanceArgs();

        return chromeSettings;
    }

    private static string BrowserCachePath
    {
        get
        {
            var appDataFolder = ApplicationFolders.ExecutionAutomaticApplicationDataFolder;
            return Path.Combine(appDataFolder, "Cache", "Browser");
        }
    }

    private static BrowserSettings GetBrowserSettings()
    {
        return new BrowserSettings
        {
            WindowlessFrameRate = 1
        };
    }

    /// <summary>
    /// Takes a screenshot of the specified URL.
    /// </summary>
    /// <param name="url">The URL to capture.</param>
    /// <param name="filename">The output filename.</param>
    /// <param name="renderingDelay">Delay in milliseconds to allow page rendering.</param>
    public async Task MakeScreenshot(string url, string filename, int renderingDelay = 100)
    {
        using var browser = new ChromiumWebBrowser(url, GetBrowserSettings());
        var loadResponse = await browser.WaitForInitialLoadAsync();

        if (!loadResponse.Success)
            throw new ChromeException($"Page loading failed with ErrorCode: {loadResponse.ErrorCode} and HttpStatusCode: {loadResponse.HttpStatusCode}");

        await Task.Delay(renderingDelay);

        var contentSize = await browser.GetContentSizeAsync();

        var viewport = new Viewport
        {
            Height = contentSize.Height,
            Width = contentSize.Width,
            Scale = 1.0
        };

        var imageByteArray = await browser.CaptureScreenshotAsync(CaptureScreenshotFormat.Png, null, viewport);

        await File.WriteAllBytesAsync(filename, imageByteArray);
    }
}
