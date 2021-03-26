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

        public Bullet(PlayingState state, TileMap tileMap, int id, int face, int x, int y)
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
            this.moveSpeed = 1;

            // action
            this.face = face;
            this.damage = 1;
            this.blocked = false;

            key_up = key_down = key_left = key_right = false;
            if(face == UP)
            {
                key_up = true;
                this.height = 20;
                this.width = 5;
            }
            else if (face == DOWN)
            {
                key_down = true;
                this.height = 20;
                this.width = 5;
            }
            else if (face == LEFT)
            {
                key_left = true;
                this.height = 5;
                this.width = 20;
            }
            else if (face == RIGHT)
            {
                key_right = true;
                this.height = 5;
                this.width = 20;
            }

            // end
            this.endTime = 20;
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
                    for (int i = 0; i < state.playerNum; i++)
                    {
                        state.clientMessages[i] += ("BulletMove," + index.ToString() + "," + point.X.ToString() + "," + point.Y.ToString() + ";");
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
                if (owner != i && state.players[i].Alive && InterSection(GetMoveRectangle(), state.players[i].GetRectangle()))
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
