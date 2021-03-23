using LittleGame.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleGame.Entity
{
    class ClientPlayer
    {
        private ClientPlayingState state;
        private System.Windows.Forms.PictureBox pictureBox;
        public System.Drawing.Point point;
        private int id;
        private bool alive;
        public bool Alive { get => alive; }
        private int hp;

        //action
        private int face;
        public int Face { get => face; set => face = value; }
        public const int UP = 0;
        public const int DOWN = 1;
        public const int LEFT = 2;
        public const int RIGHT = 3;
        public const int MOVEUP = 4;
        public const int MOVEDOWN = 5;
        public const int MOVELEFT = 6;
        public const int MOVERIGHT = 7;

        private bool attack;
        public bool Attack { get => attack; set => attack = value; }
        private bool reload;
        public bool Reload { get => reload; set => reload = value; }
        private int bulletCount;
        private int maxBulletCount;

        private static System.Drawing.Bitmap[,] images =
        {
            {
                Properties.Resources.p1up,
                Properties.Resources.p1down,
                Properties.Resources.p1left,
                Properties.Resources.p1right,
                Properties.Resources.p1moveup,
                Properties.Resources.p1movedown,
                Properties.Resources.p1moveleft,
                Properties.Resources.p1moveright
            },
            {
                Properties.Resources.p2up,
                Properties.Resources.p2down,
                Properties.Resources.p2left,
                Properties.Resources.p2right,
                Properties.Resources.p2moveup,
                Properties.Resources.p2movedown,
                Properties.Resources.p2moveleft,
                Properties.Resources.p2moveright
            },
            {
                Properties.Resources.p3up,
                Properties.Resources.p3down,
                Properties.Resources.p3left,
                Properties.Resources.p3right,
                Properties.Resources.p3moveup,
                Properties.Resources.p3movedown,
                Properties.Resources.p3moveleft,
                Properties.Resources.p3moveright,
            },
            {
                Properties.Resources.p4up,
                Properties.Resources.p4down,
                Properties.Resources.p4left,
                Properties.Resources.p4right,
                Properties.Resources.p4moveup,
                Properties.Resources.p4movedown,
                Properties.Resources.p4moveleft,
                Properties.Resources.p4moveright
            }
        };

        public ClientPlayer(State.ClientPlayingState state, int id, int x, int y)
        {
            this.state = state;
            pictureBox = new System.Windows.Forms.PictureBox();
            this.id = id;
            this.alive = true;
            this.face = DOWN;
            this.point = new System.Drawing.Point(x, y);
            this.hp = 1;
            this.attack = false;
            this.reload = true;

            LoadImage(images[this.id, this.face]);
            this.state.Controls.Add(pictureBox);
            pictureBox.BringToFront();
        }
        
        private void LoadImage(System.Drawing.Bitmap image)
        {
            this.pictureBox.Image = image;
            this.pictureBox.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox.Location = point;
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(50, 50);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
        }

        public void SetPoint(int x, int y)
        {
            this.point.X = x;
            this.point.Y = y;
        }

        public void Update()
        {
            if (alive)
            {
                if(attack)
                {
                    state.clientBullets_List.Add(new ClientBullet(state, face, point.X, point.Y));
                    bulletCount--;
                    attack = false;
                }
                if(reload)
                {
                    bulletCount = maxBulletCount;
                    reload = false;
                }
                LoadImage(images[id, face]);
                pictureBox.BringToFront();
            }
        }
    }
}
