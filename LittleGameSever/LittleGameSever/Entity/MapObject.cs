using LittleGame.State;
using LittleGame.TileMaps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleGame.Entity
{
    abstract class MapObject
    {
        protected PlayingState state;
        protected TileMap tileMap;

        //position and move
        protected System.Drawing.Point point;
        protected int height, width;
        protected int vx, vy;
        protected int stepSize = 5;
        protected int dx, dy;
        protected int moveDelay;
        protected int moveSpeed;

        //control
        protected bool up;
        protected bool down;
        protected bool left;
        protected bool right;
        

        public bool Up{ get { return up; } set { up = value; } }
        public bool Down { get { return down; } set { down = value; } }
        public bool Left { get { return left; } set { left = value; } }
        public bool Right { get { return right; } set { right = value; } }

        // action
        protected int face;
        public int Face { get { return face; } }

        public const int UP = 0;
        public const int DOWN = 1;
        public const int LEFT = 2;
        public const int RIGHT = 3;
        public const int MOVEUP = 4;
        public const int MOVEDOWN = 5;
        public const int MOVELEFT = 6;
        public const int MOVERIGHT = 7;

        public Rectangle GetRectangle()
        {
            return new Rectangle(point.X, point.Y, width, height);
        }

        public bool InterSection(MapObject obj)
        {
            Rectangle rec1 = GetRectangle();
            Rectangle rec2 = obj.GetRectangle();
            return rec1.IntersectsWith(rec2);
        }

        public bool InterSection(Rectangle rec1, Rectangle rec2)
        {
            return rec1.IntersectsWith(rec2);
        }

        public void GetNextPosition()
        {
            if (moveDelay == 0)
            {
                SetSpeed();
                dx = point.X + vx;
                dy = point.Y + vy;
                if (vx != 0 || vy != 0)
                    moveDelay = moveSpeed;
            }
            else moveDelay--;
        }

        public void SetSpeed()
        {
            vx = vy = 0;
            if (up)
            {
                vy -= stepSize;
            }
            if (down)
            {
                vy += stepSize;
            }
            if (left)
            {
                vx -= stepSize;
            }
            if (right)
            {
                vx += stepSize;
            }
        }

        protected void CheckMapCollision()
        {

            bool leftBlocked, rightBlocked, upBlocked, downBlocked;
            int l, r, u, d;

            //up down
            l = dx / TileMap.TILE_SIZE;
            r = (dx + width) / TileMap.TILE_SIZE;
            u = dy / TileMap.TILE_SIZE;
            d = (dy + height) / TileMap.TILE_SIZE;
            if (dy >= 0)
                upBlocked = tileMap.getBlocked(u, l) || tileMap.getBlocked(u, r);
            else
                upBlocked = true;
            if (d < TileMap.numRows)
                downBlocked = tileMap.getBlocked(d, l) || tileMap.getBlocked(d, r);
            else
                downBlocked = true;
            if ((vy < 0 && upBlocked) || (vy > 0 && downBlocked))
            {
                dy = point.Y;
                vy = 0;
            }

            //left right
            l = dx / TileMap.TILE_SIZE;
            r = (dx + width) / TileMap.TILE_SIZE;
            u = dy / TileMap.TILE_SIZE;
            d = (dy + height) / TileMap.TILE_SIZE;
            if (dx >= 0)
                leftBlocked = tileMap.getBlocked(u, l) || tileMap.getBlocked(d, l);
            else
                leftBlocked = true;
            if (r < TileMap.numCols)
                rightBlocked = tileMap.getBlocked(u, r) || tileMap.getBlocked(d, r);
            else
                rightBlocked = true;
            if ((vx < 0 && leftBlocked) || (vx > 0 && rightBlocked))
            {
                dx = point.X;
                vx = 0;
            }
        }
        
        protected bool SetPosition()
        {
            bool changed = (point.X != dx || point.Y != dy);
            if(changed)
            {
                point.X = dx;
                point.Y = dy;
            }
            return changed;
        }

        protected bool Move()
        {
            GetNextPosition();
            CheckMapCollision();
            return SetPosition();
        }
    }
}
