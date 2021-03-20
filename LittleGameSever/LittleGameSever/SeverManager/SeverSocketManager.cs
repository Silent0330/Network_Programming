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

namespace LittleGame.SeverManager
{
    class SeverSocketManager
    {
		#region declare
		private Socket severSocket;
		private List<ClientHandler> clientHandler_List;
        private List<int> clientNumber_List;

        private TextBox logTextBox;

        //Thread
        private Thread listeningThread;
        private List<Thread> recvThread_List;

		//Connection Properties
		private int maxConnectionNum;
		public int MaxConnectionNum{
			get { return maxConnectionNum; }
		}
		private int curConnectionNum;
		public int CurConnectionNum{
			get { return curConnectionNum; }
		}
		
        private bool listening;
        #endregion

        public SeverSocketManager(int maxConnectionNum, TextBox textBox)
        {
            logTextBox = textBox;
                
            if (maxConnectionNum > 0)
            {
                this.maxConnectionNum = maxConnectionNum;
            }
            else
            {
                maxConnectionNum = 0;
            }
            


            clientHandler_List = new List<ClientHandler>();
            clientNumber_List = new List<int>();
            recvThread_List = new List<Thread>();
            
            curConnectionNum = 0;

            logTextBox.AppendText("Sever is ready for start" + Environment.NewLine);

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
                listeningThread = new Thread(Listening);
                listeningThread.IsBackground = true;
                listeningThread.Start();
            }
            logTextBox.AppendText("Sever is listening" + Environment.NewLine);
        }
		
        public void Listening()
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
                        int clientNumber = clientNumber_List.Count;
                        for (int i = 0; i < clientNumber_List.Count; i++)
                        {
                            if (clientNumber_List[i] != i)
                                clientNumber = i;
                        }
                        clientHandler_List.Insert(clientNumber, new ClientHandler(clientNumber, clientSocket));
                        logTextBox.AppendText("new client is accept #id : " + IPAddress.Parse(((IPEndPoint)clientSocket.RemoteEndPoint).Address.ToString()) + "  #port : " + ((IPEndPoint)clientSocket.RemoteEndPoint).Port.ToString() + Environment.NewLine);
                        curConnectionNum++;
                    }
                    else
                    {
                        clientSocket.Send(System.Text.Encoding.UTF8.GetBytes("Full"));
                        clientSocket.Close();
                    }
                }
                catch
                {

                }
                Thread.Sleep(100);
			}
		}
        
        private int FindIndByClientNumber(int clientNumber)
        {
            int ind = (clientNumber_List.Count-1) * clientNumber / clientNumber_List.Last();
            while(clientNumber_List[ind] != clientNumber)
            {
                if(clientNumber_List[ind] < clientNumber)
                {
                    ind = ind + 1;
                }
                else if (clientNumber_List[ind] > clientNumber)
                {
                    ind = ind - 1;
                }
            }
            return ind;
        }

        private void RecvMessage(int clientNumber)
        {
            int ind = FindIndByClientNumber(clientNumber);
        }
        
        public bool SendMessage(int clientNumber, string message)
        {
            int ind = FindIndByClientNumber(clientNumber);
            bool connectState = clientHandler_List[ind].SendMessage(message);
            return connectState;
        }

        public void StopListeing()
        {
            listening = false;
            logTextBox.AppendText("Sever is stop listening" + Environment.NewLine);
        }

        public void Close()
        {
            if (listening)
                StopListeing();
            for (int i = 0; i < clientHandler_List.Count; i++)
            {
                clientHandler_List[i].Close();
            }
            if (severSocket != null)
            {
                severSocket.Close();
                severSocket = null;

            }
        }
    }
}
