using BAUERGROUP.Shared.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BAUERGROUP.Shared.Core.Logging
{
    public class BGLoggerNetworkListener : PropertyChangedBase, IDisposable
    {
        private CancellationTokenSource _cts;

        public BGLoggerNetworkListener(Boolean bAutomaticEnableSenderOnLocalHost = false, IPAddress oAddress = null, UInt16 uPort = 0)
        {
            _cts = new CancellationTokenSource();

            Task.Factory.StartNew(() =>
            {
                try
                {
                    UdpClient client = new UdpClient(uPort == 0 ? BGLoggerConfiguration.DefaultNetworkPort : uPort);
                    IPEndPoint endpoint = new IPEndPoint(oAddress == null ? IPAddress.Loopback : oAddress, uPort == 0 ? BGLoggerConfiguration.DefaultNLogViewerPort : uPort);

                    while (!_cts.IsCancellationRequested)
                    {
                        byte[] data = client.Receive(ref endpoint);
                        OnPropertyChanged(String.Format("{0}{1}", Encoding.UTF8.GetString(data, 0, data.Length), Environment.NewLine));
                    }

                    client.Close();
                    client.Dispose();
                }
                catch (Exception ex)
                {
                    BGLogger.Error(ex, $"BGLoggerNetworkListener() -> Exception with Reason: '{ex.Message}'.");
                }
            }, _cts.Token);

            if (bAutomaticEnableSenderOnLocalHost == true)
                BGLogger.Configuration.Network = true;
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}
