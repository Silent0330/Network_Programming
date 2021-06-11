using LittleGame.State;
using System;
using System.Timers;

namespace LittleGame.Entity
{
    class ClientBullet : ClientMapObject
    {
        private System.Windows.Forms.PictureBox pictureBox;

        private int endTime;
        private System.Timers.Timer timer;

        private bool end;
        public bool End { get => end; set => end = value; }
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
            this.tileMap = state.tileMap;
            pictureBox = new System.Windows.Forms.PictureBox();
            this.point = new System.Drawing.Point(x, y);
            this.end = false;
            this.face = face;

            // position
            this.vx = 0;
            this.vy = 0;
            this.stepSize = 20;
            this.moveDelay = 0;
            this.moveSpeed = (int)(state.gsm.form.FUpdateTime / 16 * 1);
            this.blocked = false;

            this.endTime = 20;
            this.end = false;
            this.timer = new System.Timers.Timer(100);
            this.timer.Elapsed += count;
            this.timer.Enabled = true;
            this.timer.Start();

            key_up = key_down = key_left = key_right = false;
            if (face == UP)
            {
                key_up = true;
                size = new System.Drawing.Size(5, 20);
            }
            else if (face == DOWN)
            {
                key_down = true;
                size = new System.Drawing.Size(5, 20);
            }
            else if (face == LEFT)
            {
                key_left = true;
                size = new System.Drawing.Size(20, 5);
            }
            else if (face == RIGHT)
            {
                key_right = true;
                size = new System.Drawing.Size(20, 5);
            }

            LoadImage(images[this.face]);
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

        public override void UIUpdate()
        {
            if (!end)
            {
                LoadImage(images[face]);
                pictureBox.BringToFront();
            }
        }
        public override void GameUpdate()
        {
            if (!end)
            {
                Move();
                if (blocked)
                    end = true;
            }
        }

    };
}
