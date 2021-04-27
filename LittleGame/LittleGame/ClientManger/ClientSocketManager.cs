using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using LittleGame.State;

namespace LittleGame.Client
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

        private void waitingConnect()
        {
            Thread.Sleep(3000);
            if(!connected)
            {
                CloseConnection();
            }
        }

        public string GetSeverIp(string IP, int port)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Thread waitThread = new Thread(waitingConnect);
            waitThread.IsBackground = true;
            waitThread.Start();
            string sever_ip = "null";
            try
            {
                clientSocket.Connect(IPAddress.Parse(IP), port);
                clientSocket.Send(System.Text.Encoding.UTF8.GetBytes("BeClient"));
                byte[] bytes = new byte[2048];
                clientSocket.Receive(bytes);
                string message = System.Text.Encoding.UTF8.GetString(bytes);
                string[] messages = message.Split(';');
                string[] messageArgs = messages[0].Split(',');
                if (messageArgs[0] == "Success")
                {
                    connected = true;
                    sever_ip = messageArgs[1];
                }
                else if (messageArgs[0] == "Fail")
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
                else
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                CloseConnection();
            }
            if(clientSocket != null)
            {
                clientSocket.Close();
                clientSocket = null;
            }
            connected = false;
            waitThread.Join();
            return sever_ip;
        }

        public void StartConnect(string IP, int port)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Thread waitThread = new Thread(waitingConnect);
            waitThread.IsBackground = true;
            waitThread.Start();
            try
            {
                clientSocket.Connect(IPAddress.Parse(IP), port);
                byte[] bytes = new byte[2048];
                clientSocket.Receive(bytes);
                string message = System.Text.Encoding.UTF8.GetString(bytes);
                string[] messages = message.Split(';');
                string[] messageArgs = messages[0].Split(',');
                if (messageArgs[0] == "Full")
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
                else if (messageArgs[0] == "Id")
                {
                    playerId = int.Parse(messageArgs[1]);
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
            message += ";";
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
                    string[] messages = message.Split(';');
                    for (int i = 0; i < messages.Length; i++)
                    {
                        string[] messageArgs = messages[i].Split(',');
                        Console.WriteLine(message);

                        if (messageArgs[0].Equals("Start"))
                        {
                            gameStart = true;
                        }
                        else if (messageArgs[0].Equals("Id"))
                        {
                            playerId = int.Parse(messageArgs[1]);
                        }
                        else if (messageArgs[0].Equals("Move"))
                        {
                            state.players[int.Parse(messageArgs[1])].SetPoint(int.Parse(messageArgs[2]), int.Parse(messageArgs[3]));
                        }
                        else if (messageArgs[0].Equals("Face"))
                        {
                            state.players[int.Parse(messageArgs[1])].Face = int.Parse(messageArgs[2]);
                        }
                        else if (messageArgs[0].Equals("Attack"))
                        {
                            state.players[int.Parse(messageArgs[1])].Attack = true;
                        }
                        else if (messageArgs[0].Equals("Reload"))
                        {
                            state.players[int.Parse(messageArgs[1])].Reload = true;
                        }
                        else if (messageArgs[0].Equals("ReloadDone"))
                        {
                            state.players[int.Parse(messageArgs[1])].ReloadDone = true;
                        }
                        else if (messageArgs[0].Equals("Dead"))
                        {
                            state.players[int.Parse(messageArgs[1])].Dead = true;
                        }
                        else if (messageArgs[0].Equals("PlayerNum"))
                        {
                            playerNum = int.Parse(messageArgs[1]);
                        }
                        else if (messageArgs[0].Equals("GameOver"))
                        {
                            gameStart = false;
                        }
                        Thread.Sleep(5);
                    }
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
