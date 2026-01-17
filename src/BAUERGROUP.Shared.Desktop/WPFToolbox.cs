using BAUERGROUP.Shared.Desktop.Dialogs;
using BAUERGROUP.Shared.Desktop.Logging;
using System;
using System.Windows;

namespace BAUERGROUP.Shared.Desktop
{
    /// <summary>
    /// WPF utility methods for common dialog and window operations.
    /// For browser-related methods, see WPFToolboxBrowser in BAUERGROUP.Shared.Desktop.Browser.
    /// </summary>
    public static class WPFToolbox
    {
        public static Boolean ShowInputBox(ref String data, String caption = @"Eingabe")
        {
            return InputBox.Show(ref data, caption);
        }

        public static void LogMessageReceiverWindow(String? title = null, Window? owner = null, Boolean wait = false)
        {
            var logMessageWindow = new LogMessageReceiver(title);
            logMessageWindow.Owner = owner;

            if (wait)
                logMessageWindow.ShowDialog();
            else
                logMessageWindow.Show();
        }
    }
}
