using LittleGame.State;
using System;

namespace LittleGame.Entity
{
    class ClientBullet
    {
        private ClientPlayingState state;
        private System.Windows.Forms.PictureBox pictureBox;
        public System.Drawing.Point point;

        private bool end;
        public bool End { get => end; set => end = value; }
        private int direction;
        public int Direction { get => direction; set => direction = value; }
        public const int UP = 0;
        public const int DOWN = 1;
        public const int LEFT = 2;
        public const int RIGHT = 3;
        private static System.Drawing.Bitmap[] images =
        {
            Properties.Resources.bulletup,
            Properties.Resources.bulletdown,
            Properties.Resources.bulletleft,
            Properties.Resources.bulletright
        };

        public ClientBullet(State.ClientPlayingState state, int face, int x, int y)
        {
            this.state = state;
            pictureBox = new System.Windows.Forms.PictureBox();
            this.point = new System.Drawing.Point(x, y);
            this.end = false;
            this.direction = face;

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
        }

        public void Dispose()
        {
            state.Controls.Remove(this.pictureBox);
        }

        public void Update()
        {
            if (!end)
            {
                LoadImage(images[direction]);
                pictureBox.BringToFront();
            }
        }
    };
}
