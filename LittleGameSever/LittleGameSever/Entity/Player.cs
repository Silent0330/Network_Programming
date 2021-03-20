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
        private int number;
        public int Number { get { return number; } }
        private bool alive;
        public bool Alive { get { return alive; } }
        private int hp;
        public int Hp { get { return hp; } }

        // action
        private int face;
        public int Face{ get { return face; } }

        public const int UP = 0;
        public const int DOWN = 1;
        public const int LEFT = 2;
        public const int RIGHT = 3;
        public const int MOVEUP = 4;
        public const int MOVEDOWN = 5;
        public const int MOVELEFT = 6;
        public const int MOVERIGHT = 7;

        // attack
        private bool attack;
        public bool Attack { get { return attack; } set { attack = value; } }

        public Player(PlayingState state, int number, int x, int y)
        {
            //parants
            this.state = state;
            this.tileMap = state.tileMap;

            //position
            this.point = new Point(x, y);
            this.vx = 0;
            this.vy = 0;
            this.speed = 5;
            this.moveDelay = 0;

            //info
            this.number = number;
            this.alive = true;
            this.hp = 1;

            //action
            this.up = false;
            this.down = false;
            this.left = false;
            this.right = false;
            this.attack = false;
            this.face = DOWN;

            //rectangle
            this.width = 40;
            this.height = 40;
            
        }

        public void hited(int damage)
        {
            if (alive)
            {
                hp -= damage;
                if(hp <=0)
                {
                    alive = false;
                    state.aliveNum--;
                }
            }
        }
        
        public void setAction()
        {
            if (up)
            {
                face = UP;
            }
            else if (down)
            {
                face = DOWN;
            }
            else if (left)
            {
                face = LEFT;
            }
            else if (right)
            {
                face = RIGHT;
            }
        }
        
        public void update()
        {
            if (alive)
            {
                setAction();
                move();
            }
        }

    }
}
