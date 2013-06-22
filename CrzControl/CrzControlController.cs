using CmdLib;

namespace CrzControl
{
    class CrzControlController
    {
        CrzClient client = null
            ;
        #region Fucking Invoking!!!
        private MainWindow win = null;
        private delegate void AppendMessageDelegate(string message);
        private AppendMessageDelegate AppendMessageCallBack = null;
        private void AppendMessage(string message)
        {
            message += "\r\n";
            win.txtConsole.AppendText(message);
            win.txtConsole.ScrollToEnd();
        }
        public void WriteLine(string message)
        {
            win.Dispatcher.Invoke(AppendMessageCallBack, new object[] { message });
        }
        #endregion

        private void Setup()
        {
            AppendMessageCallBack = new AppendMessageDelegate(AppendMessage);
            client = new CrzClient(this);
            client.Start(win.txtUsername.Text, win.txtPassword.Password);
        }

        public CrzControlController(MainWindow win)
        {
            this.win = win;
            Setup();
        }
        ~CrzControlController()
        {
            client.Close();
        }

        public void HandleCommand(CmdMsg cmd)
        {
            string recvMsg = "Recv new message: " + cmd.msg;
            WriteLine(recvMsg);
            string result = null;
            try
            {
                result = PreCommand(cmd);
            }
            catch (System.Exception ex)
            {
                WriteLine(ex.Message);
                return;
            }
            WriteLine(result);
            client.CmdComplete(result);
        }

        private string PreCommand(CmdMsg cmd)
        {
            string message = cmd.msg;
            string header = message.Substring(0, message.IndexOf("://"));
            if (header == "command")
            {
                string cmdString = message.Substring(message.IndexOf("://") + 3);
                return Command.ExecuteCommand(cmdString);
            }
            else if (header == "control")
            {
                cmd.msg = message.Substring(message.IndexOf("://") + 3);
                return Command.ExecuteControl(cmd);
            }
            else
            {
                return null;
            }
        }

    }
}
