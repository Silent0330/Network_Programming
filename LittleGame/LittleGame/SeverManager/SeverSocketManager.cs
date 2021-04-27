using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Timers;
using LittleGame;

namespace LittleGameSever.SeverManager
{
    class SeverSocketManager
    {
        #region declare
        Form1 form;
		private Socket severSocket, registerSocket;
		public List<ClientHandler> clientHandler_List;
        private List<int> clientId_List;

        bool connected;
        public bool Connected { get => connected; }

        //Thread
        private Thread listeningThread;
        private System.Timers.Timer checkTimer;

        //Connection Properties
        private int maxConnectionNum;
		public int MaxConnectionNum{ get => maxConnectionNum; }
		private int curConnectionNum;
		public int CurConnectionNum{
            get => curConnectionNum;
            private set
            {
                curConnectionNum = value;
                for(int i = 0; i < clientHandler_List.Count; i++)
                {
                    clientHandler_List[i].SendMessage("PlayerNum," + curConnectionNum.ToString());
                }
            }
        }
		
        private bool listening;
        public bool Listening { get => listening; }
        #endregion

        public SeverSocketManager(Form1 form, int maxConnectionNum)
        {
            this.form = form;
            connected = false;


            if (maxConnectionNum > 0)
            {
                this.maxConnectionNum = maxConnectionNum;
            }
            else
            {
                maxConnectionNum = 0;
            }

            checkTimer = new System.Timers.Timer();
            checkTimer.Interval = 1000;
            checkTimer.Elapsed += CheckingConnection;
            checkTimer.Start();

            clientHandler_List = new List<ClientHandler>();
            clientId_List = new List<int>();
            
            curConnectionNum = 0;

        }

        public void CheckingConnection(object sender, ElapsedEventArgs e)
        {
            for (int i = clientHandler_List.Count-1; i >= 0; i--)
            {
                if(clientHandler_List[i].Connected)
                    clientHandler_List[i].CheckConnection();
                if (!clientHandler_List[i].Connected)
                {
                    try
                    {
                        if(!clientHandler_List[i].DisConnectChecked)
                        {
                            clientHandler_List[i].Close();
                            CurConnectionNum = curConnectionNum - 1;
                        }
                        if(!form.Playing)
                        {
                            clientHandler_List.RemoveAt(i);
                            clientId_List.RemoveAt(i);
                            for (int j = i; j < clientHandler_List.Count; j++)
                            {
                                clientHandler_List[j].ChangeId(j);
                                clientId_List[j] = j;
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.ToString());
                    }
                }
            }
        }

        private void waitingConnect()
        {
            Thread.Sleep(3000);
            if (!connected)
            {
                Close();
            }
        }

        public void StartConnect(string IP, int port)
        {
            registerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Thread waitThread = new Thread(waitingConnect);
            waitThread.IsBackground = true;
            waitThread.Start();
            try
            {
                registerSocket.Connect(IPAddress.Parse(IP), port);
                registerSocket.Send(System.Text.Encoding.UTF8.GetBytes("BeSever"));
                byte[] bytes = new byte[2048];
                registerSocket.Receive(bytes);
                string message = System.Text.Encoding.UTF8.GetString(bytes);
                string[] messages = message.Split(';');
                if (messages[0] == "Success")
                {
                    connected = true;
                }
                else if(messages[0] == "Fail")
                {
                    try
                    {
                        registerSocket.Shutdown(SocketShutdown.Both);
                    }
                    finally
                    {
                        registerSocket = null;
                    }
                }
                else
                {
                    try
                    {
                        registerSocket.Shutdown(SocketShutdown.Both);
                    }
                    finally
                    {
                        registerSocket = null;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                StopListeing();
            }
        }

        public void StartListening()
        {
            if (severSocket == null)
            {
                severSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                severSocket.Bind(new IPEndPoint(IPAddress.Any, 7777));
                severSocket.Listen(MaxConnectionNum);
            }
            listening = true;
            if(listeningThread == null || !listeningThread.IsAlive)
            {
                listeningThread = new Thread(ListeningAcept);
                listeningThread.IsBackground = true;
                listeningThread.Start();
            }
        }
		
        public void ListeningAcept()
        {
            while (listening)
            {
                try
                {
                    Socket clientSocket = severSocket.Accept();

                    if (!listening)
                    {
                        clientSocket.Send(System.Text.Encoding.UTF8.GetBytes("Not Accept"));
                        clientSocket.Close();
                        return;
                    }

                    if (curConnectionNum < maxConnectionNum)
                    {
                        int clientId = clientId_List.Count;
                        for (int i = 0; i < clientId_List.Count; i++)
                        {
                            if (clientId_List[i] != i)
                                clientId = i;
                        }
                        clientHandler_List.Insert(clientId, new ClientHandler(clientId, clientSocket));
                        clientId_List.Insert(clientId, clientId);
                        clientHandler_List[clientId].SendMessage("Id," + clientId.ToString());
                        CurConnectionNum = CurConnectionNum + 1;
                    }
                    else
                    {
                        clientSocket.Send(System.Text.Encoding.UTF8.GetBytes("Full"));
                        clientSocket.Close();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                Thread.Sleep(100);
			}
		}
        
        public bool SendMessage(int clientId, string message)
        {
            if (clientId >= clientHandler_List.Count || !clientHandler_List[clientId].Connected)
                return false;
            bool connectState = clientHandler_List[clientId].SendMessage(message);
            return connectState;
        }

        public void StopListeing()
        {
            listening = false;
            connected = false;
            try
            {
                if (registerSocket != null)
                {
                    registerSocket.Close();
                    registerSocket = null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Close()
        {
            if (listening)
                StopListeing();
            while (clientHandler_List.Count > 0)
            {
                clientHandler_List.First().Close();
                clientHandler_List.RemoveAt(0);
            }
            curConnectionNum = 0;
            clientId_List.Clear();
            if (severSocket != null)
            {
                severSocket.Close();
                severSocket = null;

            }
        }
    }
}
