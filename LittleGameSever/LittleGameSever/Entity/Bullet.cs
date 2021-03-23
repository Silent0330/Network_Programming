using LittleGame.State;
using LittleGame.TileMaps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace LittleGame.Entity
{
    class Bullet : MapObject
    {
        private int endTime;
        private bool end;
        private int owner;
        public bool End { get => end; }
        private System.Timers.Timer timer;

        // action
        private int damage;
        private bool blocked;

        public Bullet(PlayingState state, TileMap tileMap, int id, int x, int y, int face)
        {
            // parants
            this.state = state;
            this.tileMap = tileMap;
            this.owner = id;

            // position
            this.point = new Point(x, y);
            this.vx = 0;
            this.vy = 0;
            this.stepSize = 20;
            this.moveDelay = 0;
            this.moveSpeed = 5;

            // action
            this.face = face;
            this.height = 40;
            this.width = 40;
            this.damage = 1;
            this.blocked = false;

            up = down = left = right = false;
            if(face == UP)
            {
                up = true;
            }
            else if (face == DOWN)
            {
                down = true;
            }
            else if (face == LEFT)
            {
                left = true;
            }
            else if (face == RIGHT)
            {
                right = true;
            }

            // end
            this.endTime = 10;
            this.end = false;
            this.timer = new System.Timers.Timer(100);
            this.timer.Elapsed += count;
            this.timer.Enabled = true;
            this.timer.Start();

        }
        
        public void count(Object source, ElapsedEventArgs e)
        {
            endTime--;
            if(endTime == 0 || end)
            {
                end = true;
                timer.Enabled = false;
            }
        }

        public void Update(int index)
        {
            if (!end)
            {
                if (Move())
                {
                    Console.WriteLine("BulletMove," + index.ToString() + "," + point.X.ToString() + "," + point.Y.ToString());
                    for (int i = 0; i < state.playerNum; i++)
                    {
                        state.ssm.SendMessage(i, "BulletMove," + index.ToString() + "," + point.X.ToString() + "," + point.Y.ToString());
                    }
                }
                if (blocked)
                    end = true;
            }
        }
        
        public bool Dispose()
        {
            if (end)
            {
                timer.Dispose();
                return true;
            }
            return false;
        }

        public Rectangle GetMoveRectangle()
        {
            int x = point.X, y = point.Y, w = width, h = height;
            if(face == UP)
            {
                h = height + (point.Y - dy);
                y = dy;
            }
            else if (face == DOWN)
            {
                h = height + (dy - point.Y);
            }
            else if (face == LEFT)
            {
                w = width + (point.X - dx);
                x = dx;
            }
            else if (face == RIGHT)
            {
                w = width + (dx - point.X);
            }
            return new Rectangle(x, y, w, h);
        }

        private void Attack()
        {
            for(int i = 0; i < state.playerNum; i++)
            {
                if (owner != i && InterSection(GetMoveRectangle(), state.players[i].GetRectangle()))
                {
                    state.players[i].Hited(damage);
                    end = true;
                    return;
                }
            }
        }

        protected new bool Move()
        {
            GetNextPosition();
            blocked = CheckMapCollision();
            Attack();
            return SetPosition();
        }

    }
}
