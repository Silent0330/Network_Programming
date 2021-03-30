using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleGame.TileMaps
{
    class Tile
    {
        //tile types
        public const int AIR = 0;
        public const int WALL = 1;

        private int tileType;
        private bool blocked;

        public Tile(int type)
        {
            setTileType(type);
        }

        public void setTileType(int type)
        {
            tileType = type;
            if (type == AIR)
            {
                blocked = false;
            }
            else if(type == WALL)
            {
                blocked = true;
            }
        }

        public int getTileType()
        {
            return tileType;
        }

        public bool getBlocked()
        {
            return blocked;
        }
    }
}
