using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LittleGame.States
{
    class MenuState : GameState
    {
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button joinButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.TextBox textBox;

        public MenuState(GameStateManager gsm)
        {
            this.gsm = gsm;
            this.startButton = new System.Windows.Forms.Button();
            this.joinButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.textBox = new System.Windows.Forms.TextBox();

            #region panel
            this.BackColor = System.Drawing.Color.LightBlue;
            this.BackgroundImage = global::LittleGame.Properties.Resources.menubg;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "menu state";
            this.Size = new System.Drawing.Size(800, 600);
            this.TabIndex = 0;
            this.Controls.Add(startButton);
            this.Controls.Add(joinButton);
            this.Controls.Add(exitButton);
            this.Controls.Add(textBox);
            #endregion

            #region textBox
            this.textBox.Location = new System.Drawing.Point(300, 200);
            this.textBox.Margin = new System.Windows.Forms.Padding(4);
            this.textBox.Name = "ip textBox";
            this.textBox.Size = new System.Drawing.Size(200, 50);
            this.textBox.TabIndex = 4;
            this.textBox.Text = "192.168.0.100";
            #endregion

            #region startButton
            this.startButton.BackColor = System.Drawing.Color.Transparent;
            this.startButton.FlatAppearance.BorderSize = 0;
            this.startButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.startButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startButton.Font = new System.Drawing.Font("標楷體", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.startButton.ForeColor = System.Drawing.Color.Black;
            this.startButton.Location = new System.Drawing.Point(350, 250);
            this.startButton.Name = "start button";
            this.startButton.Size = new System.Drawing.Size(100, 40);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "START";
            this.startButton.UseVisualStyleBackColor = false;
            this.startButton.Click += new System.EventHandler(this.start_Click);
            this.startButton.MouseLeave += new System.EventHandler(this.start_MouseLeave);
            this.startButton.MouseHover += new System.EventHandler(this.start_MouseHover);
            #endregion

            #region joinButton
            this.joinButton.BackColor = System.Drawing.Color.Transparent;
            this.joinButton.FlatAppearance.BorderSize = 0;
            this.joinButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.joinButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.joinButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.joinButton.Font = new System.Drawing.Font("標楷體", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.joinButton.ForeColor = System.Drawing.Color.Black;
            this.joinButton.Location = new System.Drawing.Point(350, 350);
            this.joinButton.Name = "join button";
            this.joinButton.Size = new System.Drawing.Size(100, 40);
            this.joinButton.TabIndex = 0;
            this.joinButton.Text = "JOIN";
            this.joinButton.UseVisualStyleBackColor = false;
            this.joinButton.Click += new System.EventHandler(this.join_Click);
            this.joinButton.MouseLeave += new System.EventHandler(this.join_MouseLeave);
            this.joinButton.MouseHover += new System.EventHandler(this.join_MouseHover);
            #endregion

            #region exitButton
            this.exitButton.BackColor = System.Drawing.Color.Transparent;
            this.exitButton.FlatAppearance.BorderSize = 0;
            this.exitButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.exitButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.exitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exitButton.Font = new System.Drawing.Font("標楷體", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.exitButton.ForeColor = System.Drawing.Color.Black;
            this.exitButton.Location = new System.Drawing.Point(350, 450);
            this.exitButton.Name = "exit button";
            this.exitButton.Size = new System.Drawing.Size(100, 40);
            this.exitButton.TabIndex = 0;
            this.exitButton.Text = "EXIT";
            this.exitButton.UseVisualStyleBackColor = false;
            this.exitButton.Click += new System.EventHandler(this.exit_Click);
            this.exitButton.MouseLeave += new System.EventHandler(this.exit_MouseLeave);
            this.exitButton.MouseHover += new System.EventHandler(this.exit_MouseHover);
            #endregion
        }

        public override void Update()
        {
            if(gsm.csm.Connected)
            {
                gsm.SetState(GameStateManager.CLIENTROOMSTATE);
            }
        }

        public override void KeyDown(KeyEventArgs e)
        {

        }

        public override void KeyUp(KeyEventArgs e)
        {

        }
        
        //start button
        private void start_Click(object sender, EventArgs e)
        {

            try
            {
                if(gsm.form.StartSever(textBox.Text, 6666))
                {
                    // 取得本機名稱
                    String strHostName = Dns.GetHostName();

                    // 取得本機的 IpHostEntry 類別實體
                    IPHostEntry ipHostEntry = Dns.GetHostByName(strHostName);

                    gsm.csm.StartConnect(ipHostEntry.AddressList.First().ToString(), 7777);
                }
            }
            catch(Exception exception)
            {

            }
        }

        private void start_MouseHover(object sender, EventArgs e)
        {
            startButton.ForeColor = System.Drawing.Color.Red;
        }

        private void start_MouseLeave(object sender, EventArgs e)
        {
            startButton.ForeColor = System.Drawing.Color.Black;
        }

        //join button
        private void join_Click(object sender, EventArgs e)
        {
            
            if(gsm.csm.GetSeverIp(textBox.Text, 6666))
            {
                gsm.SetState(GameStateManager.JOINROOMSTATE);
            }
            //gsm.csm.StartConnect(gsm.csm.SeverIps[0], 7777);

        }

        private void join_MouseHover(object sender, EventArgs e)
        {
            joinButton.ForeColor = System.Drawing.Color.Red;
        }

        private void join_MouseLeave(object sender, EventArgs e)
        {
            joinButton.ForeColor = System.Drawing.Color.Black;
        }

        //exit button
        private void exit_Click(object sender, EventArgs e)
        {
            gsm.form.Close();
        }

        private void exit_MouseHover(object sender, EventArgs e)
        {
            exitButton.ForeColor = System.Drawing.Color.Red;
        }

        private void exit_MouseLeave(object sender, EventArgs e)
        {
            exitButton.ForeColor = System.Drawing.Color.Black;
        }
        
    }
}
