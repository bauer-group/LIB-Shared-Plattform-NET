#nullable enable

using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Security;

namespace BAUERGROUP.Shared.Desktop.Browser.Internet;

/// <summary>
/// Manages Internet Explorer browser emulation version in the registry.
/// </summary>
internal static class InternetExplorerBrowserEmulation
{
    private const string InternetExplorerRootKey = @"Software\Microsoft\Internet Explorer";
    private const string BrowserEmulationKey = InternetExplorerRootKey + @"\Main\FeatureControl\FEATURE_BROWSER_EMULATION";

    /// <summary>
    /// Gets the browser emulation version for the application.
    /// </summary>
    /// <returns>The browser emulation version for the application.</returns>
    public static InternetExplorerBrowserEmulationVersion GetBrowserEmulationVersion()
    {
        try
        {
            var key = Registry.CurrentUser.OpenSubKey(BrowserEmulationKey, true)
                   ?? Registry.CurrentUser.CreateSubKey(BrowserEmulationKey);

            if (key != null)
            {
                var appName = Path.GetFileName(Process.GetCurrentProcess().MainModule?.FileName ?? string.Empty);
                var regValue = key.GetValue(appName, null);

                if (regValue != null)
                {
                    return (InternetExplorerBrowserEmulationVersion)Convert.ToInt32(regValue);
                }
            }
        }
        catch (SecurityException)
        {
            // The user does not have the permissions required to read from the registry key.
        }
        catch (UnauthorizedAccessException)
        {
            // The user does not have the necessary registry rights.
        }

        return InternetExplorerBrowserEmulationVersion.Default;
    }

    /// <summary>
    /// Gets the major Internet Explorer version.
    /// </summary>
    /// <returns>The major digit of the Internet Explorer version.</returns>
    public static int GetInternetExplorerMajorVersion()
    {
        try
        {
            var key = Registry.LocalMachine.OpenSubKey(InternetExplorerRootKey);

            if (key != null)
            {
                var regValue = key.GetValue("svcVersion", null) ?? key.GetValue("Version", null);

                if (regValue != null)
                {
                    var version = regValue.ToString() ?? string.Empty;
                    var separator = version.IndexOf('.');
                    if (separator != -1 && int.TryParse(version[..separator], out var versionNumber))
                    {
                        return versionNumber;
                    }
                }
            }
        }
        catch (SecurityException)
        {
            // The user does not have the permissions required to read from the registry key.
        }
        catch (UnauthorizedAccessException)
        {
            // The user does not have the necessary registry rights.
        }

        return 0;
    }

    /// <summary>
    /// Determines whether a browser emulation version is set for the application.
    /// </summary>
    /// <returns><c>true</c> if a specific browser emulation version has been set for the application; otherwise, <c>false</c>.</returns>
    public static bool IsBrowserEmulationSet()
    {
        return GetBrowserEmulationVersion() != InternetExplorerBrowserEmulationVersion.Default;
    }

    /// <summary>
    /// Sets the browser emulation version for the application.
    /// </summary>
    /// <param name="browserEmulationVersion">The browser emulation version.</param>
    /// <returns><c>true</c> the browser emulation version was updated, <c>false</c> otherwise.</returns>
    public static bool SetBrowserEmulationVersion(InternetExplorerBrowserEmulationVersion browserEmulationVersion)
    {
        try
        {
            var key = Registry.CurrentUser.OpenSubKey(BrowserEmulationKey, true)
                   ?? Registry.CurrentUser.CreateSubKey(BrowserEmulationKey);

            if (key != null)
            {
                var appName = Path.GetFileName(Process.GetCurrentProcess().MainModule?.FileName ?? string.Empty);

                if (browserEmulationVersion != InternetExplorerBrowserEmulationVersion.Default)
                {
                    key.SetValue(appName, (int)browserEmulationVersion, RegistryValueKind.DWord);
                }
                else
                {
                    key.DeleteValue(appName, false);
                }

                return true;
            }
        }
        catch (SecurityException)
        {
            // The user does not have the permissions required to read from the registry key.
        }
        catch (UnauthorizedAccessException)
        {
            // The user does not have the necessary registry rights.
        }

        return false;
    }

    /// <summary>
    /// Sets the browser emulation version for the application to the highest default mode for the version of Internet Explorer installed on the system.
    /// </summary>
    /// <returns><c>true</c> the browser emulation version was updated, <c>false</c> otherwise.</returns>
    public static bool SetBrowserEmulationVersion()
    {
        var ieVersion = GetInternetExplorerMajorVersion();

        var emulationVersion = ieVersion switch
        {
            7 => InternetExplorerBrowserEmulationVersion.Version7,
            8 => InternetExplorerBrowserEmulationVersion.Version8,
            9 => InternetExplorerBrowserEmulationVersion.Version9,
            10 => InternetExplorerBrowserEmulationVersion.Version10,
            _ => InternetExplorerBrowserEmulationVersion.Version11
        };

        return SetBrowserEmulationVersion(emulationVersion);
    }
}
