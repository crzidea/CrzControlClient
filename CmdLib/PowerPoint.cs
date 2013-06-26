using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CmdLib
{
    class PowerPoint
    {
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, UInt32 dwFlags, UInt32 dwExtraInfo);

        [DllImport("user32.dll")]
        static extern Byte MapVirtualKey(UInt32 uCode, UInt32 uMapType);

        private const UInt32 KEYEVENTF_EXTENDEDKEY = 0x0001;
        private const UInt32 KEYEVENTF_KEYUP = 0x0002;
        private const byte VK_LEFT = 0x25;
        private const byte VK_RIGHT = 0x27;

        public static void Prev()
        {
            keybd_event(VK_LEFT, MapVirtualKey(VK_LEFT, 0), KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(VK_LEFT, MapVirtualKey(VK_LEFT, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }

        public static void Next()
        {
            keybd_event(VK_RIGHT, MapVirtualKey(VK_RIGHT, 0), KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(VK_RIGHT, MapVirtualKey(VK_RIGHT, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }
    }
}
