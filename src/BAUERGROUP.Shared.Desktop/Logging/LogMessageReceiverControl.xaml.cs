using BAUERGROUP.Shared.Core.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfUserControl = System.Windows.Controls.UserControl;

namespace BAUERGROUP.Shared.Desktop.Logging
{
    public partial class LogMessageReceiverControl : WpfUserControl
    {
        private BGLoggerNetworkListener? LogReceiver { get; set; }

        public LogMessageReceiverControl()
        {
            InitializeComponent();

            //Initalize();
        }

        private void Initalize()
        {
            LogReceiver = new BGLoggerNetworkListener(true);
            LogReceiver.PropertyChanged += OnLogMessageReceived;
        }

        private void StartLogging()
        {
            LogReceiver = new BGLoggerNetworkListener(true);
            LogReceiver.PropertyChanged += OnLogMessageReceived;
        }

        private void StopLogging()
        {
            LogReceiver?.Dispose();
            LogReceiver = null;
        }

        private void OnLogMessageReceived(Object? sender, PropertyChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                loggingTextBlock.Text += e.PropertyName;

                try
                {
                    if (loggingTextBlock.LineCount > 1000)
                        loggingTextBlock.Clear();

                    loggingTextBlock.ScrollToEnd();
                }
                catch (InvalidOperationException ex)
                {
                    BGLogger.Error(ex);
                }
            }));
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var visible = (bool)e.NewValue;

            if (visible)
                StartLogging();
            else
                StopLogging();
        }
    }
}
