using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using LittleGame.State;
using System.Collections.Generic;

namespace LittleGame.Sever
{
    class ClientSocketManager
    {
        public ClientPlayingState state;

        private Socket clientSocket;
        private Thread recvThread;

        private List<string> receivedMessages;
        public List<string> ReceivedMessages { get => receivedMessages; }
        private Mutex recvMsgMutex;
        public void AddReceivedMessage(string message)
        {
            recvMsgMutex.WaitOne();
            receivedMessages.Add(message);
            recvMsgMutex.ReleaseMutex();
        }
        public string PopReceivedMessage(int index)
        {
            string result = null;
            recvMsgMutex.WaitOne();
            if (index >= 0 && index <= ReceivedMessages.Count)
            {
                result = receivedMessages[index];
                receivedMessages.RemoveAt(index);
            }
            recvMsgMutex.ReleaseMutex();
            return result;
        }


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
            receivedMessages = new List<string>();
            recvMsgMutex = new Mutex();
        }

        private void waitingConnect()
        {
            Thread.Sleep(1000);
            if (!connected)
            {
                CloseConnection();
            }
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
                connected = true;
                waitThread.Abort();
                byte[] bytes = new byte[2048];
                int ret = clientSocket.Receive(bytes);
                if(ret <= 0)
                {
                    return;
                }
                string message = System.Text.Encoding.UTF8.GetString(bytes);
                message = message.Replace("\n", "");
                Console.WriteLine(message);
                string[] messages = message.Split(';');
                for (int i = 0; i < messages.Length; i++)
                {
                    string[] messageArgs = messages[i].Split(',');
                    if (messageArgs[0] == "Full")
                    {
                        connected = false;
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
                    else if (messageArgs[0].Equals("PlayerNum"))
                    {
                        playerNum = int.Parse(messageArgs[1]);
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
            receivedMessages.Clear();
            while (Connected)
            {
                try
                {
                    byte[] bytes = new byte[2048];
                    int ret = clientSocket.Receive(bytes);
                    if(ret <= 0)
                    {
                        CloseConnection();
                        break;
                    }
                    string message = System.Text.Encoding.UTF8.GetString(bytes);
                    message = message.Replace("\n", "");
                    Console.WriteLine(message);
                    string[] messages = message.Split(';');
                    for (int i = 0; i < messages.Length; i++)
                    {
                        string[] messageArgs = messages[i].Split(',');

                        if (messageArgs[0].Equals("Start"))
                        {
                            gameStart = true;
                        }
                        else if (messageArgs[0].Equals("Id"))
                        {
                            playerId = int.Parse(messageArgs[1]);
                        }
                        else if (messageArgs[0].Equals("PlayerNum"))
                        {
                            playerNum = int.Parse(messageArgs[1]);
                        }
                        else if (messageArgs[0].Equals("Move"))
                        {
                            AddReceivedMessage(messages[i]);
                        }
                        else if (messageArgs[0].Equals("Face"))
                        {
                            AddReceivedMessage(messages[i]);
                        }
                        else if (messageArgs[0].Equals("Attack"))
                        {
                            AddReceivedMessage(messages[i]);
                        }
                        else if (messageArgs[0].Equals("Reload"))
                        {
                            AddReceivedMessage(messages[i]);
                        }
                        else if (messageArgs[0].Equals("ReloadDone"))
                        {
                            AddReceivedMessage(messages[i]);
                        }
                        else if (messageArgs[0].Equals("Hitted"))
                        {
                            AddReceivedMessage(messages[i]);
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
