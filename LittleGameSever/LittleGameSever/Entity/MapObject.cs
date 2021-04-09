using LittleGameSever.State;
using LittleGameSever.TileMaps;
using System.Drawing;

namespace LittleGameSever.Entity
{
    abstract class MapObject
    {
        protected PlayingState state;
        protected TileMap tileMap;

        //position and move
        protected int px;
        protected int py;
        protected int height, width;
        protected int vx, vy;
        protected int stepSize = 5;
        protected int dx, dy;
        protected int moveDelay;
        protected int moveSpeed;

        //control
        protected bool key_up;
        protected bool key_down;
        protected bool key_left;
        protected bool key_right;
        

        public bool Key_Up{ get { return key_up; } set { key_up = value; } }
        public bool Key_Down { get { return key_down; } set { key_down = value; } }
        public bool Key_Left { get { return key_left; } set { key_left = value; } }
        public bool Key_Right { get { return key_right; } set { key_right = value; } }

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
            return new Rectangle(px, py, width, height);
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
                dx = px + vx;
                dy = py + vy;
                if (vx != 0 || vy != 0)
                    moveDelay = moveSpeed;
            }
            else moveDelay--;
        }

        public void SetSpeed()
        {
            vx = vy = 0;
            if (key_up)
            {
                vy -= stepSize;
            }
            if (key_down)
            {
                vy += stepSize;
            }
            if (key_left)
            {
                vx -= stepSize;
            }
            if (key_right)
            {
                vx += stepSize;
            }
        }

        protected bool CheckMapCollision()
        {
            bool blocked = false;
            bool leftBlocked, rightBlocked, upBlocked, downBlocked;
            int l, r, u, d;

            //up down
            l = px / TileMap.TILE_SIZE;
            r = (px + width) / TileMap.TILE_SIZE;
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
                dy = py;
                vy = 0;
                blocked = true;
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
                dx = px;
                vx = 0;
                blocked = true;
            }
            return blocked;
        }
        
        protected bool SetPosition()
        {
            bool changed = (px != dx || py != dy);
            if(changed)
            {
                px = dx;
                py = dy;
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
