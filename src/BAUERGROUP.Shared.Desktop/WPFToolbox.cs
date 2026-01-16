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
        public static Boolean ShowInputBox(ref String sData, String sCaption = @"Eingabe")
        {
            return InputBox.Show(ref sData, sCaption);
        }

        public static void LogMessageReceiverWindow(String sTitle = null, Window oOwner = null, Boolean bWait = false)
        {
            var wLogMessageWindow = new LogMessageReceiver(sTitle);
            wLogMessageWindow.Owner = oOwner;

            if (bWait)
                wLogMessageWindow.ShowDialog();
            else
                wLogMessageWindow.Show();
        }
    }
}
