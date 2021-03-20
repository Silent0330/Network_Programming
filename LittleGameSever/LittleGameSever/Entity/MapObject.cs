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

        //position
        protected System.Drawing.Point point;
        protected int height, width;
        protected int vx, vy;
        protected int speed;
        protected int dx, dy;

        //move state
        protected bool up;
        protected bool down;
        protected bool left;
        protected bool right;
        protected int  moveDelay;
        

        public bool Up{ get { return up; } set { up = value; } }
        public bool Down { get { return down; } set { down = value; } }
        public bool Left { get { return left; } set { left = value; } }
        public bool Right { get { return right; } set { right = value; } }

        public Rectangle getRectangle()
        {
            return new Rectangle(point.X, point.Y, width, height);
        }

        public bool interSection(MapObject obj)
        {
            Rectangle rec1 = getRectangle();
            Rectangle rec2 = obj.getRectangle();
            return rec1.IntersectsWith(rec2);
        }

        public void getNextPosition()
        {
            if (moveDelay == 0)
            {
                setSpeed();
                dx = point.X + vx;
                dy = point.Y + vy;
                if (vx != 0 || vy != 0)
                    moveDelay = 7;
            }
            else moveDelay--;
        }

        public void setSpeed()
        {
            vx = vy = 0;
            if (up)
            {
                vy += speed;
            }
            if (down)
            {
                vy -= speed;
            }
            if (left)
            {
                vx -= speed;
            }
            if (right)
            {
                vx += speed;
            }
        }

        protected void checkMapCollision()
        {

            bool leftBlocked, rightBlocked, upBlocked, downBlocked;
            int l, r, u, d;

            //up down
            l = point.X / TileMap.TILE_SIZE;
            r = (point.X + width) / TileMap.TILE_SIZE;
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
        
        protected void setPosition()
        {
            point.X = dx;
            point.Y = dy;
        }

        protected void move()
        {
            getNextPosition();
            checkMapCollision();
            setPosition();
        }
    }
}
