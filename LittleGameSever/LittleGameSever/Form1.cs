using LittleGameSever.SeverManager;
using LittleGameSever.State;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace LittleGameSever
{
    public partial class Form1 : Form
    {
        private int updateTime = 16;
        public int UpdateTime { get => updateTime; }
        public float FUpdateTime { get => updateTime; }
        private SeverSocketManager ssm;
        private Thread gameThread;
        private System.Windows.Forms.Timer timer;
        private double fps;

        private bool playing;
        public bool Playing { get => playing; }
        PlayingState playingState;

        private DataTable propertiesDataTable;

        public Queue<string> log_List;
        

        public Form1()
        {
            InitializeComponent();
            log_List = new Queue<string>();
            playing = false;

            ssm = new SeverSocketManager(this, 4);
            
            this.fps = 0;


            this.timer = new System.Windows.Forms.Timer();
            this.timer.Interval = 30;
            this.timer.Tick += new System.EventHandler(UiUpdate);
            this.timer.Enabled = true;
            this.timer.Start();

            propertiesDataTable = new DataTable();
            propertiesDataTable.Columns.Add("Properties");
            propertiesDataTable.Columns.Add("Value");
            propertiesDataTable.PrimaryKey = new DataColumn[] { propertiesDataTable.Columns["Properties"] };

            propertiesDataTable.Rows.Add("Listening", ssm.Listening.ToString());
            propertiesDataTable.Rows.Add("FPS", 0);
            GetIpAddress();

            dataGridView1.DataSource = propertiesDataTable;
        }
        private void UiUpdate(object sender, EventArgs e)
        {
            for (int i = 0; i < 10 && log_List.Count > 0; i++)
            {
                txtBox_Log.AppendText(log_List.Dequeue());
            }
            propertiesDataTable.Rows.Find("FPS")[1] = (int)fps;
            if (!propertiesDataTable.Rows.Find("Listening")[1].Equals(ssm.Listening.ToString()))
                propertiesDataTable.Rows.Find("Listening")[1] = ssm.Listening.ToString();
            if (!playing)
            {
                if (fps != 0)
                {
                    fps = 0;
                }
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
        }

        private void Loop()
        {
            while(playing)
            {
                DateTime startTime = DateTime.Now;
                playingState.Update();
                double elapsedSeconds = (DateTime.Now - startTime).TotalSeconds;
                if (elapsedSeconds * 1000 < updateTime)
                {
                    Thread.Sleep(updateTime - (int)(elapsedSeconds * 1000));
                }
                elapsedSeconds = (DateTime.Now - startTime).TotalSeconds;
                fps = (1 / elapsedSeconds);
            }
        }

        public void GetIpAddress()
        {
            // 取得本機名稱
            String strHostName = Dns.GetHostName();

            // 取得本機的 IpHostEntry 類別實體
            IPHostEntry ipHostEntry = Dns.GetHostByName(strHostName);



            // 取得所有 IP 位址
            int num = 1;
            foreach (IPAddress ipAddress in ipHostEntry.AddressList)
            {
                propertiesDataTable.Rows.Add("Ip " + num.ToString(), ipAddress.ToString());
                num++;
            }
        }

        private void btn_StartSever_Click(object sender, EventArgs e)
        {
            ssm.StartListening();
            btn_StartSever.Enabled = false;
            btn_StopSever.Enabled = true;
            btn_StartGame.Enabled = true;
        }

        private void btn_StopSever_Click(object sender, EventArgs e)
        {
            ssm.Close();
            btn_StopSever.Enabled = false;
            btn_StartSever.Enabled = true;
            btn_StartGame.Enabled = false;
            btn_StopGame.Enabled = false;
        }

        private void button_StartGame_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void StartGame()
        {
            ssm.StopListeing();
            for (int i = 0; i < ssm.CurConnectionNum; i++)
            {
                ssm.SendMessage(i, "Start");
            }
            int count = 0;
            while(true)
            {
                bool ready = true;
                for (int i = 0; i < ssm.CurConnectionNum; i++)
                    if (!ssm.clientHandler_List[i].ReadyToStart)
                        ready = false;
                if (ready)
                    break;
                count++;
                if(count > 10000)
                {
                    for (int i = 0; i < ssm.CurConnectionNum; i++)
                    {
                        ssm.SendMessage(i, "Start");
                    }
                    count = 0;
                }
            }
            playingState = new PlayingState(this, ssm, ssm.CurConnectionNum);
            playing = true;
            gameThread = new Thread(Loop);
            gameThread.IsBackground = true;
            gameThread.Start();
            btn_StartGame.Enabled = false;
            btn_StopSever.Enabled = false;
            btn_StopGame.Enabled = true;
        }

        private void btn_StopGame_Click(object sender, EventArgs e)
        {
            StopGame();
        }

        private void StopGame()
        {
            if (playing)
            {
                playing = false;
                playingState = null;
                if(gameThread != null)
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
            btn_StopGame.Enabled = false;
            btn_StartGame.Enabled = true;
            btn_StopSever.Enabled = true;
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
