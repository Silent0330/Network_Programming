using LittleGame.SeverManager;
using LittleGame.State;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LittleGameSever
{
    public partial class Form1 : Form
    {
        private SeverSocketManager ssm;
        private Timer timer;

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

            timer = new System.Windows.Forms.Timer();
            this.timer.Interval = 1;
            this.timer.Tick += new System.EventHandler(this.loop);
            this.timer.Enabled = true;
            this.timer.Start();

            propertiesDataTable = new DataTable();
            propertiesDataTable.Columns.Add("Properties");
            propertiesDataTable.Columns.Add("Value");

            propertiesDataTable.Rows.Add("Listening", ssm.Listening.ToString());
            GetIpAddress();

            dataGridView1.DataSource = propertiesDataTable;
        }

        private void loop(object sender, EventArgs e)
        {
            for(int i = 0; i < 10 && log_List.Count > 0; i++)
            {
                txtBox_Log.AppendText(log_List.Dequeue());
            }
            if(playing)
            {
                playingState.Update();
                if(playingState.GameOver)
                {
                    StopGame();
                }
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
                propertiesDataTable.Rows.Add("Ip", ipAddress.ToString());
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
            playingState = new PlayingState(ssm, ssm.CurConnectionNum);
            playing = true;
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
                playingState = null;
                playing = false;
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

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
