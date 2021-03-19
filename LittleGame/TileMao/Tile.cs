using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleGame.TileMap
{
    class Tile
    {
        //tile types
        public const int GRASS = 0;
        public const int TREE = 1;
        public const int BOX1 = 2;
        public const int TRI = 3;
        public const int BOX2 = 4;
        public const int GROUND = 5;

        //tile images
        private static System.Drawing.Bitmap[] tileImages =
         {

         };

        private int tileType;
        private bool blocked;
        private System.Drawing.Point point;
            
        public System.Windows.Forms.PictureBox pictureBox;

        public Tile(int type, int x, int y)
        {
            this.pictureBox = new System.Windows.Forms.PictureBox();
            point = new System.Drawing.Point(x, y);
            setTileType(type);
        }

        public void setTileType(int type)
        {
            tileType = type;
            if (type == GRASS)
            {
                blocked = false;
                loadImage();
            }
            else if(type == TREE)
            {
                blocked = true;
                loadImage();
            }
            else if (type == BOX1)
            {
                blocked = true;
                loadImage();
            }
            else if (type == TRI)
            {
                blocked = true;
                loadImage();
            }
            else if (type == BOX2)
            {
                blocked = true;
                loadImage();
            }
            else if (type == GROUND)
            {
                blocked = false;
                loadImage();
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

        private void loadImage()
        {
            this.pictureBox.Image = tileImages[tileType];
            this.pictureBox.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox.Location = point;
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(50, 50);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
        }
    }
}
