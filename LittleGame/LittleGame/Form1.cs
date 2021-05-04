using LittleGame.Client;
using LittleGame.State;
using LittleGameSever.SeverManager;
using LittleGameSever.State;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LittleGame
{
    public partial class Form1 : Form
    {
        private int updateTime = 16;
        public int UpdateTime { get => updateTime; }
        public float FUpdateTime { get => updateTime; }

        private System.Windows.Forms.Timer timer;
        private GameStateManager gsm;
        private ClientSocketManager csm;

        private SeverSocketManager ssm;
        private Thread gameThread;
        private bool playing;
        public bool Playing { get => playing; }
        private PlayingState playingState;

        public Form1()
        {
            InitializeComponent();
            csm = new ClientSocketManager();
            gsm = new GameStateManager(this, csm);

            timer = new System.Windows.Forms.Timer();
            this.timer.Interval = updateTime;
            this.timer.Tick += new System.EventHandler(this.loop);
            this.timer.Enabled = true;
            this.timer.Start();

            playing = false;

            ssm = new SeverSocketManager(this, 4);

        }

        private void SeverLoop()
        {
            while (playing)
            {
                DateTime startTime = DateTime.Now;
                playingState.Update();
                double elapsedSeconds = (DateTime.Now - startTime).TotalSeconds;
                if (elapsedSeconds * 1000 < updateTime)
                {
                    Thread.Sleep(updateTime - (int)(elapsedSeconds * 1000));
                }
                elapsedSeconds = (DateTime.Now - startTime).TotalSeconds;
            }
        }

        public bool StartSever(string IP, int port)
        {
            ssm.StartConnect(IP, port);
            if (!ssm.Connected) return false;
            ssm.StartListening();
            return true;
        }

        public void StopSever()
        {
            ssm.Close();
        }

        private void StartGame()
        {
            ssm.StopListeing();
            for (int i = 0; i < ssm.CurConnectionNum; i++)
            {
                ssm.SendMessage(i, "Start");
            }
            playingState = new PlayingState(this, ssm, ssm.CurConnectionNum);
            playing = true;
            gameThread = new Thread(SeverLoop);
            gameThread.IsBackground = true;
            gameThread.Start();
        }

        private void StopGame()
        {
            if (playing)
            {
                playing = false;
                playingState = null;
                if (gameThread != null)
                {
                    gameThread.Abort();
                    gameThread = null;
                }
                for (int i = 0; i < ssm.CurConnectionNum; i++)
                {
                    ssm.SendMessage(i, "GameOver");
                }
            }
            ssm.StartListening();
        }

        private void loop(object sender, EventArgs e)
        {
            ssm.CheckingConnection();
            if (!playing)
            {
                if (ssm.CurConnectionNum > 0 && ssm.clientHandler_List[0].StartGameRequest)
                {
                    if (ssm.CurConnectionNum > 1)
                    {
                        playing = true;
                        StartGame();
                    }
                    ssm.clientHandler_List[0].StartGameRequest = false;
                }
            }
            else
            {
                if (playingState.GameOver)
                {
                    StopGame();
                }
            }
            gsm.Update();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            gsm.KeyDown(e);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            gsm.KeyUp(e);
        }

    }
}
