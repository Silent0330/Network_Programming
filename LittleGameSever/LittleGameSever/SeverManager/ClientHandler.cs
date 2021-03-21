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
        private int id;
        private Thread recvThread;

        private bool connected;
        public bool Connected { get => connected; }

        private bool readyToStart;
        public bool ReadyToStart { get => readyToStart; }

        //datas
        private bool up;
        public bool Up { get => up; }
        private bool down;
        public bool Down { get => down; }
        private bool left;
        public bool Left { get => left; }
        private bool right;
        public bool Right { get => right; }


        public ClientHandler(int clientId, Socket clientSocket)
        {
            this.id = clientId;
            socket = clientSocket;

            connected = true;
            readyToStart = false;

            recvThread = new Thread(RecvMessage);
            recvThread.IsBackground = true;
            recvThread.Start();
        }

        public void ChangeId(int new_id)
        {
            id = new_id;
            SendMessage("Id," + id.ToString());
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

                if (messages[0].Equals("Up"))
                {
                    if (messages[1].Equals("T")) up = true;
                    if (messages[1].Equals("F")) up = false;
                }
                else if (messages[0].Equals("Down"))
                {
                    if (messages[1].Equals("T")) down = true;
                    if (messages[1].Equals("F")) down = false;
                }
                else if (messages[0].Equals("Left"))
                {
                    if (messages[1].Equals("T")) left = true;
                    if (messages[1].Equals("F")) left = false;
                }
                else if (messages[0].Equals("Right"))
                {
                    if (messages[1].Equals("T")) right = true;
                    if (messages[1].Equals("F")) right = false;
                }
            }
        }

        public bool SendMessage(string message)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(message);
            try
            {
                socket.Send(bytes);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
            return true;
        }

        public void Close()
        {
            connected = false;
            if (socket != null)
            {
                socket.Shutdown(SocketShutdown.Both);
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
