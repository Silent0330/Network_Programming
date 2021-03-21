using LittleGame.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleGame.Client
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
        public const int UP = 0;
        public const int DOWN = 1;
        public const int LEFT = 2;
        public const int RIGHT = 3;
        public const int MOVEUP = 4;
        public const int MOVEDOWN = 5;
        public const int MOVELEFT = 6;
        public const int MOVERIGHT = 7;

        private static System.Drawing.Bitmap[,] images =
        {
            {
                global::LittleGame.Properties.Resources.p1up,
                global::LittleGame.Properties.Resources.p1down,
                global::LittleGame.Properties.Resources.p1left,
                global::LittleGame.Properties.Resources.p1right,
                global::LittleGame.Properties.Resources.p1moveup,
                global::LittleGame.Properties.Resources.p1movedown,
                global::LittleGame.Properties.Resources.p1moveleft,
                global::LittleGame.Properties.Resources.p1moveright
            },
            {
                global::LittleGame.Properties.Resources.p2up,
                global::LittleGame.Properties.Resources.p2down,
                global::LittleGame.Properties.Resources.p2left,
                global::LittleGame.Properties.Resources.p2right,
                global::LittleGame.Properties.Resources.p2moveup,
                global::LittleGame.Properties.Resources.p2movedown,
                global::LittleGame.Properties.Resources.p2moveleft,
                global::LittleGame.Properties.Resources.p2moveright
            },
            {
                global::LittleGame.Properties.Resources.p3up,
                global::LittleGame.Properties.Resources.p3down,
                global::LittleGame.Properties.Resources.p3left,
                global::LittleGame.Properties.Resources.p3right,
                global::LittleGame.Properties.Resources.p3moveup,
                global::LittleGame.Properties.Resources.p3movedown,
                global::LittleGame.Properties.Resources.p3moveleft,
                global::LittleGame.Properties.Resources.p3moveright,
            },
            {
                global::LittleGame.Properties.Resources.p4up,
                global::LittleGame.Properties.Resources.p4down,
                global::LittleGame.Properties.Resources.p4left,
                global::LittleGame.Properties.Resources.p4right,
                global::LittleGame.Properties.Resources.p4moveup,
                global::LittleGame.Properties.Resources.p4movedown,
                global::LittleGame.Properties.Resources.p4moveleft,
                global::LittleGame.Properties.Resources.p4moveright
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
            this.pictureBox.Size = new System.Drawing.Size(25, 25);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
        }
        

        public void Hited(int damage)
        {
            if (alive)
            {
                hp -= damage;
                if(hp <= 0)
                {
                    alive = false;
                    this.state.Controls.Remove(pictureBox);
                }
            }
        }

        public void SetPoint(int x, int y)
        {
            this.point.X = x;
            this.point.Y = y;
        }

        public void SetFace(int face)
        {
            this.face = face;
        }
        
        public void Update()
        {
            if (alive)
            {
                LoadImage(images[id, face]);
                pictureBox.BringToFront();
            }
        }
    }
}
