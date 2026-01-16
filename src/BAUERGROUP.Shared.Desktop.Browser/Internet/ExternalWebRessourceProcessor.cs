#nullable enable

using BAUERGROUP.Shared.Core.Extensions;
using System.Windows;

namespace BAUERGROUP.Shared.Desktop.Browser.Internet;

/// <summary>
/// Processes external web resources by opening them in a browser.
/// </summary>
public class ExternalWebRessourceProcessor
{
    /// <summary>
    /// Gets the browser window title.
    /// </summary>
    protected string BrowserTitle { get; }

    /// <summary>
    /// Gets the owner window.
    /// </summary>
    protected Window? Owner { get; }

    /// <summary>
    /// Gets whether to wait for the browser to close.
    /// </summary>
    protected bool Wait { get; }

    /// <summary>
    /// Gets whether closing the window is disabled.
    /// </summary>
    protected bool DisableCloseWindow { get; }

    /// <summary>
    /// Creates a new processor with the specified parameters.
    /// </summary>
    /// <param name="title">The browser window title.</param>
    /// <param name="owner">The owner window.</param>
    /// <param name="wait">Whether to wait for the browser to close.</param>
    /// <param name="disableCloseWindow">Whether to disable window closing.</param>
    public ExternalWebRessourceProcessor(string? title = null, Window? owner = null, bool wait = false, bool disableCloseWindow = false)
    {
        BrowserTitle = title ?? owner?.Title ?? "BAUER GROUP - Internet Browser";
        Owner = owner;
        Wait = wait;
        DisableCloseWindow = disableCloseWindow;
    }

    /// <summary>
    /// Creates a new processor with the specified owner window.
    /// </summary>
    /// <param name="owner">The owner window.</param>
    /// <param name="wait">Whether to wait for the browser to close.</param>
    /// <param name="disableCloseWindow">Whether to disable window closing.</param>
    public ExternalWebRessourceProcessor(Window owner, bool wait = false, bool disableCloseWindow = false)
        : this(null, owner, wait, disableCloseWindow)
    {
    }

    /// <summary>
    /// Processes a collection of web resource jobs.
    /// </summary>
    /// <typeparam name="T">The type of data associated with the jobs.</typeparam>
    /// <param name="jobs">The jobs to process.</param>
    /// <param name="mode">The processing mode filter.</param>
    public void Process<T>(IEnumerable<ExternalWebRessourceJob<T>> jobs, ExternalWebRessourceMode mode = ExternalWebRessourceMode.Unspecified) where T : class
    {
        foreach (var job in jobs)
        {
            if (mode == job.Mode || mode == ExternalWebRessourceMode.Unspecified)
                ProcessWebResourceJobEntry(job);
        }
    }

    /// <summary>
    /// Processes a single web resource job.
    /// </summary>
    protected void ProcessWebResourceJobEntry<T>(ExternalWebRessourceJob<T> job) where T : class
    {
        if (string.IsNullOrWhiteSpace(job.URL))
            return;

        if (job.Data == null)
        {
            ShowBrowser(job.URL);
            return;
        }

        var url = job.Data.SetURLParameters(job.URL);
        ShowBrowser(url);
    }

    /// <summary>
    /// Shows the browser with the specified URL.
    /// </summary>
    protected void ShowBrowser(string url)
    {
        WPFToolboxBrowser.ChromeEmbeddedWebbrowserWindow(BrowserTitle, url, Owner, Wait, DisableCloseWindow);
    }
}
