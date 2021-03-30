using LittleGameSever.Entity;
using LittleGameSever.SeverManager;
using LittleGameSever.TileMaps;
using System;
using System.Collections.Generic;

namespace LittleGameSever.State
{
    class PlayingState
    {
        public SeverSocketManager ssm;
        public Form1 form;

        public TileMap tileMap;
        public List<Entity.Bullet> bullet_List;
        
        public Player[] players;
        public string[] clientMessages;
        private bool gameOver;
        public bool GameOver { get => gameOver; }
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



        public PlayingState(Form1 form, SeverSocketManager ssm, int player_num)
        {
            this.form = form;
            this.ssm = ssm;
            this.tileMap = new TileMap(maps[0]);
            this.players = new Player[maxPlayerNum];
            this.clientMessages = new string[maxPlayerNum];
            this.bullet_List = new List<Bullet>();

            this.playerNum = player_num;
            for (int i = 0; i < playerNum; i++)
            {
                players[i] = new Entity.Player(this, i, playerPoints[i].X, playerPoints[i].Y);
            }
            aliveNum = playerNum;
            gameOver = false;
            winner = 0;
        }

        public void AddMessage(int i, string message)
        {
            clientMessages[i] += message + ";";
        }

        public void Update()
        {
            if (!gameOver)
            {
                for (int i = 0; i < playerNum; i++)
                {
                    clientMessages[i] = "";
                }
                for (int i = 0; i < playerNum; i++)
                {
                    players[i].Update();
                }
                for (int i = 0; i < bullet_List.Count; i++)
                {
                    bullet_List[i].Update(i);
                    if(bullet_List[i].End)
                    {
                        bullet_List[i].Dispose();
                        bullet_List.RemoveAt(i);
                        for (int j = 0; j < playerNum; j++)
                        {
                            AddMessage(j,"BulletRemove," + i.ToString());
                        }
                        i--;
                    }
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
                    for (int i = 0; i < playerNum; i++)
                    {
                        AddMessage(i, "GameOver");
                    }
                }
                if(ssm.CurConnectionNum < 1)
                {
                    gameOver = true;
                }
            }
            for (int i = 0; i < playerNum; i++)
            {
                if(clientMessages[i].Length > 0)
                {
                    if(!ssm.SendMessage(i, clientMessages[i]))
                    {
                        Console.WriteLine(i + "failed " + clientMessages[i]);
                    }
                }
            }
        }

    }
}
