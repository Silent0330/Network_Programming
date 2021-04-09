using LittleGame.Sever;
using LittleGame.State;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
               
        }
        
        private void loop(object sender, EventArgs e)
        {
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
