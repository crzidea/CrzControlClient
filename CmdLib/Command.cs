using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CmdLib
{
    public class Command
    {
        static public string ExecuteCommand(string cmd)
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.StandardInput.WriteLine(cmd.Trim());
            //process.WaitForInputIdle();
            process.StandardInput.WriteLine("exit");
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            process.Close();

            output = output.Substring(output.IndexOf(">"));
            output = output.Substring(output.IndexOf("\n") + 1);
            output = output.Substring(0, output.LastIndexOf(">"));
            output = output.Substring(0, output.LastIndexOf("\n") - 1);
            return output;
        }

        #region 设置音量API
        private const byte VK_VOLUME_MUTE = 0xAD;
        private const byte VK_VOLUME_DOWN = 0xAE;
        private const byte VK_VOLUME_UP = 0xAF;
        private const UInt32 KEYEVENTF_EXTENDEDKEY = 0x0001;
        private const UInt32 KEYEVENTF_KEYUP = 0x0002;

        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, UInt32 dwFlags, UInt32 dwExtraInfo);

        [DllImport("user32.dll")]
        static extern Byte MapVirtualKey(UInt32 uCode, UInt32 uMapType);

        public static void VolumeUp()
        {
            keybd_event(VK_VOLUME_UP, MapVirtualKey(VK_VOLUME_UP, 0), KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(VK_VOLUME_UP, MapVirtualKey(VK_VOLUME_UP, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }

        public static void VolumeDown()
        {
            keybd_event(VK_VOLUME_DOWN, MapVirtualKey(VK_VOLUME_DOWN, 0), KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(VK_VOLUME_DOWN, MapVirtualKey(VK_VOLUME_DOWN, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }

        public static void Mute()
        {
            keybd_event(VK_VOLUME_MUTE, MapVirtualKey(VK_VOLUME_MUTE, 0), KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(VK_VOLUME_MUTE, MapVirtualKey(VK_VOLUME_MUTE, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }
        #endregion

        public static string ExecuteControl(CmdMsg cmd)
        {
            switch (cmd.msg)
            {
                case "volume":
                    switch (cmd.value)
                    {
                        case "up":
                            VolumeUp();
                            break;
                        case "down":
                            VolumeDown();
                            break;
                        case "mute":
                            Mute();
                            break;
                    }
                    break;
                case "light":
                    break;
            }

            return "As you wish!";
        }
    }
}
