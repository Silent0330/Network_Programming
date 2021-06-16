package com.example.littlegame;

import android.content.Context;
import android.content.pm.ActivityInfo;
import android.graphics.Canvas;
import android.graphics.Paint;
import android.util.Log;
import android.view.MotionEvent;
import android.view.View;
import android.widget.LinearLayout;

import com.example.littlegame.States.GameStateManager;

public class DrawView extends View {
    private Paint paint;
    private GameStateManager gsm;
    private ClientSocketManager csm;
    private int frameCount;
    private boolean initial;

    private int updateTime;
    public int getUpdateTime() { return updateTime; }

    private Thread thread;

    public ClientSocketManager getCsm() { return csm; }

    public DrawView(Context context)
    {
        super(context);

        csm = new ClientSocketManager();
        paint = new Paint();
        frameCount = 0;
        updateTime = 32;
        initial = false;

    }

    private Runnable Update = new Runnable() {
        @Override
        public void run() {
            while (true) {
                long startTime = System.currentTimeMillis();
                gsm.Update();
                double elapsedMilliSeconds = System.currentTimeMillis() - startTime;
                try {
                    Thread.sleep(updateTime - (int)elapsedMilliSeconds - 4);
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        }
    };

    @Override
    public boolean onTouchEvent(MotionEvent event) {
        if(initial)
            gsm.TouchEvent(event);
        invalidate();
        return true;
    }

    protected void onDraw(Canvas canvas)
    {
        frameCount = (frameCount+1) % 10000;
        if(!initial) {
            gsm = new GameStateManager(this);
            thread = new Thread(Update);
            thread.start();
            initial = true;
        }
        else {
            gsm.Draw(canvas);
        }
        try {
            Thread.sleep(updateTime);
            invalidate();
        }catch(Exception e) {
            e.printStackTrace();
        }
    }
}
