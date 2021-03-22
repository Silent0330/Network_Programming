﻿using LittleGame.State;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleGame.Entity
{
    class Player : MapObject
    {
        // info
        private int id;
        public int Id { get { return id; } }
        private bool alive;
        public bool Alive { get { return alive; } }
        private int hp;
        public int Hp { get { return hp; } }

        // action
        private int face;
        public int Face{ get { return face; } }

        public const int UP = 0;
        public const int DOWN = 1;
        public const int LEFT = 2;
        public const int RIGHT = 3;
        public const int MOVEUP = 4;
        public const int MOVEDOWN = 5;
        public const int MOVELEFT = 6;
        public const int MOVERIGHT = 7;

        // attack
        private bool attack;
        public bool Attack { get { return attack; } set { attack = value; } }

        public Player(PlayingState state, int id, int x, int y)
        {
            //parants
            this.state = state;
            this.tileMap = state.tileMap;

            //position
            this.point = new Point(x, y);
            this.vx = 0;
            this.vy = 0;
            this.speed = 10;
            this.moveDelay = 0;

            //info
            this.id = id;
            this.alive = true;
            this.hp = 1;

            //action
            this.up = false;
            this.down = false;
            this.left = false;
            this.right = false;
            this.attack = false;
            this.face = DOWN;

            //rectangle
            this.width = 40;
            this.height = 40;
            
        }

        public void Hited(int damage)
        {
            if (alive)
            {
                hp -= damage;
                if(hp <=0)
                {
                    alive = false;
                    state.aliveNum--;
                }
            }
        }
        
        public bool SetAction()
        {
            if (up)
            {
                if(face != UP)
                {
                    face = UP;
                    return true;
                }
            }
            else if (down)
            {
                if (face != DOWN)
                {
                    face = DOWN;
                    return true;
                }
            }
            else if (left)
            {
                if (face != LEFT)
                {
                    face = LEFT;
                    return true;
                }
            }
            else if (right)
            {
                if (face != RIGHT)
                {
                    face = RIGHT;
                    return true;
                }
            }
            return false;
        }
        
        private void SetControll()
        {
            if(id < state.ssm.CurConnectionNum)
            {
                up = state.ssm.clientHandler_List[id].Up;
                down = state.ssm.clientHandler_List[id].Down;
                left = state.ssm.clientHandler_List[id].Left;
                right = state.ssm.clientHandler_List[id].Right;
            }
        }

        public void Update()
        {
            if (Alive)
            {
                SetControll();
                if(Move())
                {
                    for(int i = 0; i < state.playerNum; i++)
                    {
                        state.ssm.SendMessage(i, "Move," + id + "," + point.X.ToString() + "," + point.Y.ToString());
                    }
                }
                if (SetAction())
                {
                    for (int i = 0; i < state.playerNum; i++)
                    {
                        state.ssm.SendMessage(i, "Face," + id + "," + face.ToString());
                    }
                }
            }
        }

    }
}
