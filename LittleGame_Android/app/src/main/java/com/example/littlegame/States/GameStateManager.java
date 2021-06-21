package com.example.littlegame.States;

import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.view.MotionEvent;

import com.example.littlegame.ClientSocketManager;
import com.example.littlegame.DrawView;

public class GameStateManager {

    private ClientSocketManager csm;
    private DrawView drawView;
    private GameState[] gameStates;
    private int currentState;
    private int count;

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
        count = 0;

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
        count = 0;
        unloadState(currentState);
        currentState = state;
        loadState(state);
    }

    public void Draw(Canvas canvas)
    {
        Paint paint = new Paint();
        if(gameStates[currentState] != null) {
            gameStates[currentState].Draw(canvas);
        }
        else {
            paint.setColor(Color.BLACK);
            paint.setTextAlign(Paint.Align.CENTER);
            paint.setTextSize(500);
            String text = "Loading";
            for(int i = 0; i < count; i++) {
                text += ".";
            }
            count = (count+1) % 3;
            canvas.drawText(text, drawView.getWidth()/2, drawView.getHeight()/2 - paint.getFontMetrics().ascent / 2, paint);
        }
    }

    public void Update()
    {
        if(gameStates[currentState] != null) {
            gameStates[currentState].Update();
        }
    }

    public void TouchEvent(MotionEvent event)
    {
        if(gameStates[currentState] != null) {
            gameStates[currentState].TouchEvent(event);
        }
    }

}
