using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.IO;

namespace Osu_Remote
{
    static class KeyEmulator
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        public static void Press(bool press)
        {
            //test();

            //return;

            if (press)
            {
                //emulator.Keyboard.KeyDown(VirtualKeyCode.VK_Z);
                keybd_event((int)Key.Z, 0x45, 0 | 0, 0);
            }
            else
            {
                keybd_event((int)Key.Z, 0x45, 2 | 0, 0);
            }
        }
    }
}
