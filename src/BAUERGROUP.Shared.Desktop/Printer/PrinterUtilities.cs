using System;
using System.Linq;
using System.Printing;
using System.Windows.Controls;
using WpfPrintDialog = System.Windows.Controls.PrintDialog;

namespace BAUERGROUP.Shared.Desktop.Printer
{
    public static class PrinterUtilities
    {
        public static Boolean SelectPrinter(out String sName)
        {
            var pDialog = new WpfPrintDialog();
            pDialog.PageRangeSelection = PageRangeSelection.AllPages;
            pDialog.UserPageRangeEnabled = false;

            var pdResult = pDialog.ShowDialog();

            if (pdResult == true)
            {
                sName = pDialog.PrintQueue.Name;
                return true;
            }

            sName = null;
            return false;
        }

        public static String[] EnumeratePrinters()
        {
            var printQueues = new LocalPrintServer().GetPrintQueues(new[] { EnumeratedPrintQueueTypes.Local, EnumeratedPrintQueueTypes.Connections });

            return printQueues.Select(p => p.Name).ToArray();
        }
    }
}
