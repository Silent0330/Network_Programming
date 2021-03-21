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
        public const int AIR = 0;
        public const int WALL = 1;

        //tile images
        private static System.Drawing.Bitmap[] tileImages =
        {
            global::LittleGame.Properties.Resources.grass,
            global::LittleGame.Properties.Resources.block1
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
            if (type == AIR)
            {
                blocked = false;
                loadImage();
            }
            else if(type == WALL)
            {
                blocked = true;
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
            this.pictureBox.Size = new System.Drawing.Size(TileMap.TILESIZE, TileMap.TILESIZE);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
        }
    }
}
