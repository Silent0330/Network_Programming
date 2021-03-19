using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LittleGameV1._1.State
{
    class RoomState : GameState
    {
        private int playerNum;
        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Label mapLabel;
        private System.Windows.Forms.Label[] playerInfoLabels;
        private System.Windows.Forms.Panel[] subpanels;
        private System.Windows.Forms.PictureBox[] plaIcon;
        private System.Windows.Forms.PictureBox mapPictureBox;
        private System.Windows.Forms.Button leftChooseButton;
        private System.Windows.Forms.Button rightChooseButton;

        private int map;
        public const int MAXMAPNUM = 4;


        private static System.Drawing.Bitmap[] playerIcons =
        {
            global::LittleGameV1._1.Properties.Resources.p1down,
            global::LittleGameV1._1.Properties.Resources.p2down,
            global::LittleGameV1._1.Properties.Resources.p3down,
            global::LittleGameV1._1.Properties.Resources.p4down
        };

        private static System.Drawing.Bitmap[] buttonImages =
        {
            global::LittleGameV1._1.Properties.Resources.LeftButton_Blue,
            global::LittleGameV1._1.Properties.Resources.LeftButton_Red,
            global::LittleGameV1._1.Properties.Resources.LeftButton_Gray,
            global::LittleGameV1._1.Properties.Resources.RightButton_Blue,
            global::LittleGameV1._1.Properties.Resources.RightButton_Red,
            global::LittleGameV1._1.Properties.Resources.RightButton_Gray
        };

        private static System.Drawing.Bitmap[] images =
        {
            global::LittleGameV1._1.Properties.Resources.Map1_1,
            global::LittleGameV1._1.Properties.Resources.Map1_2,
            global::LittleGameV1._1.Properties.Resources.Map1_3,
            global::LittleGameV1._1.Properties.Resources.Map1_4
        };

        public RoomState(GameStateManager gsm)
        {
            this.gsm = gsm;
            this.backButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.playerInfoLabels = new System.Windows.Forms.Label[4];
            this.subpanels = new System.Windows.Forms.Panel[4];
            this.plaIcon = new System.Windows.Forms.PictureBox[4];
            this.mapPictureBox = new System.Windows.Forms.PictureBox();
            this.leftChooseButton = new System.Windows.Forms.Button();
            this.rightChooseButton = new System.Windows.Forms.Button();

            map = 0;
            this.gsm.ssm.map = map;

            //panel
            this.BackColor = System.Drawing.Color.LightBlue;
            this.BackgroundImage = global::LittleGameV1._1.Properties.Resources.Menubg1;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "room state";
            this.Size = new System.Drawing.Size(800, 600);
            this.TabIndex = 0;
            this.Controls.Add(backButton);
            this.Controls.Add(startButton);
            this.Controls.Add(mapPictureBox);
            this.Controls.Add(leftChooseButton);
            this.Controls.Add(rightChooseButton);

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

            //map pictureBox
            this.mapPictureBox.Image = images[0];
            this.mapPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.mapPictureBox.Location = new System.Drawing.Point(250, 50);
            this.mapPictureBox.Name = "pictureBox";
            this.mapPictureBox.Size = new System.Drawing.Size(480, 360);
            this.mapPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.mapPictureBox.TabIndex = 0;
            this.mapPictureBox.TabStop = false;

            //map label
            this.mapLabel = new System.Windows.Forms.Label();
            this.mapLabel.AutoSize = true;
            this.mapLabel.BackColor = System.Drawing.Color.Transparent;
            this.mapLabel.Location = new System.Drawing.Point(430, 430);
            this.mapLabel.Name = "player info";
            this.mapLabel.Size = new System.Drawing.Size(800, 100);
            this.mapLabel.Font = new System.Drawing.Font("標楷體", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.mapLabel.Text = "Map " + (this.gsm.ssm.map + 1).ToString();
            this.Controls.Add(this.mapLabel);
            this.mapLabel.BringToFront();

            //left choose button
            this.leftChooseButton.BackColor = System.Drawing.Color.Transparent;
            this.leftChooseButton.BackgroundImage = buttonImages[0];
            this.leftChooseButton.BackgroundImageLayout = ImageLayout.Stretch;
            this.leftChooseButton.FlatAppearance.BorderSize = 0;
            this.leftChooseButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.leftChooseButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.leftChooseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.leftChooseButton.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.leftChooseButton.ForeColor = System.Drawing.Color.Black;
            this.leftChooseButton.Location = new System.Drawing.Point(250, 420);
            this.leftChooseButton.Name = "left choose button";
            this.leftChooseButton.Size = new System.Drawing.Size(30, 60);
            this.leftChooseButton.TabIndex = 0;
            this.leftChooseButton.Text = "";
            this.leftChooseButton.UseVisualStyleBackColor = false;
            this.leftChooseButton.Click += new System.EventHandler(this.left_Click);
            this.leftChooseButton.MouseLeave += new System.EventHandler(this.left_MouseLeave);
            this.leftChooseButton.MouseHover += new System.EventHandler(this.left_MouseHover);

            //right choose button
            this.rightChooseButton.BackColor = System.Drawing.Color.Transparent;
            this.rightChooseButton.BackgroundImage = buttonImages[3];
            this.rightChooseButton.BackgroundImageLayout = ImageLayout.Stretch;
            this.rightChooseButton.FlatAppearance.BorderSize = 0;
            this.rightChooseButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.rightChooseButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.rightChooseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rightChooseButton.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rightChooseButton.ForeColor = System.Drawing.Color.Black;
            this.rightChooseButton.Location = new System.Drawing.Point(700, 420);
            this.rightChooseButton.Name = "right choose button";
            this.rightChooseButton.Size = new System.Drawing.Size(30, 60);
            this.rightChooseButton.TabIndex = 0;
            this.rightChooseButton.Text = "";
            this.rightChooseButton.UseVisualStyleBackColor = false;
            this.rightChooseButton.Click += new System.EventHandler(this.right_Click);
            this.rightChooseButton.MouseLeave += new System.EventHandler(this.right_MouseLeave);
            this.rightChooseButton.MouseHover += new System.EventHandler(this.right_MouseHover);

            gsm.ssm.startConnect();

        }

        public override void update()
        {
            playerNum = this.gsm.ssm.getClientPlayerNum() + 1;
            for (int i = 0; i < 4; i++)
            {
                if (i == 0)
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
            this.gsm.ssm.map = map;
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
            gsm.ssm.stopConnect();
            gsm.ssm.closeConnection();
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
            gsm.ssm.stopConnect();
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

        //left button
        private void left_Click(object sender, EventArgs e)
        {
            if (map > 0)
            {
                map--;
                mapPictureBox.Image = images[map];
                mapLabel.Text = "Map " + (map + 1).ToString();
            }
        }

        private void left_MouseHover(object sender, EventArgs e)
        {
            if (map > 0)
                leftChooseButton.BackgroundImage = buttonImages[1];
            else
                leftChooseButton.BackgroundImage = buttonImages[2];
        }

        private void left_MouseLeave(object sender, EventArgs e)
        {
            leftChooseButton.BackgroundImage = buttonImages[0];
        }

        //right button
        private void right_Click(object sender, EventArgs e)
        {
            if (map < MAXMAPNUM - 1)
            {
                map++;
                mapPictureBox.Image = images[map];
                mapLabel.Text = "Map " + (map + 1).ToString();
            }
        }

        private void right_MouseHover(object sender, EventArgs e)
        {
            if (map < MAXMAPNUM - 1)
                rightChooseButton.BackgroundImage = buttonImages[4];
            else
                rightChooseButton.BackgroundImage = buttonImages[5];
        }

        private void right_MouseLeave(object sender, EventArgs e)
        {
            rightChooseButton.BackgroundImage = buttonImages[3];
        }

    }
}
