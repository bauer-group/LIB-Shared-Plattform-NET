using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAUERGROUP.Shared.Desktop.Dialogs
{
    public static class InputBox
    {
        public static Boolean Show(ref String sData, String sCaption = @"Eingabe")
        {
            var oDialog = new InputForm();
            oDialog.Title = sCaption;
            oDialog.Data = sData;

            var bResult = oDialog.ShowDialog().Value;
            sData = oDialog.Data;

            return bResult;
        }
    }
}
