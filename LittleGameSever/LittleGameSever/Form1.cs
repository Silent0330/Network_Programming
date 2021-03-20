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
        PlayingState playingState;

        public Form1()
        {
            InitializeComponent();
            ssm = new SeverSocketManager(4, txtBox_Log);
            playing = false;
            timer = new System.Windows.Forms.Timer();
            this.timer.Interval = 1;
            this.timer.Tick += new System.EventHandler(this.loop);
            this.timer.Enabled = true;
            this.timer.Start();
        }

        private void loop(object sender, EventArgs e)
        {
            if(playing)
            {
                playingState.update();
            }
        }

        public void getIpAddress()
        {
            // 取得本機名稱
            String strHostName = Dns.GetHostName();

            // 取得本機的 IpHostEntry 類別實體
            IPHostEntry ipHostEntry = Dns.GetHostByName(strHostName);



            // 取得所有 IP 位址
            int num = 1;
            foreach (IPAddress ipAddress in ipHostEntry.AddressList)
            {
                Console.WriteLine("IP #" + num + ": " + ipAddress.ToString());
                num = num + 1;
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
        }

        private void button_StartGame_Click(object sender, EventArgs e)
        {
            playing = true;
            playingState = new PlayingState(ssm.CurConnectionNum);
            btn_StopSever.Enabled = false;
        }

    }
}
