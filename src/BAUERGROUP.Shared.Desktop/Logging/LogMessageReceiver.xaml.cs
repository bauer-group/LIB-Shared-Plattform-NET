using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BAUERGROUP.Shared.Desktop.Logging
{
    public partial class LogMessageReceiver : Window
    {
        public LogMessageReceiver()
        {
            InitializeComponent();

            Initialize();
        }

        public LogMessageReceiver(String sTitle) 
            : this()
        {
            if (sTitle == null)
                return;

            this.Title = sTitle;
        }

        private void Initialize()
        {
            this.PreviewKeyDown += CloseOnEscape_Event;
        }        

        private void CloseOnEscape_Event(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }
    }
}
