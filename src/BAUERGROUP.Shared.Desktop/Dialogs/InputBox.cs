using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAUERGROUP.Shared.Desktop.Dialogs
{
    /// <summary>
    /// Provides a simple input dialog for collecting user text input.
    /// </summary>
    public static class InputBox
    {
        /// <summary>
        /// Shows an input dialog and returns the user's input.
        /// </summary>
        /// <param name="data">The initial value and output for the user's input.</param>
        /// <param name="caption">The dialog title. Defaults to "Eingabe".</param>
        /// <returns>True if the user confirmed the dialog; false if cancelled.</returns>
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
