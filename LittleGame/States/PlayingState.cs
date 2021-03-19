using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LittleGameV1._1.State
{
    class PlayingState : GameState
    {
        public TileMap.TileMap tileMap;
        public List<Entity.Bomp> bomps;

        public struct BompSet
        {
            public int player, x, y;
        };
        public List<BompSet>[] bompSets;
        public struct BompRemove
        {
            public int player;
        };
        public List<BompRemove>[] bompRemoves;
        public struct BompExplosion
        {
            public int player, k, h_x, h_width, v_y, v_height;
        }
        public List<BompExplosion>[] bompExplosions;
        public struct HitSet
        {
            public int player, damage;
        };
        public List<HitSet>[] hitSets;

        private System.Windows.Forms.Label winnerLabel;
        public Entity.Player[] players;
        private bool gameOver;
        public int playerNum;
        public int aliveNum;
        public const int maxPlayerNum = 4;
        private int winner;
        private static System.Drawing.Point[] playerPoints =
        {
            new System.Drawing.Point(50,50),
            new System.Drawing.Point(700,50),
            new System.Drawing.Point(50,500),
            new System.Drawing.Point(700,500)
        };
        private System.Windows.Forms.Label playerInfo;

        private static System.Drawing.Bitmap[] images =
        {
            global::LittleGameV1._1.Properties.Resources.Map1_1,
            global::LittleGameV1._1.Properties.Resources.Map1_2,
            global::LittleGameV1._1.Properties.Resources.Map1_3,
            global::LittleGameV1._1.Properties.Resources.Map1_4
        };

        private static string[] maps =
        {
            @"..\..\Resources\Map\Map1_1.txt",
            @"..\..\Resources\Map\Map1_2.txt",
            @"..\..\Resources\Map\Map1_3.txt",
            @"..\..\Resources\Map\Map1_4.txt"
        };



        public PlayingState(GameStateManager gsm)
        {
            this.gsm = gsm;
            tileMap = new TileMap.TileMap(this, maps[this.gsm.ssm.map]);
            players = new Entity.Player[maxPlayerNum];
            players[0] = new Entity.Player(gsm, this, 0, playerPoints[0]);
            playerNum = 1;
            if(gsm.ssm.getClientPlayerNum() < 1)
            {
                addPlyaer();
            }
            for (int i = 0; i < gsm.ssm.getClientPlayerNum(); i++)
                addPlyaer();
            aliveNum = playerNum;
            gameOver = false;
            winner = 0;

            bomps = new List<Entity.Bomp>();
            bompSets = new List<BompSet>[playerNum-1];
            bompRemoves = new List<BompRemove>[playerNum-1];
            bompExplosions = new List<BompExplosion>[playerNum-1];
            hitSets = new List<HitSet>[playerNum-1];
            for (int i = 0; i < playerNum-1; i++)
            {
                bompSets[i] = new List<BompSet>();
                bompRemoves[i] = new List<BompRemove>();
                bompExplosions[i] = new List<BompExplosion>();
                hitSets[i] = new List<HitSet>();
            }

            this.playerInfo = new System.Windows.Forms.Label();
            this.playerInfo.AutoSize = true;
            this.playerInfo.BackColor = System.Drawing.Color.Transparent;
            this.playerInfo.Location = new System.Drawing.Point(0, 0);
            this.playerInfo.Name = "player info";
            this.playerInfo.Size = new System.Drawing.Size(30, 60);
            this.playerInfo.Font = new System.Drawing.Font("標楷體", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.playerInfo.Text = players[0].getBompNum().ToString() + "/" + players[0].getMaxBompNum().ToString();
            this.playerInfo.BringToFront();

            //panel
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "playing state";
            this.Size = new System.Drawing.Size(800, 600);
            this.TabIndex = 0;
            this.BackColor = System.Drawing.Color.LightBlue;
            this.BackgroundImage = images[gsm.ssm.map];
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(playerInfo);
            

            //winner label
            this.winnerLabel = new System.Windows.Forms.Label();
            this.winnerLabel.AutoSize = true;
            this.winnerLabel.BackColor = System.Drawing.Color.Transparent;
            this.winnerLabel.Location = new System.Drawing.Point(250, 250);
            this.winnerLabel.Name = "winner";
            this.winnerLabel.Size = new System.Drawing.Size(200, 100);
            this.winnerLabel.Font = new System.Drawing.Font("標楷體", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.winnerLabel.Text = "P1 is winner";

            this.gsm.ssm.setPlayingState(this);
        }

        public void addPlyaer()
        {
            players[playerNum] = new Entity.Player(gsm, this, playerNum, playerPoints[playerNum]);
            playerNum++;
        }

        public override void update()
        {
            if (!gameOver)
            {
                for (int i = 0; i < playerNum; i++)
                {
                    players[i].update();
                }
                this.playerInfo.Text = players[0].getBompNum().ToString() + "/" + players[0].getMaxBompNum().ToString();
                this.playerInfo.BringToFront();
                if (aliveNum <= 1)
                {
                    for(int i = 0; i < playerNum; i++)
                    {
                        if (players[i].getAlive())
                        {
                            winner = i+1;
                        }
                    }
                    gameOver = true;
                    gsm.ssm.gameStart = false;
                }
            }
            if (gameOver)
            {
                if(winner != 0)
                    winnerLabel.Text = "P" + winner.ToString() + " is winner";
                else
                    winnerLabel.Text = "TIE";
                this.Controls.Add(winnerLabel);
                winnerLabel.BringToFront();
            }
            
        }

        public override void keyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) { players[0].setUp(true); }
            if (e.KeyCode == Keys.S) { players[0].setDown(true); }
            if (e.KeyCode == Keys.A) { players[0].setLeft(true); }
            if (e.KeyCode == Keys.D) { players[0].setRight(true); }
            if (e.KeyCode == Keys.Space) { players[0].setAttack(true); }

            if (gsm.ssm.getClientPlayerNum() < 1)
            {
                if (e.KeyCode == Keys.Up) { players[1].setUp(true); }
                if (e.KeyCode == Keys.Down) { players[1].setDown(true); }
                if (e.KeyCode == Keys.Left) { players[1].setLeft(true); }
                if (e.KeyCode == Keys.Right) { players[1].setRight(true); }
                if (e.KeyCode == Keys.Enter) { players[1].setAttack(true); }
            }

            if (gameOver)
            {
                gsm.setState(GameStateManager.MENUSTATE);
            }
        }

        public override void keyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) { players[0].setUp(false); }
            if (e.KeyCode == Keys.S) { players[0].setDown(false); }
            if (e.KeyCode == Keys.A) { players[0].setLeft(false); }
            if (e.KeyCode == Keys.D) { players[0].setRight(false); }
            if (e.KeyCode == Keys.Space) { players[0].setAttack(false); }

            if (gsm.ssm.getClientPlayerNum() < 1)
            {
                if (e.KeyCode == Keys.Up) { players[1].setUp(false); }
                if (e.KeyCode == Keys.Down) { players[1].setDown(false); }
                if (e.KeyCode == Keys.Left) { players[1].setLeft(false); }
                if (e.KeyCode == Keys.Right) { players[1].setRight(false); }
                if (e.KeyCode == Keys.Enter) { players[1].setAttack(false); }
            }
        }

    }
}
