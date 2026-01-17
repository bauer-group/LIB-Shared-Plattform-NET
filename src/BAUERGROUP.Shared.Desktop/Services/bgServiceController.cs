using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;
using System.Threading.Tasks;

namespace BAUERGROUP.Shared.Desktop.Services
{
    public class bgServiceController : ServiceController, INotifyPropertyChanged, IDisposable
    {
        #region Internal
        private CancellationTokenSource _cts;

        public bgServiceController(string name) :
            base(name)
        {
            if (!IsInstalled(name))
                throw new InvalidOperationException(String.Format("Service {0} is not installed.", name));

            _cts = new CancellationTokenSource();

            InitalizeStatusMonitor();
            OnPropertyChanged("ServiceStatus");
        }

        private void InitalizeStatusMonitor()
        {
            Task.Factory.StartNew(() =>
            {
                ServiceControllerStatus _previousStatus = ServiceControllerStatus.Stopped;

                //Check for status change once a second; notify if status changes
                while (!_cts.IsCancellationRequested)
                {
                    Refresh();

                    if (_previousStatus != Status)
                    {
                        _previousStatus = Status;
                        OnPropertyChanged("ServiceStatus");
                    }

                    Thread.Sleep(1000);
                }
            }, _cts.Token);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _cts.Cancel();
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler == null)
                return;

            handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public static bool IsInstalled(string name)
        {
            return ServiceController.GetServices().FirstOrDefault(p => p.ServiceName == name) != null;
        }
    }
}
//Hint: http://msdn.microsoft.com/de-de/library/system.serviceprocess.servicecontroller(v=vs.110).aspx
