using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LittleGameSever.SeverManager
{
    class ClientHandler
    {
        private Socket socket;
        private int number;
        private Thread recvThread;

        public ClientHandler(int clientNumber, Socket clientSocket)
        {
            number = clientNumber;

            recvThread = new Thread(RecvMessage);
            recvThread.IsBackground = true;
            recvThread.Start();
        }

        private void RecvMessage()
        {
            byte[] bytes;
            while (true)
            {
                bytes = new byte[2048];
                try
                {
                    socket.Receive(bytes);
                }
                catch (System.Exception e)
                {
                    Console.WriteLine(e.Data);
                    break;
                }
                string message = System.Text.Encoding.UTF8.GetString(bytes);
                string[] messages = message.Split(',');
            }
        }

        public bool SendMessage(string message)
        {
            bool connectState = true;
            bool blockingState = socket.Blocking;

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(message);
            try
            {
                socket.Blocking = false;
                socket.Send(bytes);
            }
            catch (SocketException e)
            {
                // 10035 == WSAEWOULDBLOCK
                if (e.NativeErrorCode.Equals(10035))
                {
                    //Console.WriteLine("Still Connected, but the Send would block");
                    connectState = true;
                }
                else
                {
                    //Console.WriteLine("Disconnected: error code {0}!", e.NativeErrorCode);
                    connectState = false;
                }
            }
            socket.Blocking = blockingState;

            return connectState;
        }

        public void Close()
        {
            if(socket != null)
            {
                socket.Close();
                socket = null;
            }
            if (recvThread != null)
            {
                recvThread.Abort();
                recvThread = null;
            }
        }
    }
}
