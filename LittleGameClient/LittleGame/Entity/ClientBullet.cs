using LittleGame.State;
using System;
using System.Timers;

namespace LittleGame.Entity
{
    class ClientBullet
    {
        private ClientPlayingState state;
        private System.Windows.Forms.PictureBox pictureBox;
        public System.Drawing.Point point;
        public System.Drawing.Size size;

        private int endTime;
        private System.Timers.Timer timer;

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

            this.endTime = 20;
            this.end = false;
            this.timer = new System.Timers.Timer(100);
            this.timer.Elapsed += count;
            this.timer.Enabled = true;
            this.timer.Start();

            if (direction == UP || direction == DOWN)
            {
                size = new System.Drawing.Size(5, 20);
            }
            else if (direction == LEFT || direction == RIGHT)
            {
                size = new System.Drawing.Size(20, 5);
            }

            LoadImage(images[this.direction]);
            this.state.Controls.Add(pictureBox);
            pictureBox.BringToFront();
        }
        public void count(Object source, ElapsedEventArgs e)
        {
            endTime--;
            if (endTime == 0 || end)
            {
                end = true;
                timer.Enabled = false;
            }
        }

        private void LoadImage(System.Drawing.Bitmap image)
        {
            this.pictureBox.Image = image;
            this.pictureBox.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox.Location = point;
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = size;
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
            timer.Dispose();
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
