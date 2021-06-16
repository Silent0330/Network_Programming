using LittleGame.Entity;
using LittleGame.TileMaps;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace LittleGame.State
{
    class ClientPlayingState : GameState
    {
        public TileMap tileMap;
        public ClientPlayer[] players;
        public List<ClientBullet> clientBullets_List;
        public List<string> recivedCommand_List;
        public int playerNum;
        public int aliveNum;
        public bool gameOver;
        private System.Windows.Forms.Label winnerLabel;
        private System.Windows.Forms.Label bulletcountLabel;
        private Thread gameThread;

        private static System.Drawing.Point[] playerPoints =
        {
            new System.Drawing.Point(50,50),
            new System.Drawing.Point(700,50),
            new System.Drawing.Point(50,500),
            new System.Drawing.Point(700,500)
        };

        private static string[] maps =
        {
            @"..\..\Resources\Map\Map1_1.txt",
        };

        //move state
        private bool key_up;
        private bool key_down;
        private bool key_left;
        private bool key_right;
        private bool key_attack;
        private bool key_reload;

        public bool Key_Up { get => key_up; }
        public bool Key_UpDown { get => key_down; }
        public bool Key_UpLeft { get => key_left; }
        public bool Key_UpRight { get => key_right; }
        public bool Key_UpAttack { get => key_attack; }
        public bool Key_UpReload { get => key_reload; }

        private static System.Drawing.Bitmap[] images =
        {
            Properties.Resources.Map1_1
        };

        public ClientPlayingState(State.GameStateManager gsm)
        {
            this.gsm = gsm;
            this.playerNum = gsm.csm.PlayerNum;
            Console.WriteLine("Create PlayerNum = " + playerNum);
            aliveNum = playerNum;
            this.tileMap = new TileMap(maps[0]);
            players = new ClientPlayer[4];
            clientBullets_List = new List<ClientBullet>();
            recivedCommand_List = new List<string>();
            gameOver = false;
            key_up = key_down = key_left = key_right = key_attack = key_reload = false;

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
            this.bulletcountLabel.Size = new System.Drawing.Size(20, 20);
            this.bulletcountLabel.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));

            this.Controls.Add(bulletcountLabel);
            bulletcountLabel.BringToFront();

            gameThread = new Thread(GameLoop);
            gameThread.IsBackground = true;
            gameThread.Start();

            gsm.csm.state = this;
            gsm.csm.SendMessage("Ready," + true.ToString());
        }
        
        public override void KeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W && !key_up)
            {
                key_up = true;
                gsm.csm.SendMessage("Up," + key_up.ToString());
            }
            if (e.KeyCode == Keys.S && !key_down)
            {
                key_down = true;
                gsm.csm.SendMessage("Down," + key_down.ToString());
            }
            if (e.KeyCode == Keys.A && !key_left)
            {
                key_left = true;
                gsm.csm.SendMessage("Left," + key_left.ToString());
            }
            if (e.KeyCode == Keys.D && !key_right)
            {
                key_right = true;
                gsm.csm.SendMessage("Right," + key_right.ToString());
            }
            if (e.KeyCode == Keys.Space && !key_attack)
            {
                key_attack = true;
                gsm.csm.SendMessage("Attack," + key_attack.ToString());
            }
            if (e.KeyCode == Keys.R && !key_reload)
            {
                key_reload = true;
                gsm.csm.SendMessage("Reload," + key_reload.ToString());
            }

            if (gameOver && e.KeyCode == Keys.Space)
            {
                gsm.csm.CloseConnection();
                gsm.SetState(GameStateManager.MENUSTATE);
            }
        }

        public override void KeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W && key_up)
            {
                key_up = false;
                gsm.csm.SendMessage("Up," + key_up.ToString());
            }
            if (e.KeyCode == Keys.S && key_down)
            {
                key_down = false;
                gsm.csm.SendMessage("Down," + key_down.ToString());
            }
            if (e.KeyCode == Keys.A && key_left)
            {
                key_left = false;
                gsm.csm.SendMessage("Left," + key_left.ToString());
            }
            if (e.KeyCode == Keys.D && key_right)
            {
                key_right = false;
                gsm.csm.SendMessage("Right," + key_right.ToString());
            }
            if (e.KeyCode == Keys.Space && key_attack)
            {
                key_attack = false;
                gsm.csm.SendMessage("Attack," + key_attack.ToString());
            }
            if (e.KeyCode == Keys.R && key_reload)
            {
                key_reload = false;
                gsm.csm.SendMessage("Reload," + key_reload.ToString());
            }
        }

        public override void Update()
        {
            if (!gameOver)
            {
                if (!gsm.csm.Connected)
                {
                    gsm.SetState(GameStateManager.MENUSTATE);
                }
                for (int i = 0; i < 20 && gsm.csm.ReceivedMessages.Count > 0; i++)
                {
                    String message = gsm.csm.PopReceivedMessage(0);
                    if (message == null) continue;
                    String[] messages = message.Split(';');
                    for (int j = 0; j < messages.Length; j++)
                    {
                        String[] messageArgs = messages[j].Split(',');

                        if (messageArgs[0].Equals("Move"))
                        {
                            players[int.Parse(messageArgs[1])].SetPoint(int.Parse(messageArgs[2]), int.Parse(messageArgs[3]));
                        }
                        else if (messageArgs[0].Equals("Face"))
                        {
                            players[int.Parse(messageArgs[1])].Face = (int.Parse(messageArgs[2]));
                        }
                        else if (messageArgs[0].Equals("Attack"))
                        {
                            players[int.Parse(messageArgs[1])].Attack(); ;
                        }
                        else if (messageArgs[0].Equals("Reload"))
                        {
                            players[int.Parse(messageArgs[1])].Reload = (true);
                        }
                        else if (messageArgs[0].Equals("ReloadDone"))
                        {
                            players[int.Parse(messageArgs[1])].ReloadDone = (true);
                        }
                        else if (messageArgs[0].Equals("Hitted"))
                        {
                            players[int.Parse(messageArgs[1])].Hitted(int.Parse(messageArgs[2]));
                        }
                    }
                }
                for (int i = 0; i < playerNum; i++)
                {
                    players[i].UIUpdate();
                }
                for (int i = 0; i < clientBullets_List.Count; i++)
                {
                    clientBullets_List[i].UIUpdate();
                    if(clientBullets_List[i].End)
                    {
                        clientBullets_List[i].Dispose();
                        clientBullets_List.RemoveAt(i);
                        i--;
                    }
                }
                if (!gsm.csm.GameStart)
                {
                    gameOver = true;
                }
                Console.WriteLine("PlayerNum = " + playerNum);
                bulletcountLabel.Location = new System.Drawing.Point(players[gsm.csm.PlayerId].point.X + 15, players[gsm.csm.PlayerId].point.Y - 20);
                if (players[gsm.csm.PlayerId].Reload)
                {
                    this.bulletcountLabel.Text = "R";
                }
                else
                {
                    this.bulletcountLabel.Text = players[gsm.csm.PlayerId].bulletCount.ToString();
                }
                bulletcountLabel.BringToFront();
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

        private void GameLoop()
        {
            while (!gameOver)
            {
                DateTime startTime = DateTime.Now;

                for (int i = 0; i < playerNum; i++)
                {
                    players[i].GameUpdate();
                }

                for (int i = 0; i < 20 && recivedCommand_List.Count > 0; i++)
                {
                    string[] messageArgs = recivedCommand_List[0].Split(',');
                    if (messageArgs[0].Equals("BulletMove"))
                    {
                        if (int.Parse(messageArgs[1]) < clientBullets_List.Count)
                        {
                            clientBullets_List[int.Parse(messageArgs[1])].SetPoint(int.Parse(messageArgs[2]), int.Parse(messageArgs[3]));
                            recivedCommand_List.RemoveAt(0);
                        }
                        else
                            break;
                    }
                    else if (messageArgs[0].Equals("BulletRemove"))
                    {
                        if (int.Parse(messageArgs[1]) < clientBullets_List.Count)
                        {
                            clientBullets_List[int.Parse(messageArgs[1])].End = true;
                            recivedCommand_List.RemoveAt(0);
                        }
                        else
                            break;
                    }
                }
                for (int i = 0; i < clientBullets_List.Count; i++)
                {
                    clientBullets_List[i].GameUpdate();
                }

                if (!gsm.csm.GameStart)
                {
                    gameOver = true;
                }

                double elapsedSeconds = (DateTime.Now - startTime).TotalSeconds;
                if (elapsedSeconds * 1000 < gsm.form.UpdateTime)
                {
                    Thread.Sleep(gsm.form.UpdateTime - (int)(elapsedSeconds * 1000));
                }
                //elapsedSeconds = (DateTime.Now - startTime).TotalSeconds;
                //fps = (1 / elapsedSeconds);
            }
        }
        public new void Dispose()
        {
            base.Dispose();
            if (gameThread != null)
            {
                gameThread.Abort();
                gameThread = null;
            }
        }

    }

}
