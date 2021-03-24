using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LittleGame.State
{
    class ClientRoomState : State.GameState
    {
        private int playerNum;
        private int playerId;
        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Panel[] subpanels;
        private System.Windows.Forms.PictureBox[] plaIcon;
        private System.Windows.Forms.Label[] playerInfoLabels;
        

        private static System.Drawing.Bitmap[] playerIcons =
        {
            global::LittleGame.Properties.Resources.p1down,
            global::LittleGame.Properties.Resources.p2down,
            global::LittleGame.Properties.Resources.p3down,
            global::LittleGame.Properties.Resources.p4down
        };

        public ClientRoomState(GameStateManager gsm)
        {
            this.gsm = gsm;
            this.backButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.playerInfoLabels = new System.Windows.Forms.Label[4];
            this.subpanels = new System.Windows.Forms.Panel[4];
            this.plaIcon = new System.Windows.Forms.PictureBox[4];
            this.playerId = -1;
            this.playerNum = 0;


            //panel
            this.BackColor = System.Drawing.Color.LightBlue;
            this.BackgroundImage = global::LittleGame.Properties.Resources.menubg;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "room state";
            this.Size = new System.Drawing.Size(800, 600);
            this.TabIndex = 0;
            this.Controls.Add(backButton);

            for (int i = 0; i < 4; i++)
            {
                this.subpanels[i] = new System.Windows.Forms.Panel();
                this.subpanels[i].Location = new System.Drawing.Point(0, i * 100 + 25);
                this.subpanels[i].Name = "Player" + i.ToString();
                this.subpanels[i].Size = new System.Drawing.Size(200, 100);
                this.subpanels[i].TabIndex = 0;
                this.subpanels[i].Visible = false;
                this.subpanels[i].BringToFront();
                this.Controls.Add(this.subpanels[i]);
                plaIcon[i] = new System.Windows.Forms.PictureBox();
                plaIcon[i].BackColor = System.Drawing.Color.Transparent;
                plaIcon[i].Location = new System.Drawing.Point(100, 15);
                plaIcon[i].Name = "player" + (i + 1).ToString();
                plaIcon[i].Size = new System.Drawing.Size(70, 70);
                plaIcon[i].SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                plaIcon[i].TabIndex = 0;
                plaIcon[i].TabStop = false;
                plaIcon[i].BringToFront();
                this.subpanels[i].Controls.Add(plaIcon[i]);
                this.playerInfoLabels[i] = new System.Windows.Forms.Label();
                this.playerInfoLabels[i].AutoSize = true;
                this.playerInfoLabels[i].BackColor = System.Drawing.Color.Transparent;
                this.playerInfoLabels[i].Location = new System.Drawing.Point(0, i * 100 + 50);
                this.playerInfoLabels[i].Name = "player info";
                this.playerInfoLabels[i].Size = new System.Drawing.Size(800, 100);
                this.playerInfoLabels[i].Font = new System.Drawing.Font("標楷體", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                this.playerInfoLabels[i].Text = "P" + (i + 1).ToString();
                this.Controls.Add(this.playerInfoLabels[i]);
                this.playerInfoLabels[i].BringToFront();
            }

            #region back button
            this.backButton.BackColor = System.Drawing.Color.Transparent;
            this.backButton.FlatAppearance.BorderSize = 0;
            this.backButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.backButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.backButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.backButton.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.backButton.ForeColor = System.Drawing.Color.Black;
            this.backButton.Location = new System.Drawing.Point(0, 500);
            this.backButton.Name = "back button";
            this.backButton.Size = new System.Drawing.Size(100, 40);
            this.backButton.TabIndex = 0;
            this.backButton.Text = "BACK";
            this.backButton.UseVisualStyleBackColor = false;
            this.backButton.Click += new System.EventHandler(this.back_Click);
            this.backButton.MouseLeave += new System.EventHandler(this.back_MouseLeave);
            this.backButton.MouseHover += new System.EventHandler(this.back_MouseHover);
            #endregion

            #region start button
            this.startButton.BackColor = System.Drawing.Color.Transparent;
            this.startButton.FlatAppearance.BorderSize = 0;
            this.startButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.startButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startButton.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.startButton.ForeColor = System.Drawing.Color.Black;
            this.startButton.Location = new System.Drawing.Point(650, 500);
            this.startButton.Name = "start button";
            this.startButton.Size = new System.Drawing.Size(100, 40);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "START";
            this.startButton.UseVisualStyleBackColor = false;
            this.startButton.Click += new System.EventHandler(this.start_Click);
            this.startButton.MouseLeave += new System.EventHandler(this.start_MouseLeave);
            this.startButton.MouseHover += new System.EventHandler(this.start_MouseHover);
            #endregion
        }

        public override void KeyDown(KeyEventArgs e)
        {

        }

        public override void KeyUp(KeyEventArgs e)
        {

        }

        public override void Update()
        {
            if (!gsm.csm.Connected)
            {
                gsm.SetState(GameStateManager.MENUSTATE);
            }
            if (gsm.csm.GameStart)
            {
                gsm.SetState(GameStateManager.CLIENTPLAYINGSTATE);
            }

            bool refresh = false;
            if (playerId != gsm.csm.PlayerId)
            {
                playerId = gsm.csm.PlayerId;
                refresh = true;
            }
            if (playerNum != gsm.csm.PlayerNum)
            {
                playerNum = gsm.csm.PlayerNum;
                refresh = true;
            }

            if (refresh)
            {
                if(playerId == 0)
                {
                    this.Controls.Add(startButton);
                }
                else
                {
                    this.Controls.Remove(startButton);
                }
                for (int i = 0; i < 4; i++)
                {
                    if (i == playerId)
                    {
                        playerInfoLabels[i].ForeColor = System.Drawing.Color.Red;
                        plaIcon[i].Image = playerIcons[i];
                        this.subpanels[i].BackColor = System.Drawing.Color.FromArgb(80, 255, 0, 0);
                        if (this.playerInfoLabels[i].Parent != this.subpanels[i])
                        {
                            this.Controls.Remove(this.playerInfoLabels[i]);
                            this.subpanels[i].Controls.Add(this.playerInfoLabels[i]);
                            this.playerInfoLabels[i].Location = new System.Drawing.Point(0, 25);
                        }
                        this.subpanels[i].Visible = true;
                    }
                    else if (i < playerNum)
                    {
                        playerInfoLabels[i].ForeColor = System.Drawing.Color.Green;
                        plaIcon[i].Image = playerIcons[i];
                        this.subpanels[i].BackColor = System.Drawing.Color.FromArgb(80, 0, 255, 0);
                        if (this.playerInfoLabels[i].Parent != this.subpanels[i])
                        {
                            this.Controls.Remove(this.playerInfoLabels[i]);
                            this.subpanels[i].Controls.Add(this.playerInfoLabels[i]);
                            this.playerInfoLabels[i].Location = new System.Drawing.Point(0, 25);
                        }
                        this.subpanels[i].Visible = true;
                    }
                    else
                    {
                        playerInfoLabels[i].ForeColor = System.Drawing.Color.Black;
                        plaIcon[i].Image = null;
                        if (this.playerInfoLabels[i].Parent == this.subpanels[i])
                        {
                            this.subpanels[i].Controls.Remove(this.playerInfoLabels[i]);
                            this.Controls.Add(this.playerInfoLabels[i]);
                            this.playerInfoLabels[i].Location = new System.Drawing.Point(0, i * 100 + 50);
                        }
                        this.subpanels[i].Visible = false;
                    }
                }
            }
            
        }

        //back button
        private void back_Click(object sender, EventArgs e)
        {
            gsm.csm.CloseConnection();
            gsm.SetState(GameStateManager.MENUSTATE);
        }

        private void back_MouseHover(object sender, EventArgs e)
        {
            backButton.ForeColor = System.Drawing.Color.Red;
        }

        private void back_MouseLeave(object sender, EventArgs e)
        {
            backButton.ForeColor = System.Drawing.Color.Black;
        }

        //start button
        private void start_Click(object sender, EventArgs e)
        {
            gsm.csm.SendMessage("StartGame");
        }

        private void start_MouseHover(object sender, EventArgs e)
        {
            startButton.ForeColor = System.Drawing.Color.Red;
        }

        private void start_MouseLeave(object sender, EventArgs e)
        {
            startButton.ForeColor = System.Drawing.Color.Black;
        }
    }
}
