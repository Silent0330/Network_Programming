using LittleGame.Entity;
using LittleGame.TileMaps;
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

namespace LittleGame.State
{
    class PlayingState
    {
        public TileMap tileMap;
        public List<Entity.Bullet> bullet_List;
        
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

        private static string[] maps =
        {
            @"..\..\Resources\Map\Map1_1.txt",
        };



        public PlayingState(int player_num)
        {
            tileMap = new TileMap(maps[0]);
            players = new Player[maxPlayerNum];
            playerNum = player_num;
            for (int i = 0; i < playerNum; i++)
            {
                players[playerNum] = new Entity.Player(this, i, playerPoints[i].X, playerPoints[i].Y);
                playerNum++;
            }
            aliveNum = playerNum;
            gameOver = false;
            winner = 0;
        }
        
        public void update()
        {
            if (!gameOver)
            {
                for (int i = 0; i < playerNum; i++)
                {
                    players[i].update();
                }
                if (aliveNum <= 1)
                {
                    for(int i = 0; i < playerNum; i++)
                    {
                        if (players[i].Alive)
                        {
                            winner = i+1;
                        }
                    }
                    gameOver = true;
                }
            }
            
        }
    }
}
