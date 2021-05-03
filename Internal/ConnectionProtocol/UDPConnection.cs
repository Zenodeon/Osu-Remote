using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using DebugLogger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OsuRemote.Internal.ConnectionProtocol
{
    public class UDPConnection
    {
        public int defaultPort { get; private set; } = 64;

        private static CancellationTokenSource CTS = new CancellationTokenSource();
        private static CancellationToken CT = CTS.Token;

        private bool _listen = false;

        private UdpClient client;
        private IPEndPoint endPoint;

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

        public event EventHandler<TouchState> Touched;

        public async void Listen(string ip = null)
        {
            if (listen)
                return;

            IPAddress address;

            if (ip != null)
                address = IPAddress.Parse(ip);
            else
                address = IPAddress.Any;

            listen = true;

            endPoint = new IPEndPoint(address, defaultPort);
            client = new UdpClient(endPoint);
 
            DLog.Log("Listening Starting");

            try
            {
                while (listen)
                {                   
                    UdpReceiveResult receiveBytes = await client.ReceiveAsync().WithCancellation(CT);

                    if (receiveBytes.Buffer.Length > 0)
                    {
                        string data = Encoding.ASCII.GetString(receiveBytes.Buffer);

                        TouchState touchState = new TouchState();

                        bool phaseSuccess = true;

                        try
                        {
                            touchState = JsonConvert.DeserializeObject<TouchState>(data);

                        }
                        catch (Exception ex)
                        {
                            phaseSuccess = false;
                            DLog.Log(ex.Message);
                        }

                        if (phaseSuccess)
                        {
                            OnTouch(touchState);                           
                        }
                        else
                        {
                            DLog.Log(data);
                        }                       
                    }
                }
            }
            catch (OperationCanceledException)
            {
                client.Close();
            }
        }

        public void StopListening()
        {
            listen = false;

            CTS.Cancel();

            DLog.Log("Listening Closed");
        }

        protected virtual void OnTouch(TouchState e)
        {
            EventHandler<TouchState> handler = Touched;

            if (handler != null)
                handler(this, e);
        }
    }
    public static class AsyncExtensions
    {
        public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            using (cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).TrySetResult(true), tcs))
            {
                if (task != await Task.WhenAny(task, tcs.Task))
                {
                    throw new OperationCanceledException(cancellationToken);
                }
            }

            return task.Result;
        }
    }
}
