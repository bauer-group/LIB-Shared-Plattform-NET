using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace BAUERGROUP.Shared.Desktop.Processes
{
    public static class ProcessControl
    {
        //Hint: http://msdn.microsoft.com/en-us/library/system.diagnostics.processstartinfo.verb.aspx

        public static bool PrintDocument(string fileName, ProcessWindowStyle windowStyle = ProcessWindowStyle.Hidden)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.WindowStyle = windowStyle;
            psi.Verb = @"Print";
            psi.FileName = fileName;

            Process p = new Process();
            p.StartInfo = psi;

            return p.Start();
        }
    }
}
