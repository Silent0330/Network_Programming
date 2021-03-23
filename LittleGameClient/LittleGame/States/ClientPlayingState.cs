﻿using LittleGame.Entity;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LittleGame.State
{
    class ClientPlayingState : GameState
    {
        public ClientPlayer[] players;
        public List<ClientBullet> clientBullets_List;
        public List<string> recivedCommand_List;
        public int playerNum;
        public bool gameOver;
        private System.Windows.Forms.Label winnerLabel;
        private System.Windows.Forms.Label bulletcountLabel;

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
        private bool reload;

        public bool Up { get => up; }
        public bool Down { get => down; }
        public bool Left { get => left; }
        public bool Right { get => right; }
        public bool Attack { get => attack; }
        public bool Reload { get => reload; }

        private static System.Drawing.Bitmap[] images =
        {
            Properties.Resources.Map1_1
        };

        public ClientPlayingState(State.GameStateManager gsm)
        {
            this.gsm = gsm;
            this.playerNum = gsm.csm.PlayerNum;
            players = new ClientPlayer[4];
            clientBullets_List = new List<ClientBullet>();
            recivedCommand_List = new List<string>();
            gameOver = false;
            up = down = left = right = attack = reload = false;

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

            //bulletcount lable
            this.bulletcountLabel = new System.Windows.Forms.Label();
            this.bulletcountLabel.AutoSize = true;
            this.bulletcountLabel.BackColor = System.Drawing.Color.Transparent;
            this.bulletcountLabel.Location = new System.Drawing.Point(700, 0);
            this.bulletcountLabel.Size = new System.Drawing.Size(100, 100);
            this.bulletcountLabel.Font = new System.Drawing.Font("標楷體", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));

            this.Controls.Add(bulletcountLabel);
            bulletcountLabel.BringToFront();

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
            if (e.KeyCode == Keys.R && !reload)
            {
                reload = true;
                gsm.csm.SendMessage("Reload," + reload.ToString());
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
            if (e.KeyCode == Keys.R && reload)
            {
                reload = false;
                gsm.csm.SendMessage("Reload," + reload.ToString());
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
                for(int i = 0; i < 10 && recivedCommand_List.Count > 0; i++)
                {
                    string[] messages = recivedCommand_List[0].Split(',');
                    if (messages[0].Equals("BulletMove"))
                    {
                        if (int.Parse(messages[1]) < clientBullets_List.Count)
                            clientBullets_List[int.Parse(messages[1])].SetPoint(int.Parse(messages[2]), int.Parse(messages[3]));
                        else
                            break;
                    }
                    else if (messages[0].Equals("BulletRemove"))
                    {
                        if (int.Parse(messages[1]) < clientBullets_List.Count)
                            clientBullets_List[int.Parse(messages[1])].End = true;
                        else
                            break;
                    }
                }
                for (int i = 0; i < clientBullets_List.Count; i++)
                {
                    clientBullets_List[i].Update();
                    if(clientBullets_List[i].End)
                    {
                        clientBullets_List[i].Dispose();
                        clientBullets_List.RemoveAt(i);
                    }
                }
                if (!gsm.csm.GameStart)
                {
                    gameOver = true;
                }
                this.bulletcountLabel.Text = players[gsm.csm.PlayerId].bulletCount.ToString();
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
