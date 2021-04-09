using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LittleGame.State
{

    abstract class GameState : Panel
    {
        public GameStateManager gsm;

        public abstract void Update();

        public abstract void KeyDown(KeyEventArgs e);

        public abstract void KeyUp(KeyEventArgs e);
    }
}
