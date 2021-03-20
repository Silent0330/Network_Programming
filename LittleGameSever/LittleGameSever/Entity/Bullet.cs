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
        private int speed;
        private int endTime;
        private System.Timers.Timer timer;
        
        public Bullet(PlayingState state, TileMap tileMap, int x, int y)
        {
            //parants
            this.state = state;
            this.tileMap = tileMap;

            //info
            this.point.X = x;
            this.point.Y = y;

            this.timer = new System.Timers.Timer(100);
            this.timer.Elapsed += count;
            this.timer.Enabled = true;
            this.timer.Start();

        }
        
        public void count(Object source, ElapsedEventArgs e)
        {
            endTime--;
            if(endTime == 0)
            {
                timer.Enabled = false;
            }
        }
        
        public bool dispose()
        {
            if (endTime == 0)
            {
                timer.Dispose();
                return true;
            }
            return false;
        }

    }
}
