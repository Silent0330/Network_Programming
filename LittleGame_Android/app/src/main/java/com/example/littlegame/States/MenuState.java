package com.example.littlegame.States;

import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Rect;
import android.hardware.camera2.params.BlackLevelPattern;
import android.util.Log;
import android.view.MotionEvent;

import com.example.littlegame.DrawView;
import com.example.littlegame.GameActivity;
import com.example.littlegame.R;

public class MenuState extends GameState{

    private Rect ipTextRect;
    private Rect joinButtonRect;
    private Rect exitButtonRect;
    private MyKeyBoard keyBoard;
    private String ipText;

    public MenuState(GameStateManager gsm) {
        this.gsm = gsm;
        ipTextRect = new Rect(gsm.getDrawView().getWidth()*8/32, gsm.getDrawView().getHeight()*4/32, gsm.getDrawView().getWidth()*24/32, gsm.getDrawView().getHeight()*8/32);
        joinButtonRect = new Rect(gsm.getDrawView().getWidth()*24/32, gsm.getDrawView().getHeight()*4/32, gsm.getDrawView().getWidth()*28/32, gsm.getDrawView().getHeight()*8/32);
        exitButtonRect = new Rect(gsm.getDrawView().getWidth()*12/32, gsm.getDrawView().getHeight()*24/32, gsm.getDrawView().getWidth()*20/32, gsm.getDrawView().getHeight()*28/32);
        keyBoard = new MyKeyBoard(gsm.getDrawView());
        ipText = "192.168.0.196";
    }

    @Override
    public void Update() {
        if(gsm.getCsm().getConnected()) {
            gsm.SetState(GameStateManager.CLIENTROOMSTATE);
        }
    }

    @Override
    public void Draw(Canvas canvas) {
        Paint paint = new Paint();
        paint.setColor(Color.BLACK);
        paint.setStyle(Paint.Style.STROKE);
        paint.setStrokeWidth(5);
        canvas.drawRect(ipTextRect, paint);

        paint.setStyle(Paint.Style.FILL);
        paint.setTextAlign(Paint.Align.CENTER);
        paint.setTextSize(50);
        canvas.drawText(ipText, ipTextRect.centerX(), ipTextRect.centerY() - paint.getFontMetrics().ascent / 2, paint);

        paint.setStyle(Paint.Style.STROKE);
        canvas.drawRect(joinButtonRect, paint);

        paint.setStyle(Paint.Style.FILL);
        paint.setTextAlign(Paint.Align.CENTER);
        canvas.drawText("Join", joinButtonRect.centerX(), joinButtonRect.centerY() - paint.getFontMetrics().ascent / 2, paint);

        if(!keyBoard.input) {
            paint.setStyle(Paint.Style.STROKE);
            canvas.drawRect(exitButtonRect, paint);

            paint.setStyle(Paint.Style.FILL);
            paint.setTextAlign(Paint.Align.CENTER);
            canvas.drawText("Exit", exitButtonRect.centerX(), exitButtonRect.centerY() - paint.getFontMetrics().ascent / 2, paint);
        }

        keyBoard.Draw(canvas);
    }

    @Override
    public void TouchEvent(MotionEvent event) {
        switch(event.getAction()) {
            case MotionEvent.ACTION_DOWN:
                if(keyBoard.input) {
                    String ret = keyBoard.KeyDown((int)event.getX(), (int)event.getY());
                    if(ret == "del" && ipText.length() > 0) {
                        ipText = ipText.substring(0, ipText.length()-1);
                    }
                    else if(ret == "done") {
                        keyBoard.input = false;
                    }
                    else {
                        ipText += ret;
                    }
                }
                else if (ipTextRect.contains((int)event.getX(), (int)event.getY())) {
                    keyBoard.input = true;
                }
                else if (joinButtonRect.contains((int)event.getX(), (int)event.getY())) {
                    gsm.getCsm().StartConnect(ipText, 7777);
                }
                else if (exitButtonRect.contains((int)event.getX(), (int)event.getY())) {
                   android.os.Process.killProcess(android.os.Process.myPid());
                   System.exit(0);
                }
                break;
            case MotionEvent.ACTION_UP:
                break;
            case MotionEvent.ACTION_MOVE:
                break;
        }
    }

    private class MyKeyBoard {
        private DrawView drawView;
        private InputButton[] inputButtons;
        public boolean input;

        public MyKeyBoard(DrawView drawView) {
            this.drawView = drawView;
            input = false;
            inputButtons = new InputButton[13];
            for(int i = 0; i < 10; i++) {
                inputButtons[i] = new InputButton(drawView, (i+1) * drawView.getWidth()*2/32, drawView.getHeight()*24/32, Integer.toString(i));
            }
            inputButtons[10] = new InputButton(drawView, drawView.getWidth()*22/32, drawView.getHeight()*24/32, ".");
            inputButtons[11] = new InputButton(drawView, drawView.getWidth()*24/32, drawView.getHeight()*24/32, "del");
            inputButtons[12] = new InputButton(drawView, drawView.getWidth()*26/32, drawView.getHeight()*24/32, "done");
        }

        public String KeyDown(int x, int y) {
            if(drawView.getWidth()*2/32 <= x && x <= drawView.getWidth()*28/32 && y >= drawView.getHeight()*24/32 && y <= drawView.getHeight()*26/32) {
                return inputButtons[(x-drawView.getWidth()*2/32)/(drawView.getWidth()*2/32)].getText();
            }
            return "";
        }

        public void Draw(Canvas canvas){
            if(input) {
                for(int i = 0; i < 13; i++) {
                    inputButtons[i].Draw(canvas);
                }
            }
        }
    }

    private class InputButton {
        private Rect rect;
        private String text;

        public String getText() { return text; }

        InputButton(DrawView drawView, int x, int y, String text) {
            rect = new Rect(x, y, x+gsm.getDrawView().getWidth()*2/32, y+gsm.getDrawView().getHeight()*2/32);
            this.text = text;
        }

        public void Draw(Canvas canvas) {
            Paint paint = new Paint();
            paint.setColor(Color.BLACK);
            paint.setStyle(Paint.Style.STROKE);
            paint.setStrokeWidth(5);
            canvas.drawRect(rect, paint);

            paint.setTextAlign(Paint.Align.CENTER);
            paint.setTextSize(50);
            canvas.drawText(text, rect.centerX(), rect.centerY() - paint.getFontMetrics().ascent / 2, paint);
        }
    }
}
