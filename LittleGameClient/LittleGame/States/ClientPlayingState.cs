using LittleGame.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LittleGame.State
{
    class ClientPlayingState : GameState
    {
        public ClientPlayer[] players;
        public int playerNum;
        public bool gameOver;
        private System.Windows.Forms.Label winnerLabel;

        private static System.Drawing.Point[] playerPoints =
        {
            new System.Drawing.Point(50,50),
            new System.Drawing.Point(700,50),
            new System.Drawing.Point(50,500),
            new System.Drawing.Point(700,500)
        };

        //move state
        private bool up;
        private bool down;
        private bool left;
        private bool right;
        private bool attack;

        public bool getUp() { return up; }
        public bool getDown() { return down; }
        public bool getLeft() { return left; }
        public bool getRight() { return right; }
        public bool getAttack() { return attack; }

        private static System.Drawing.Bitmap[] images =
        {
            global::LittleGame.Properties.Resources.Map1_1
        };

        public ClientPlayingState(State.GameStateManager gsm)
        {
            this.gsm = gsm;
            this.playerNum = gsm.csm.PlayerNum;
            players = new Client.ClientPlayer[4];
            gameOver = false;

            //panel
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "playing state";
            this.Size = new System.Drawing.Size(800, 600);
            this.TabIndex = 0;
            this.BackColor = System.Drawing.Color.LightBlue;
            this.BackgroundImage = images[0];
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;

            for (int i = 0; i < playerNum; i++)
            {
                players[i] = new ClientPlayer(this, i, playerPoints[i].X, playerPoints[i].Y);
            }

            //winner label
            this.winnerLabel = new System.Windows.Forms.Label();
            this.winnerLabel.AutoSize = true;
            this.winnerLabel.BackColor = System.Drawing.Color.Transparent;
            this.winnerLabel.Location = new System.Drawing.Point(250, 250);
            this.winnerLabel.Name = "winner";
            this.winnerLabel.Size = new System.Drawing.Size(200, 100);
            this.winnerLabel.Font = new System.Drawing.Font("標楷體", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.winnerLabel.Text = "P1 is winner";

            gsm.csm.state = this;
        }
        
        public override void KeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W && !up)
            {
                up = true;
                gsm.csm.SendMessage("Up," + up.ToString());
            }
            if (e.KeyCode == Keys.S && !down)
            {
                down = true;
                gsm.csm.SendMessage("Down," + down.ToString());
            }
            if (e.KeyCode == Keys.A && !left)
            {
                left = true;
                gsm.csm.SendMessage("Left," + left.ToString());
            }
            if (e.KeyCode == Keys.D && !right)
            {
                right = true;
                gsm.csm.SendMessage("Right," + right.ToString());
            }
            if (e.KeyCode == Keys.Space && !attack)
            {
                attack = true;
                gsm.csm.SendMessage("Attack," + attack.ToString());
            }

            if (gameOver && e.KeyCode == Keys.Space)
            {
                gsm.csm.CloseConnection();
                gsm.SetState(GameStateManager.MENUSTATE);
            }
        }

        public override void KeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W && up)
            {
                up = false;
                gsm.csm.SendMessage("Up," + up.ToString());
            }
            if (e.KeyCode == Keys.S && down)
            {
                down = false;
                gsm.csm.SendMessage("Down," + down.ToString());
            }
            if (e.KeyCode == Keys.A && left)
            {
                left = false;
                gsm.csm.SendMessage("Left," + left.ToString());
            }
            if (e.KeyCode == Keys.D && right)
            {
                right = false;
                gsm.csm.SendMessage("Right," + right.ToString());
            }
            if (e.KeyCode == Keys.Space && attack)
            {
                attack = false;
                gsm.csm.SendMessage("Attack," + attack.ToString());
            }
        }

        public override void Update()
        {
            if (!gsm.csm.Connected)
            {
                gsm.SetState(GameStateManager.MENUSTATE);
            }
            if (!gameOver)
            {
                for (int i = 0; i < playerNum; i++)
                {
                    players[i].Update();
                }
            }
            if (gameOver)
            {
                int winner = 0;
                for (int i = 0; i < playerNum; i++)
                {
                    if (players[i].Alive)
                    {
                        winner = i + 1;
                    }
                }
                if(winner != 0)
                    winnerLabel.Text = "P" + winner.ToString() + " is winner";
                else
                    winnerLabel.Text = "TIE";
                this.Controls.Add(winnerLabel);
                winnerLabel.BringToFront();
            }
        }

    }
}
