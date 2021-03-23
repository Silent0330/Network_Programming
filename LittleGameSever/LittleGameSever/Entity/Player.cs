using LittleGame.State;
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
        public int Id { get => id; }
        private bool alive;
        public bool Alive { get => alive; }
        private int hp;
        public int Hp { get => hp; }

        // attack
        private bool attack;
        public bool Attack { get => attack; }
        private bool reload;
        public bool Reload { get => reload; }
        private int attackDelay;
        private int attackSpeed;
        private int bulletCount;
        private int maxBulletCount;
        private int reloadingTime;
        private int reloadingDownCount;

        public Player(PlayingState state, int id, int x, int y)
        {
            //parants
            this.state = state;
            this.tileMap = state.tileMap;

            //position
            this.point = new Point(x, y);
            this.vx = 0;
            this.vy = 0;
            this.stepSize = 10;
            this.moveDelay = 0;
            this.moveSpeed = 5;

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
            this.reload = false;
            this.face = DOWN;

            //attack
            this.attackSpeed = 50;
            this.attackDelay = 0;
            this.maxBulletCount = 6;
            this.bulletCount = maxBulletCount;
            this.reloadingTime = 10;
            this.reloadingDownCount = 0;

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
                    for (int i = 0; i < state.playerNum; i++)
                    {
                        state.ssm.SendMessage(i, "Dead," + id.ToString());
                    }
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
            if(id < state.ssm.clientHandler_List.Count && state.ssm.clientHandler_List[id].Connected)
            {
                up = state.ssm.clientHandler_List[id].Up;
                down = state.ssm.clientHandler_List[id].Down;
                left = state.ssm.clientHandler_List[id].Left;
                right = state.ssm.clientHandler_List[id].Right;
                attack = state.ssm.clientHandler_List[id].Attack;
                reload = state.ssm.clientHandler_List[id].Reload;
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
                        state.ssm.SendMessage(i, "Move," + id.ToString() + "," + point.X.ToString() + "," + point.Y.ToString());
                    }
                }
                if(reloadingDownCount == 0)
                {
                    if (attack)
                    {
                        if(bulletCount > 0 && attackDelay == 0)
                        {
                            state.bullet_List.Add(new Bullet(state, tileMap, id, point.X, point.Y, face));
                            bulletCount--;
                            attackDelay = attackSpeed;
                            for (int i = 0; i < state.playerNum; i++)
                            {
                                state.ssm.SendMessage(i, "Attack," + id.ToString());
                            }
                        }
                    }
                    if(reload)
                    {
                        reloadingDownCount = reloadingTime;
                        for (int i = 0; i < state.playerNum; i++)
                        {
                            state.ssm.SendMessage(i, "Reload," + id.ToString());
                        }
                        reload = false;
                    }
                }
                else
                {
                    reloadingDownCount--;
                    if (reloadingDownCount == 0)
                        bulletCount = maxBulletCount;
                }
                if (attackDelay > 0)
                    attackDelay--;
                if (SetAction())
                {
                    for (int i = 0; i < state.playerNum; i++)
                    {
                        state.ssm.SendMessage(i, "Face," + id.ToString() + "," + face.ToString());
                    }
                }
            }
        }

    }
}
