#nullable enable

using BAUERGROUP.Shared.Desktop.Browser.Internet;
using System.Windows;

namespace BAUERGROUP.Shared.Desktop.Browser;

/// <summary>
/// Browser-related WPF utility methods using CefSharp Chromium and IE embedded browsers.
/// </summary>
public static class WPFToolboxBrowser
{
    /// <summary>
    /// Shows an Internet Explorer embedded browser window.
    /// </summary>
    /// <param name="title">The window title.</param>
    /// <param name="url">The URL to navigate to.</param>
    /// <param name="owner">The owner window.</param>
    /// <param name="wait">Whether to wait for the browser to close.</param>
    /// <param name="disableCloseWindow">Whether to disable window closing.</param>
    public static void IEEmbeddedWebbrowserWindow(string title, string url, Window? owner = null, bool wait = false, bool disableCloseWindow = false)
    {
        using var browser = new IEEmbeddedWebbrowser(title, url);
        browser.Owner = owner;
        browser.CloseWindowByUserInterfaceIsDisabled = disableCloseWindow;

        if (wait)
            browser.ShowDialog();
        else
            browser.Show();
    }

    /// <summary>
    /// Shows a Chrome (CefSharp) embedded browser window.
    /// </summary>
    /// <param name="title">The window title.</param>
    /// <param name="url">The URL to navigate to.</param>
    /// <param name="owner">The owner window.</param>
    /// <param name="wait">Whether to wait for the browser to close.</param>
    /// <param name="disableCloseWindow">Whether to disable window closing.</param>
    public static void ChromeEmbeddedWebbrowserWindow(string title, string url, Window? owner = null, bool wait = false, bool disableCloseWindow = false)
    {
        using var browser = new ChromeEmbeddedWebbrowser(title, url);
        browser.Owner = owner;
        browser.CloseWindowByUserInterfaceIsDisabled = disableCloseWindow;

        if (wait)
            browser.ShowDialog();
        else
            browser.Show();
    }

    /// <summary>
    /// Makes a screenshot of a website using the offscreen Chrome browser.
    /// </summary>
    /// <param name="url">The URL to capture.</param>
    /// <param name="filename">The output filename.</param>
    public static async Task MakeWebsiteScreenshot(string url, string filename)
    {
        await ChromeOffscreenWebbrowser.Runtime.MakeScreenshot(url, filename);
    }
}
