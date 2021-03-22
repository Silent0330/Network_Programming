using LittleGame.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleGame.Entity
{ 
    class ClientBullet
    {
        private ClientPlayingState state;
        private System.Windows.Forms.PictureBox pictureBox;
        public System.Drawing.Point point;
        private int speed;
        private bool exist;
        private int direction;
        public const int UP = 0;
        public const int DOWN = 1;
        public const int LEFT = 2;
        public const int RIGHT = 3;
        private static System.Drawing.Bitmap[] images =
        {
            {
                global::LittleGame.Properties.Resources.bulletup,
                global::LittleGame.Properties.Resources.bulletdown,
                global::LittleGame.Properties.Resources.bulletleft,
                global::LittleGame.Properties.Resources.bulletright
            }
        };

        public ClientBullet(State.ClientPlayingState state, int x, int y)
        {
            this.state = state;
            pictureBox = new System.Windows.Forms.PictureBox();
            this.point = new System.Drawing.Point(x, y);
            this.speed = 100;
            this.exist = true;
            this.direction = DOWN;

            LoadImage(images[this.direction]);
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
            Console.WriteLine(point);
        }

        public void SetDirection(int dir)
        {
            this.direction = dir;
        }
        public void Update()
        {
            if (exist)
            {
                LoadImage(images[direction]);
                pictureBox.BringToFront();
            }
        }
    };
}
