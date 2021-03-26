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
        private bool key_attack;
        public bool Key_Attack { get => key_attack; }
        private bool key_reload;
        public bool Key_Reload { get => key_reload; }
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
            this.moveSpeed = 2;

            //info
            this.id = id;
            this.alive = true;
            this.hp = 1;

            //action
            this.key_up = false;
            this.key_down = false;
            this.key_left = false;
            this.key_right = false;
            this.key_attack = false;
            this.key_reload = false;
            this.face = DOWN;

            //attack
            this.attackSpeed = 50;
            this.attackDelay = 0;
            this.maxBulletCount = 6;
            this.bulletCount = maxBulletCount;
            this.reloadingTime = 100;
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
                        state.clientMessages[i] += ("Dead," + id.ToString());
                    }
                }
            }
        }
        
        public bool SetAction()
        {
            if (key_up)
            {
                if(face != UP)
                {
                    face = UP;
                    return true;
                }
            }
            else if (key_down)
            {
                if (face != DOWN)
                {
                    face = DOWN;
                    return true;
                }
            }
            else if (key_left)
            {
                if (face != LEFT)
                {
                    face = LEFT;
                    return true;
                }
            }
            else if (key_right)
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
                key_up = state.ssm.clientHandler_List[id].Up;
                key_down = state.ssm.clientHandler_List[id].Down;
                key_left = state.ssm.clientHandler_List[id].Left;
                key_right = state.ssm.clientHandler_List[id].Right;
                key_attack = state.ssm.clientHandler_List[id].Attack;
                key_reload = state.ssm.clientHandler_List[id].Reload;
            }
        }

        public void Attack()
        {
            if (reloadingDownCount == 0)
            {
                if (key_attack)
                {
                    if (bulletCount > 0 && attackDelay == 0)
                    {
                        state.bullet_List.Add(new Bullet(state, tileMap, id, face, point.X + width / 2, point.Y + height / 2));
                        bulletCount--;
                        attackDelay = attackSpeed;
                        for (int i = 0; i < state.playerNum; i++)
                        {
                            state.clientMessages[i] += ("Attack," + id.ToString() + ";");
                        }
                    }
                }
                if (key_reload)
                {
                    if(bulletCount != maxBulletCount)
                    {
                        reloadingDownCount = reloadingTime;
                        for (int i = 0; i < state.playerNum; i++)
                        {
                            state.clientMessages[i] += ("Reload," + id.ToString() + ";");
                        }
                    }
                    key_reload = false;
                }
            }
            else
            {
                reloadingDownCount--;
                if (reloadingDownCount == 0)
                {
                    bulletCount = maxBulletCount;
                    for (int i = 0; i < state.playerNum; i++)
                    {
                        state.clientMessages[i] += ("ReloadDone," + id.ToString() + ";");
                    }
                }
            }
            if (attackDelay > 0)
                attackDelay--;
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
                        state.clientMessages[i] += ("Move," + id.ToString() + "," + point.X.ToString() + "," + point.Y.ToString() + ";");
                    }
                }
                Attack();
                if (SetAction())
                {
                    for (int i = 0; i < state.playerNum; i++)
                    {
                        state.clientMessages[i] += ("Face," + id.ToString() + "," + face.ToString() + ";");
                    }
                }
            }
        }

    }
}
