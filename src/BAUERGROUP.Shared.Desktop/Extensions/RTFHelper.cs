using BAUERGROUP.Shared.Core.Logging;
using System;
using System.Windows.Forms;

namespace BAUERGROUP.Shared.Desktop.Extensions
{
    public static class RTFHelper
    {
        public static String ToPlainText(this string value)
        {
            try
            {
                RichTextBox rtBox = new RichTextBox();                
                rtBox.Rtf = value;
                return rtBox.Text;
            }
            catch (ArgumentException ex)
            {
                //BGLOGCON-18 Fix
                BGLogger.Warn(String.Format("RTFHelper() -> ToPlainText() => ERROR: '{0}'", ex.Message), ex);
                return value;
            }
        }
    }
}
