using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LittleGame.States
{
    class JoinRoomState : GameState
    {
        class Room
        {
            private GameStateManager gsm;
            private FlowLayoutPanel parentPanel;
            private Panel panel;
            private Label ipLabel;
            private Button joinButton;
            private string ip;

            public Room(GameStateManager gsm, FlowLayoutPanel flowLayoutPanel, string ip)
            {
                this.gsm = gsm;
                parentPanel = flowLayoutPanel;
                this.ip = ip;
                panel = new Panel();
                ipLabel = new Label();
                joinButton = new Button();
                flowLayoutPanel.Controls.Add(panel);

                //room panel
                panel.Location = new System.Drawing.Point(25, 0);
                panel.Name = "pnael";
                panel.Size = new System.Drawing.Size(790, 100);
                panel.TabIndex = 0;
                panel.BackColor = System.Drawing.Color.White;
                panel.Visible = true;
                panel.Controls.Add(ipLabel);
                panel.Controls.Add(joinButton);

                ipLabel.AutoSize = true;
                ipLabel.BackColor = System.Drawing.Color.Transparent;
                ipLabel.Location = new System.Drawing.Point(50, 25);
                ipLabel.Name = "ip label";
                ipLabel.Size = new System.Drawing.Size(100, 50);
                ipLabel.Font = new System.Drawing.Font("標楷體", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                ipLabel.Text = ip.ToString();
                ipLabel.BringToFront();

                #region join button
                joinButton.BackColor = System.Drawing.Color.Transparent;
                joinButton.FlatAppearance.BorderSize = 0;
                joinButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
                joinButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
                joinButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                joinButton.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                joinButton.ForeColor = System.Drawing.Color.Black;
                joinButton.Location = new System.Drawing.Point(600, 0);
                joinButton.Name = "join button";
                joinButton.Size = new System.Drawing.Size(100, 100);
                joinButton.TabIndex = 0;
                joinButton.Text = "JOIN";
                joinButton.UseVisualStyleBackColor = false;
                joinButton.Click += new System.EventHandler(this.join_Click);
                joinButton.MouseLeave += new System.EventHandler(this.join_MouseLeave);
                joinButton.MouseHover += new System.EventHandler(this.join_MouseHover);
                #endregion
            }

            //start button
            private void join_Click(object sender, EventArgs e)
            {
                gsm.csm.StartConnect(ip, 7777);
            }

            private void join_MouseLeave(object sender, EventArgs e)
            {
                joinButton.ForeColor = System.Drawing.Color.Black;
            }

            private void join_MouseHover(object sender, EventArgs e)
            {
                joinButton.ForeColor = System.Drawing.Color.Red;
            }

            public void Dispose()
            {
                panel.Dispose();
            }
        }

        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.Button startButton;
        private FlowLayoutPanel roomFlowPanel;
        private List<Room> rooms;


        private static System.Drawing.Bitmap[] playerIcons =
        {
            global::LittleGame.Properties.Resources.p1down,
            global::LittleGame.Properties.Resources.p2down,
            global::LittleGame.Properties.Resources.p3down,
            global::LittleGame.Properties.Resources.p4down
        };

        public JoinRoomState(GameStateManager gsm)
        {
            this.gsm = gsm;
            backButton = new Button();
            startButton = new Button();
            roomFlowPanel = new FlowLayoutPanel();
            rooms = new List<Room>();

            //panel
            BackColor = System.Drawing.Color.LightBlue;
            //BackgroundImage = global::LittleGame.Properties.Resources.menubg;
            //BackgroundImageLayout = ImageLayout.Stretch;
            Dock = System.Windows.Forms.DockStyle.Fill;
            Location = new System.Drawing.Point(0, 0);
            Name = "room state";
            Size = new System.Drawing.Size(800, 600);
            TabIndex = 0;
            Controls.Add(startButton);
            Controls.Add(backButton);
            Controls.Add(roomFlowPanel);


            roomFlowPanel.AutoScroll = true;
            roomFlowPanel.Location = new System.Drawing.Point(0, 100);
            roomFlowPanel.Name = "flowLayoutPanel1";
            roomFlowPanel.Size = new System.Drawing.Size(800, 400);
            roomFlowPanel.TabIndex = 0;

            #region back button
            this.backButton.BackColor = System.Drawing.Color.Transparent;
            this.backButton.FlatAppearance.BorderSize = 0;
            this.backButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.backButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.backButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.backButton.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.backButton.ForeColor = System.Drawing.Color.Black;
            this.backButton.Location = new System.Drawing.Point(0, 550);
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
            this.startButton.Location = new System.Drawing.Point(650, 550);
            this.startButton.Name = "start button";
            this.startButton.Size = new System.Drawing.Size(100, 40);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "START";
            this.startButton.UseVisualStyleBackColor = false;
            this.startButton.Click += new System.EventHandler(this.start_Click);
            this.startButton.MouseLeave += new System.EventHandler(this.start_MouseLeave);
            this.startButton.MouseHover += new System.EventHandler(this.start_MouseHover);
            #endregion

            SetRooms(gsm.csm.SeverIps);
        }

        public void SetRooms(string[] ips)
        {
            for(int i = 0; i < rooms.Count; i++)
            {
                rooms[i].Dispose();
            }
            rooms.Clear();
            for (int i = 0; i < ips.Length; i++)
            {
                rooms.Add(new Room(gsm, roomFlowPanel, ips[i]));
            }

        }

        public override void KeyDown(KeyEventArgs e)
        {

        }

        public override void KeyUp(KeyEventArgs e)
        {

        }

        public override void Update()
        {
            if (gsm.csm.Connected)
            {
                gsm.SetState(GameStateManager.CLIENTROOMSTATE);
            }
        }

        //back button
        private void back_Click(object sender, EventArgs e)
        {
            gsm.form.StopSever();
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
