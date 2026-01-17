using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAUERGROUP.Shared.Desktop.Dialogs
{
    public static class InputBox
    {
        public static Boolean Show(ref String data, String caption = @"Eingabe")
        {
            var dialog = new InputForm();
            dialog.Title = caption;
            dialog.Data = data;

            var result = dialog.ShowDialog() ?? false;
            data = dialog.Data;

            return result;
        }
    }
}
