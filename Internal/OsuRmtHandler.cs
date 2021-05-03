using System;
using System.Collections.Generic;
using System.Text;
using DebugLogger;
using OsuRemote.Internal.ConnectionProtocol;

namespace OsuRemote.Internal
{
    public class OsuRmtHandler
    {
        UDPConnection udpConnection = new UDPConnection();

        Dictionary<int, KeyCode> activeKey = new Dictionary<int, KeyCode>();

        List<KeyCode> useableKeys = new List<KeyCode>() { KeyCode.W, KeyCode.D};

        public void Initialize()
        {
            udpConnection.Touched += NewInput;

            ListenForInput();
        }

        private void NewInput(object sender, TouchState touch)
        {
            int id = touch.touchIndex;
            TouchPhase phase = touch.phase;

            if (id >= 2)
                return;

            if (!activeKey.ContainsKey(id))
                activeKey.Add(id, useableKeys[id]);

            ExecuteInput(touch);

            DLog.Log(touch.touchIndex + " : " + touch.phase.ToString());
        }

        private void ExecuteInput(TouchState touch)
        {
            if (touch.phase == TouchPhase.Began)
            {
                KeyEmulator.PressKey(activeKey[touch.touchIndex]);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                KeyEmulator.ReleaseKey(activeKey[touch.touchIndex]);
            }
        }

        public void Close()
        {
            StopListening();

            udpConnection.Touched -= NewInput;
        }

        public void ListenForInput()
        {
            udpConnection.Listen();
        }

        public void StopListening()
        {
            udpConnection.StopListening();
        }
    }
}
