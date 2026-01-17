using System;
using System.Linq;
using System.Printing;
using System.Windows.Controls;
using WpfPrintDialog = System.Windows.Controls.PrintDialog;

namespace BAUERGROUP.Shared.Desktop.Printer
{
    public static class PrinterUtilities
    {
        public static Boolean SelectPrinter(out String? name)
        {
            var printDialog = new WpfPrintDialog();
            printDialog.PageRangeSelection = PageRangeSelection.AllPages;
            printDialog.UserPageRangeEnabled = false;

            var dialogResult = printDialog.ShowDialog();

            if (dialogResult == true)
            {
                name = printDialog.PrintQueue.Name;
                return true;
            }

            name = null;
            return false;
        }

        public static String[] EnumeratePrinters()
        {
            var printQueues = new LocalPrintServer().GetPrintQueues(new[] { EnumeratedPrintQueueTypes.Local, EnumeratedPrintQueueTypes.Connections });

            return printQueues.Select(p => p.Name).ToArray();
        }
    }
}
