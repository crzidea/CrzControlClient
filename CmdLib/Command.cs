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
        public static string ExecuteControl(CmdMsg cmd)
        {
            string result = null;
            switch (cmd.msg)
            {
                case "volume":
                    switch (cmd.value)
                    {
                        case "up":
                            Volume.Up();
                            result = "Turned volume up!";
                            break;
                        case "down":
                            Volume.Down();
                            result = "Turned volume down!";
                            break;
                        case "mute":
                            Volume.Mute();
                            result = "Sound: On/Off!";
                            break;
                    }
                    break;
                case "brightness":
                    Brightness.Set(int.Parse(cmd.value));
                    break;
                case "player":
                    switch (cmd.value)
                    {
                        case "prev":
                            Player.Prev();
                            break;
                        case "next":
                            Player.Next();
                            break;
                        case "play":
                            Player.Prev();
                            break;
                    }
                    break;
                case "powerpoint":
                    switch (cmd.value)
                    {
                        case "prev":
                            PowerPoint.Prev();
                            break;
                        case "next":
                            PowerPoint.Next();
                            break;
                    }
                    break;
                default:
                    result = "Can't do it!";
                    break;
            }

            return result;
        }
    }
}
