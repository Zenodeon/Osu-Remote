using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using DebugLogger;

namespace ConnectionProtocol
{
    public class UDPConnection
    {
        public int defaultPort { get; private set; } = 8080;


        private static CancellationTokenSource CTS = new CancellationTokenSource();
        private static CancellationToken CT = CTS.Token;

        private Task listening;

        private bool _listen = false;

        public bool listen
        {
            get
            {
                return _listen;
            }
            private set
            {
                _listen = value;

                if (!value)
                    CTS.Cancel();
            }
        } 

        public UDPConnection()
        {

        }

        public void Listen(IPAddress address)
        {
            if (listening != null)
                if (listening.Status == TaskStatus.Running)
                    listen = false;

            listen = true;

            listening = Task.Run(() =>
            {
                while (listen)
                {
                    Thread.Sleep(100);
                    DLog.Log("Hi");
                }
            });
        }

        public void StopListening()
        {
            listen = false;
        }
    }
}
