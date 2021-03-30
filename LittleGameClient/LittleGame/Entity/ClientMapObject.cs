using LittleGame.State;
using LittleGame.TileMaps;
using System.Drawing;

namespace LittleGame.Entity
{
    abstract class ClientMapObject
    {
        protected ClientPlayingState state;
        protected TileMap tileMap;

        //position and move
        protected Point point;
        protected System.Drawing.Size size;
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
            return new Rectangle(point.X, point.Y, size.Width, size.Height);
        }

        public bool InterSection(ClientMapObject obj)
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
            l = point.X / TileMap.TILE_SIZE;
            r = (point.X + size.Width) / TileMap.TILE_SIZE;
            u = dy / TileMap.TILE_SIZE;
            d = (dy + size.Height) / TileMap.TILE_SIZE;
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
                blocked = true;
            }

            //left right
            l = dx / TileMap.TILE_SIZE;
            r = (dx + size.Width) / TileMap.TILE_SIZE;
            u = dy / TileMap.TILE_SIZE;
            d = (dy + size.Height) / TileMap.TILE_SIZE;
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
                blocked = true;
            }
            return blocked;
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
