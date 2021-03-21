using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using LittleGame.State;

namespace LittleGame.Sever
{
    class ClientSocketManager
    {
        ClientPlayingState state;

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
            try
            {
                if(recvThread != null)
                {
                    recvThread.Abort();
                    recvThread = null;
                }
                if(clientSocket != null)
                {
                    clientSocket.Close();
                    clientSocket = null;
                }
            }
            catch
            {

            }
        }
        

        private void SendMessage(String message)
        {
            if (Connected)
            {
                try
                {
                    clientSocket.Send(System.Text.Encoding.UTF8.GetBytes(message));
                }
                catch
                {
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
                    System.Console.WriteLine(messages[0]);

                    if (messages[0] == "Start")
                    {
                        gameStart = true;
                    }
                    else if (messages[0] == "Move")
                    {
                        state.players[int.Parse(messages[1])].SetPoint(int.Parse(messages[2]), int.Parse(messages[3]));
                    }
                    else if (messages[0] == "PlayerNum")
                    {
                        playerNum = int.Parse(messages[1]);
                    }
                }
                catch
                {
                    CloseConnection();
                }
            }
        }

    }
}
