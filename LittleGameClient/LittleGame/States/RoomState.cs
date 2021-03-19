using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LittleGame.State
{
    class RoomState : GameState
    {
        private int playerNum;
        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Label[] playerInfoLabels;
        private System.Windows.Forms.Panel[] subpanels;
        private System.Windows.Forms.PictureBox[] plaIcon;
        
        
        public RoomState(GameStateManager gsm)
        {
            this.gsm = gsm;
            this.backButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.playerInfoLabels = new System.Windows.Forms.Label[4];
            this.subpanels = new System.Windows.Forms.Panel[4];
            this.plaIcon = new System.Windows.Forms.PictureBox[4];

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
            this.Controls.Add(startButton);

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
                this.playerInfoLabels[i].Location = new System.Drawing.Point(0, 50 + i * 100);
                this.playerInfoLabels[i].Name = "player info";
                this.playerInfoLabels[i].Size = new System.Drawing.Size(100, 100);
                this.playerInfoLabels[i].Font = new System.Drawing.Font("標楷體", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                this.playerInfoLabels[i].Text = "P" + (i + 1).ToString();
                this.Controls.Add(this.playerInfoLabels[i]);
                this.playerInfoLabels[i].BringToFront();

            }

            //back button
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

            //start button
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
        }

        public override void update()
        {

        }

        public override void keyDown(KeyEventArgs e)
        {

        }

        public override void keyUp(KeyEventArgs e)
        {

        }

        //back button
        private void back_Click(object sender, EventArgs e)
        {
            gsm.setState(GameStateManager.MENUSTATE);
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
            gsm.setState(GameStateManager.PLAYINGSTATE);
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
