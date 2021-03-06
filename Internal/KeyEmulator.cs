using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.IO;

namespace OsuRemote.Internal
{
    static class KeyEmulator
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        public static void PressKey(KeyCode key)
        {
                keybd_event((byte)key, 0x45, 0 | 0, 0);
        }

        public static void ReleaseKey(KeyCode key)
        {
            keybd_event((byte)key, 0x45, 2 | 0, 0);
        }
    }

    public enum KeyCode
    {
        A = 0x41,
        B = 0x42,
        C = 0x43,
        D = 0x44,
        E = 0x45,
        F = 0x46,
        G = 0x47,
        H = 0x48,
        I = 0x49,
        J = 0x4A,
        K = 0x4B,
        L = 0x4C,
        M = 0x4D,
        N = 0x4E,
        O = 0x4F,
        P = 0x50,
        Q = 0x51,
        R = 0x52,
        S = 0x53,
        T = 0x54,
        U = 0x55,
        V = 0x56, 
        W = 0x57,
        X = 0x58,
        Y = 0x59,
        Z = 0x5A
    }
}
