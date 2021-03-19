using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LittleGame.State
{
    class PlayingState : GameState
    {
        public TileMap.TileMap tileMap;

        private bool key_up;
        private bool key_down;
        private bool key_left;
        private bool key_right;

        public PlayingState(GameStateManager gsm)
        {
            this.gsm = gsm;
            key_left = false;
            key_up = false;
            key_right = false;
            key_down = false;
        }

        public override void update()
        {

        }

        public override void keyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) { key_up = true; }
            if (e.KeyCode == Keys.S) { key_down = true; }
            if (e.KeyCode == Keys.A) { key_left = true; }
            if (e.KeyCode == Keys.D) { key_right = true; }
            if (e.KeyCode == Keys.Space) { key_left = true; }
            
        }

        public override void keyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) { key_up = false; }
            if (e.KeyCode == Keys.S) { key_down = false; }
            if (e.KeyCode == Keys.A) { key_left = false; }
            if (e.KeyCode == Keys.D) { key_right = false; }
            if (e.KeyCode == Keys.Space) { key_left = false; }

        }

    }
}
