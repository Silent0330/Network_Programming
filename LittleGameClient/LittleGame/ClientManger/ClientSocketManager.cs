using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using LittleGame.State;

namespace LittleGame.Sever
{
    class ClientSocketManager
    {
        public ClientPlayingState state;

        private Socket clientSocket;
        private Thread recvThread;

        private int playerId;
        public int PlayerId { get => playerId; }
        private int playerNum;
        public int PlayerNum { get => playerNum; }

        private bool connected;
        public bool Connected { get => connected; }
        private bool gameStart;
        public bool GameStart { get => gameStart; }

        

        public ClientSocketManager()
        {
            connected = false;
            gameStart = false;
        }
        
        public void StartConnect(string IP, int port)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                clientSocket.Connect(IPAddress.Parse(IP), port);
                byte[] bytes = new byte[2048];
                string message;
                clientSocket.Receive(bytes);
                message = System.Text.Encoding.UTF8.GetString(bytes);
                string[] messages = message.Split(',');
                if (messages[0] == "Full")
                {
                    try
                    {
                        clientSocket.Shutdown(SocketShutdown.Both);
                    }
                    finally
                    {
                        clientSocket = null;
                    }
                }
                else if (messages[0] == "Id")
                {
                    playerId = int.Parse(messages[1]);
                    connected = true;
                    recvThread = new Thread(rcvMessage);
                    recvThread.IsBackground = true;
                    recvThread.Start();
                }
                else
                {
                    try
                    {
                        Console.WriteLine("No id");
                        clientSocket.Shutdown(SocketShutdown.Both);
                    }
                    finally
                    {
                        clientSocket = null;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void CloseConnection()
        {
            connected = false;
            gameStart = false;
            try
            {
                if (clientSocket != null)
                {
                    clientSocket.Close();
                    clientSocket = null;
                }
                if (recvThread != null)
                {
                    recvThread.Abort();
                    recvThread = null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        

        public void SendMessage(string message)
        {
            message += ",";
            if (Connected)
            {
                try
                {
                    clientSocket.Send(System.Text.Encoding.UTF8.GetBytes(message));
                    Thread.Sleep(5);
                }
                catch (Exception e)
                {
                    Console.Write(e.ToString());
                    CloseConnection();
                }
            }
        }

        private void rcvMessage()
        {
            while (Connected)
            {
                try
                {
                    byte[] bytes = new byte[2048];
                    clientSocket.Receive(bytes);
                    string message = System.Text.Encoding.UTF8.GetString(bytes);
                    string[] messages = message.Split(',');
                    Console.WriteLine(message);

                    if (messages[0].Equals("Start"))
                    {
                        gameStart = true;
                        SendMessage("Ready," + true.ToString());
                    }
                    else if (messages[0].Equals("Id"))
                    {
                        playerId = int.Parse(messages[1]);
                    }
                    else if (messages[0].Equals("Move"))
                    {
                        state.players[int.Parse(messages[1])].SetPoint(int.Parse(messages[2]), int.Parse(messages[3]));
                    }
                    else if (messages[0].Equals("Face"))
                    {
                        state.players[int.Parse(messages[1])].Face = int.Parse(messages[2]);
                    }
                    else if (messages[0].Equals("Attack"))
                    {
                        state.players[int.Parse(messages[1])].Attack = true;
                    }
                    else if (messages[0].Equals("BulletMove"))
                    {
                        if(int.Parse(messages[1]) < state.clientBullets_List.Count)
                            state.clientBullets_List[int.Parse(messages[1])].SetPoint(int.Parse(messages[2]), int.Parse(messages[3]));
                    }
                    else if (messages[0].Equals("BulletRemove"))
                    {
                        if (int.Parse(messages[1]) < state.clientBullets_List.Count)
                            state.clientBullets_List[int.Parse(messages[1])].End = true ;
                    }
                    else if (messages[0].Equals("Reload"))
                    {
                        state.players[int.Parse(messages[1])].Reload = true;
                    }
                    else if (messages[0].Equals("PlayerNum"))
                    {
                        playerNum = int.Parse(messages[1]);
                    }
                    else if (messages[0].Equals("GameOver"))
                    {
                        gameStart = false;
                    }
                    Thread.Sleep(10);
                }
                catch (Exception e)
                {
                    Console.Write(e.ToString());
                    CloseConnection();
                }
            }
        }

    }
}
