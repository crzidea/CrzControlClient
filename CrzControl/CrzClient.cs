using CmdLib;
using SocketIOClient;
using System;

namespace CrzControl
{
    class CrzClient
    {
        Client socket;
        string serverUrl = "http://lx-pc/";
        string user;
        string pass;

        CrzControlController controller;
        public CrzClient(CrzControlController controller)
        {
            this.controller = controller;
        }

        public void Start(string user, string pass)
        {
            this.user = user;
            this.pass = pass;
            Start();
        }

        private void Start()
        {
            controller.WriteLine("Starting CrzControl client...");

            socket = new Client(serverUrl); // url to the nodejs / socket.io instance

            socket.Opened += SocketOpened;
            socket.Message += SocketMessage;
            socket.SocketConnectionClosed += SocketConnectionClosed;
            socket.Error += SocketError;

            // register for 'connect' event with io server
            socket.On("connect", (fn) =>
            {
                controller.WriteLine("\r\nConnected event...\r\n");
                // emit Json Serializable object, anonymous types, or strings
                CmdMsg msg = new CmdMsg() { from = "pc", user = this.user, pass = this.pass };
                socket.Emit("login", msg);
            });

            socket.On("loginSuccess", (data) =>
            {
                CmdMsg msg = data.Json.GetFirstArgAs<CmdMsg>();
                controller.WriteLine("[login success]: " + msg.msg);
            });
            socket.On("loginFailed", (data) =>
            {
                CmdMsg msg = data.Json.GetFirstArgAs<CmdMsg>();
                controller.WriteLine("[login failed]: " + msg.msg);
            });

            socket.On("command", (data) =>
            {
                CmdMsg cmd = data.Json.GetFirstArgAs<CmdMsg>();
                //controller.WriteLine("New command arrived: " + cmd.Msg);
                controller.HandleCommand(cmd);
            });

            // make the socket.io connection
            socket.Connect();
        }

        public void CmdComplete(string result)
        {
            CmdMsg data = new CmdMsg() { msg = result, user = this.user };
            socket.Emit("cmdComplete", data);
        }

        void SocketError(object sender, ErrorEventArgs e)
        {
            controller.WriteLine("socket client error:");
            controller.WriteLine(e.Message);
        }

        void SocketConnectionClosed(object sender, EventArgs e)
        {
            controller.WriteLine("WebSocketConnection was terminated!");
        }

        void SocketMessage(object sender, MessageEventArgs e)
        {
            // uncomment to show any non-registered messages
            //if (string.IsNullOrEmpty(e.Message.Event))
            //    Console.WriteLine("Generic SocketMessage: {0}", e.Message.MessageText);
            //else
            //    Console.WriteLine("Generic SocketMessage: {0} : {1}", e.Message.Event, e.Message.JsonEncodedMessage.ToJsonString());
        }

        void SocketOpened(object sender, EventArgs e)
        {

        }

        public void Close()
        {
            if (this.socket != null)
            {
                socket.Opened -= SocketOpened;
                socket.Message -= SocketMessage;
                socket.SocketConnectionClosed -= SocketConnectionClosed;
                socket.Error -= SocketError;
                this.socket.Dispose(); // close & dispose of socket client
            }
        }
    }
}
