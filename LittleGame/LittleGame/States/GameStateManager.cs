using LittleGame;
using LittleGame.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LittleGame.State
{
    class GameStateManager
    {
        public ClientSocketManager csm;

        public Form1 form;
        public GameState[] gameStates;
        public int currentState;

        public const int NUMGAMESTATE = 3;
        public const int CLIENTPLAYINGSTATE = 2;
        public const int CLIENTROOMSTATE = 1;
        public const int MENUSTATE = 0;

        public GameStateManager(Form1 form, ClientSocketManager csm)
        {
            this.form = form;
            this.csm = csm;

            gameStates = new GameState[NUMGAMESTATE];

            currentState = MENUSTATE;
            loadState(currentState);

        }

        private void loadState(int state)
        {
            if (state == MENUSTATE)
                gameStates[state] = new MenuState(this);
            if (state == CLIENTROOMSTATE)
                gameStates[state] = new ClientRoomState(this);
            if (state == CLIENTPLAYINGSTATE)
                gameStates[state] = new ClientPlayingState(this);
            this.form.Controls.Add(gameStates[currentState]);
            this.form.Focus();
        }

        private void unloadState(int state)
        {
            gameStates[state].Dispose();
            gameStates[state] = null;
        }

        public void SetState(int state)
        {
            form.Controls.Remove(gameStates[currentState]);
            unloadState(currentState);
            currentState = state;
            loadState(currentState);
        }

        public void Update()
        {
            gameStates[currentState].Update();
         
        }

        public void KeyDown(KeyEventArgs e)
        {
            gameStates[currentState].KeyDown(e);
        }

        public void KeyUp(KeyEventArgs e)
        {
            gameStates[currentState].KeyUp(e);
        }
        
    }
}
