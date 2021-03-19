using LittleGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LittleGameV1._1.State
{
    class GameStateManager
    {
        public Form1 form;
        public GameState[] gameStates;
        public int currentState;

        public const int NUMGAMESTATE = 3;
        public const int PLAYINGSTATE = 2;
        public const int ROOMSTATE = 1;
        public const int MENUSTATE = 0;

        public GameStateManager(Form1 form)
        {
            this.form = form;

            gameStates = new GameState[NUMGAMESTATE];

            currentState = MENUSTATE;
            loadState(currentState);

        }

        private void loadState(int state)
        {
            if (state == MENUSTATE)
                gameStates[state] = new MenuState(this);
            if (state == ROOMSTATE)
                gameStates[state] = new RoomState(this);
            if (state == PLAYINGSTATE)
                gameStates[state] = new PlayingState(this);
            this.form.Controls.Add(gameStates[currentState]);
            this.form.Focus();
        }

        private void unloadState(int state)
        {
            gameStates[state].Dispose();
            gameStates[state] = null;
        }

        public void setState(int state)
        {
            form.Controls.Remove(gameStates[currentState]);
            unloadState(currentState);
            currentState = state;
            loadState(currentState);
        }

        public void update()
        {
            gameStates[currentState].update();
         
        }

        public void keyDown(KeyEventArgs e)
        {
            gameStates[currentState].keyDown(e);
        }

        public void keyUp(KeyEventArgs e)
        {
            gameStates[currentState].keyUp(e);
        }
        
    }
}
