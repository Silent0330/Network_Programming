using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LittleGameSever.SeverManager
{
    class ClientHandler
    {
        private Socket socket;
        public Socket Socket { get => socket; }
        private int id;
        private Thread recvThread;

        private bool connected;
        public bool Connected { get => connected; }
        private bool disConnectChecked;
        public bool DisConnectChecked { get => disConnectChecked; }
        private IPAddress ip;
        public IPAddress Ip { get => ip; }
        private int port;
        public int Port { get => port; }

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
        private bool attack;
        public bool Attack { get => attack; }
        private bool reload;
        public bool Reload { get => reload; }
        private bool startGameRequest;
        public bool StartGameRequest { get => startGameRequest; set => startGameRequest = value; }


        public ClientHandler(int clientId, Socket clientSocket)
        {
            this.id = clientId;
            this.socket = clientSocket;
            this.ip = IPAddress.Parse(((IPEndPoint)socket.RemoteEndPoint).Address.ToString());
            this.port = ((IPEndPoint)socket.RemoteEndPoint).Port;

            up = down = left = right = false;


            connected = true;
            disConnectChecked = false;
            readyToStart = false;
            startGameRequest = false;

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
                    int ret = socket.Receive(bytes);
                    if(ret <= 0)
                    {
                        connected = false;
                        break;
                    }
                }
                catch (System.Exception e)
                {
                    Console.WriteLine(e.Data);
                    connected = false;
                    break;
                }
                string message = System.Text.Encoding.UTF8.GetString(bytes);
                message = message.Replace("\n", "");
                string[] messages = message.Split(';');
                for(int i = 0; i < messages.Length; i++)
                {
                    Console.WriteLine("client= " + id + " recv= " + messages[i]);
                    string[] messageArgs = messages[i].Split(',');

                    if (messageArgs[0].Equals("Up"))
                    {
                        up = bool.Parse(messageArgs[1]);
                    }
                    else if (messageArgs[0].Equals("Down"))
                    {
                        down = bool.Parse(messageArgs[1]); ;
                    }
                    else if (messageArgs[0].Equals("Left"))
                    {
                        left = bool.Parse(messageArgs[1]);
                    }
                    else if (messageArgs[0].Equals("Right"))
                    {
                        right = bool.Parse(messageArgs[1]);
                    }
                    else if (messageArgs[0].Equals("Attack"))
                    {
                        attack = bool.Parse(messageArgs[1]);
                    }
                    else if (messageArgs[0].Equals("Reload"))
                    {
                        reload = bool.Parse(messageArgs[1]);
                    }
                    else if (messageArgs[0].Equals("StartGame"))
                    {
                        startGameRequest = true;
                    }
                    else if (messageArgs[0].Equals("Ready"))
                    {
                        readyToStart = bool.Parse(messageArgs[1]);
                    }
                }
            }
        }

        public bool SendMessage(string message)
        {
            if(connected)
            {
                if(!message.Last().Equals(';'))
                    message += ";";
                message += "\n";
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(message);
                try
                {
                    socket.Send(bytes);
                    Thread.Sleep(1);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    connected = false;
                    return false;
                }
                return true;
            }
            return false;
        }

        public void Close()
        {
            connected = false;
            disConnectChecked = true;
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
