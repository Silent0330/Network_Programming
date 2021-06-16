package com.example.littlegame.States;

import android.graphics.Canvas;
import android.view.KeyEvent;
import android.view.MotionEvent;

public abstract class GameState {
    protected GameStateManager gsm;
    public GameStateManager getGsm() { return gsm; };

    public abstract void Update();

    public abstract void Draw(Canvas canvas);

    public abstract void TouchEvent(MotionEvent event);
}
