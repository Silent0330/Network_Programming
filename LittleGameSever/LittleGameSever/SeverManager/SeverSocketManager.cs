using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Timers;
using System.Drawing;
using LittleGameSever.SeverManager;
using System.Windows.Forms;
using LittleGameSever;

namespace LittleGame.SeverManager
{
    class SeverSocketManager
    {
        #region declare
        Form1 form;
		private Socket severSocket;
		public List<ClientHandler> clientHandler_List;
        private List<int> clientId_List;

        //Thread
        private Thread listeningThread;
        private List<Thread> recvThread_List;
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
            recvThread_List = new List<Thread>();
            
            curConnectionNum = 0;

            form.log_List.Enqueue("Sever is ready for start" + Environment.NewLine);

        }

        public void CheckingConnection(object sender, ElapsedEventArgs e)
        {
            for (int i = clientHandler_List.Count-1; i >= 0; i--)
            {
                if (!clientHandler_List[i].Connected)
                {
                    try
                    {
                        clientHandler_List[i].Close();
                        clientHandler_List.RemoveAt(i);
                        clientId_List.RemoveAt(i);
                        if(!form.Playing)
                        {
                            for(int j = i; j < clientHandler_List.Count; j++)
                            {
                                clientHandler_List[j].ChangeId(j);
                                clientId_List[j] = j;
                            }
                        }
                        CurConnectionNum = curConnectionNum - 1;
                        break;
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.ToString());
                    }
                }
            }
        }

        public void StartListening()
        {
            if(severSocket == null)
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
            form.log_List.Enqueue("Sever is listening" + Environment.NewLine);
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
                        form.log_List.Enqueue("new client is accept #id : " + IPAddress.Parse(((IPEndPoint)clientSocket.RemoteEndPoint).Address.ToString()) + "  #port : " + ((IPEndPoint)clientSocket.RemoteEndPoint).Port.ToString() + Environment.NewLine);
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

        private void RecvMessage(int clientNumber)
        {

        }
        
        public bool SendMessage(int clientNumber, string message)
        {
            bool connectState = clientHandler_List[clientNumber].SendMessage(message);
            return connectState;
        }

        public void StopListeing()
        {
            listening = false;
            form.log_List.Enqueue("Sever is stop listening" + Environment.NewLine);
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
            clientId_List.Clear();
            if (severSocket != null)
            {
                severSocket.Close();
                severSocket = null;

            }
        }
    }
}
