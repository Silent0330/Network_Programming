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
        protected GameStateManager gsm;

        public abstract void update();

        public abstract void keyDown(KeyEventArgs e);

        public abstract void keyUp(KeyEventArgs e);
    }
}
