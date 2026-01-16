#nullable enable

using CefSharp;
using CefSharp.Handler;
using CefSharp.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BAUERGROUP.Shared.Desktop.Browser.Internet.Handlers;

/// <summary>
/// Handles fullscreen mode changes for Chrome browser.
/// </summary>
internal class ChromeDisplayHandler : DisplayHandler
{
    private Grid? _containerGrid;
    private Grid? _browserGrid;
    private Window? _fullScreenWindow;

    /// <inheritdoc />
    protected override void OnFullscreenModeChange(IWebBrowser chromiumWebBrowser, IBrowser browser, bool fullscreen)
    {
        var webBrowser = (ChromiumWebBrowser)chromiumWebBrowser;

        webBrowser.Dispatcher.BeginInvoke(() =>
        {
            if (fullscreen)
            {
                var parentBorder = (Border)VisualTreeHelper.GetParent(webBrowser);
                _browserGrid = (Grid)VisualTreeHelper.GetParent(parentBorder);
                _containerGrid = (Grid)VisualTreeHelper.GetParent(_browserGrid);

                _containerGrid.Children.Remove(_browserGrid);

                _fullScreenWindow = new Window
                {
                    WindowStyle = WindowStyle.None,
                    WindowState = WindowState.Maximized,
                    Content = _browserGrid
                };

                _fullScreenWindow.PreviewKeyDown += FullScreenWindow_PreviewKeyDown;
                _fullScreenWindow.ShowDialog();
            }
            else
            {
                if (_fullScreenWindow != null)
                {
                    _fullScreenWindow.Content = null;
                    _containerGrid?.Children.Add(_browserGrid!);
                    _fullScreenWindow.Close();
                    _fullScreenWindow = null;
                }
                _containerGrid = null;
            }
        });
    }

    private void FullScreenWindow_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape || e.Key == Key.F11)
        {
            _fullScreenWindow?.Close();
        }
    }
}
