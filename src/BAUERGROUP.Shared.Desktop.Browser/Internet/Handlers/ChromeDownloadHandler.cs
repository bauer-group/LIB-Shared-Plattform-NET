#nullable enable

using CefSharp;

namespace BAUERGROUP.Shared.Desktop.Browser.Internet.Handlers;

/// <summary>
/// Handles file downloads in Chrome browser.
/// </summary>
internal class ChromeDownloadHandler : IDownloadHandler
{
    /// <summary>
    /// Occurs before a download starts.
    /// </summary>
    public event EventHandler<DownloadItem>? OnBeforeDownloadFired;

    /// <summary>
    /// Occurs when download progress is updated.
    /// </summary>
    public event EventHandler<DownloadItem>? OnDownloadUpdatedFired;

    /// <inheritdoc />
    public bool CanDownload(IWebBrowser chromiumWebBrowser, IBrowser browser, string url, string requestMethod)
    {
        return true;
    }

    /// <inheritdoc />
    public bool OnBeforeDownload(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IBeforeDownloadCallback callback)
    {
        OnBeforeDownloadFired?.Invoke(this, downloadItem);

        if (!callback.IsDisposed)
        {
            using (callback)
            {
                callback.Continue(downloadItem.SuggestedFileName, showDialog: true);
            }
        }

        return true;
    }

    /// <inheritdoc />
    public void OnDownloadUpdated(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback)
    {
        OnDownloadUpdatedFired?.Invoke(this, downloadItem);
    }
}
