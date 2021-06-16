package com.example.littlegame.States;

import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Rect;
import android.os.Bundle;
import android.util.Log;
import android.view.KeyEvent;
import android.view.MotionEvent;

import com.example.littlegame.GameActivity;
import com.example.littlegame.MainActivity;
import com.example.littlegame.R;

public class ClientRoomState extends GameState{
    private int playerNum;
    private int playerId;
    private Rect backButtonRect;
    private Rect startButtonRect;
    private Rect[] subpanelRects;

    private static Bitmap[] playerIconBitmaps = {};

    public ClientRoomState(GameStateManager gsm) {
        this.gsm = gsm;
        playerNum = 0;
        playerId = -1;
        backButtonRect = new Rect(0, gsm.getDrawView().getHeight()*28/32, gsm.getDrawView().getWidth()*4/32, gsm.getDrawView().getBottom());
        startButtonRect = new Rect(gsm.getDrawView().getWidth()*28/32, gsm.getDrawView().getHeight()*28/32, gsm.getDrawView().getRight(), gsm.getDrawView().getBottom());

        subpanelRects = new Rect[4];
        for(int i = 0; i < 4; i++) {
            subpanelRects[i] = new Rect(0, gsm.getDrawView().getHeight()*i*4/32, gsm.getDrawView().getWidth()*12/32, gsm.getDrawView().getHeight()*(i+1)*4/32);
        }
        playerIconBitmaps = new Bitmap[4];
        playerIconBitmaps[0] = BitmapFactory.decodeResource(gsm.getDrawView().getResources(), R.drawable.p1down);
        playerIconBitmaps[1] = BitmapFactory.decodeResource(gsm.getDrawView().getResources(), R.drawable.p2down);
        playerIconBitmaps[2] = BitmapFactory.decodeResource(gsm.getDrawView().getResources(), R.drawable.p3down);
        playerIconBitmaps[3] = BitmapFactory.decodeResource(gsm.getDrawView().getResources(), R.drawable.p4down);

    }

    public Rect GetBitmapRect(Bitmap bitmap) {
        return new Rect(0, 0, bitmap.getWidth(), bitmap.getHeight());
    }

    @Override
    public void Update()
    {
        if (!gsm.getCsm().getConnected())
        {
            gsm.SetState(GameStateManager.MENUSTATE);
        }
        if (gsm.getCsm().getGameStart())
        {
            gsm.SetState(GameStateManager.CLIENTPLAYINGSTATE);
        }

        playerId = gsm.getCsm().getPlayerId();

        playerNum = gsm.getCsm().getPlayerNum();


    }

    @Override
    public void Draw(Canvas canvas) {
        Paint paint = new Paint();
        paint.setTextAlign(Paint.Align.CENTER);
        paint.setTextSize(50);

        for(int i = 0; i < playerNum; i++) {
            if(i != playerId) {
                paint.setColor(Color.argb(125, 0, 255, 0));
            }
            else {
                paint.setColor(Color.argb(125, 255, 0, 0));
            }
            canvas.drawRect(subpanelRects[i], paint);
            paint.setColor(Color.BLACK);
            canvas.drawText("P" + (i+1), subpanelRects[i].left + subpanelRects[i].width()*2/32, subpanelRects[i].centerY() - paint.getFontMetrics().ascent / 2, paint);
            canvas.drawBitmap(playerIconBitmaps[i], GetBitmapRect(playerIconBitmaps[i]),
                    new Rect(subpanelRects[i].left+ subpanelRects[i].width()*4/32, subpanelRects[i].top, subpanelRects[i].left+ subpanelRects[i].width()*12/32, subpanelRects[i].bottom), paint);
        }

        paint.setColor(Color.CYAN);
        canvas.drawRect(backButtonRect, paint);
        paint.setColor(Color.BLACK);
        canvas.drawText("back", backButtonRect.centerX(), backButtonRect.centerY() - paint.getFontMetrics().ascent / 2, paint);

        if(playerId == 0) {
            paint.setColor(Color.CYAN);
            canvas.drawRect(startButtonRect, paint);
            paint.setColor(Color.BLACK);
            canvas.drawText("start", startButtonRect.centerX(), startButtonRect.centerY() - paint.getFontMetrics().ascent / 2, paint);
        }

    }

    @Override
    public void TouchEvent(MotionEvent event) {
        switch(event.getAction()) {
            case MotionEvent.ACTION_DOWN:
                if (backButtonRect.contains((int) event.getX(), (int) event.getY())) {
                    gsm.getCsm().Disconnet();
                    gsm.SetState(GameStateManager.MENUSTATE);
                }
                if (startButtonRect.contains((int) event.getX(), (int) event.getY())) {
                    gsm.getCsm().AddMessageToSend("StartGame");
                }
                break;
            case MotionEvent.ACTION_UP:
                break;
            case MotionEvent.ACTION_MOVE:
                break;
        }
    }

}
