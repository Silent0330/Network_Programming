﻿using System;
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
        public Socket Sockeet { get => socket; }
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

            up = down = left = right = false;

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
                    up = bool.Parse(messages[1]);
                }
                else if (messages[0].Equals("Down"))
                {
                    down = bool.Parse(messages[1]); ;
                }
                else if (messages[0].Equals("Left"))
                {
                    left = bool.Parse(messages[1]);
                }
                else if (messages[0].Equals("Right"))
                {
                    right = bool.Parse(messages[1]);
                }
                else if (messages[0].Equals("Ready"))
                {
                    readyToStart = bool.Parse(messages[1]);
                }
            }
        }

        public bool SendMessage(string message)
        {
            message += ",";
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

        public void CheckConnection()
        {
            bool blockingState = socket.Blocking;
            try
            {
                byte[] tmp = new byte[1];

                socket.Blocking = false;
                socket.Send(tmp, 0, 0);
            }
            catch (SocketException e)
            {
                // 10035 == WSAEWOULDBLOCK
                if (e.NativeErrorCode.Equals(10035))
                {
                    Console.WriteLine("Still Connected, but the Send would block");
                }
                else
                {
                    connected = false;
                    Console.WriteLine("Disconnected: error code {0}!", e.NativeErrorCode);
                }
            }
            finally
            {
                socket.Blocking = blockingState;
            }
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
