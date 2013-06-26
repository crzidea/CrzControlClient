using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CmdLib
{
    class Brightness
    {
        [DllImport("gdi32.dll")]
        public static extern int GetDeviceGammaRamp(IntPtr hDC, ref RAMP lpRamp);

        [DllImport("gdi32.dll")]
        public static extern int SetDeviceGammaRamp(IntPtr hDC, ref RAMP lpRamp);

        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hWnd);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct RAMP
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public UInt16[] Red;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public UInt16[] Green;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public UInt16[] Blue;
        }

        static RAMP ramp = new RAMP();

        public static void Set(int gamma)
        {
            ramp.Red = new ushort[256];
            ramp.Green = new ushort[256];
            ramp.Blue = new ushort[256];

            for (int i = 1; i < 256; i++)
            {
                // gamma 必须在3和44之间
                ramp.Red[i] = ramp.Green[i] = ramp.Blue[i] = (ushort)(Math.Min(65535, Math.Max(0, Math.Pow((i + 1) / 256.0, gamma * 0.1) * 65535 + 0.5)));
            }
            SetDeviceGammaRamp(GetDC(IntPtr.Zero), ref ramp);
        }
    }
}
