﻿using System;
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
        public int defaultPort { get; private set; } = 60;


        private static CancellationTokenSource CTS = new CancellationTokenSource();
        private static CancellationToken CT = CTS.Token;

        private Task listening;

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

        public UDPConnection()
        {

        }

        public async void Listen(IPAddress address)
        {
            if (listen)
                return;

            listen = true;

            endPoint = new IPEndPoint(address, defaultPort);
            client = new UdpClient(endPoint);
 
            /*
            listening = Task.Run(() =>
            {
                while (listen)
                {
                    byte[] receiveBytes = await client.ReceiveAsync(ref endPoint).WithCancellation(CT);
                    DLog.Log("Hi");
                }
            }, CT);*/

            DLog.Log("Listening Starting");

            try
            {
                while (listen)
                {
                    UdpReceiveResult receiveBytes = await client.ReceiveAsync().WithCancellation(CT);

                    if (receiveBytes.Buffer.Length > 0)
                    {
                        DLog.Log(Encoding.ASCII.GetString(receiveBytes.Buffer));
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
