package com.example.littlegame.States;

import android.graphics.Canvas;
import android.view.KeyEvent;
import android.view.MotionEvent;

import com.example.littlegame.ClientSocketManager;
import com.example.littlegame.DrawView;
import com.example.littlegame.GameActivity;

public class GameStateManager {

    private ClientSocketManager csm;
    private DrawView drawView;
    private GameState[] gameStates;
    private int currentState;

    public DrawView getDrawView() { return drawView; }
    public ClientSocketManager getCsm() { return csm; }

    public static final int NUMGAMESTATE = 3;
    public static final int CLIENTPLAYINGSTATE = 2;
    public static final int CLIENTROOMSTATE = 1;
    public static final int MENUSTATE = 0;


    public GameStateManager(DrawView drawView)
    {
        this.drawView = drawView;
        this.csm = drawView.getCsm();

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
    }

    private void unloadState(int state)
    {
        gameStates[state] = null;
    }

    public void SetState(int state)
    {
        int s = currentState;
        loadState(state);
        currentState = state;
        unloadState(s);
    }

    public void Draw(Canvas canvas)
    {
        gameStates[currentState].Draw(canvas);
    }

    public void Update()
    {
        gameStates[currentState].Update();
    }

    public void TouchEvent(MotionEvent event)
    {
        gameStates[currentState].TouchEvent(event);
    }

}
