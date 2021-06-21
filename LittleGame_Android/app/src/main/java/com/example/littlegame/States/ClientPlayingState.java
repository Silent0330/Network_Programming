package com.example.littlegame.States;

import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Path;
import android.graphics.Rect;
import android.view.MotionEvent;

import com.example.littlegame.Entity.Bullet;
import com.example.littlegame.Entity.ClientPlayer;
import com.example.littlegame.Tiles.TileMap;

import java.util.Vector;

public class ClientPlayingState extends GameState{
    private TileMap tileMap;
    public TileMap getTileMap() {return tileMap;}
    private ClientPlayer[] players;
    public ClientPlayer getPlayer(int i ) {
        if(i >= 0 && i < 4) return players[i];
        return null;
    }
    private Vector<Bullet> bullets;
    public Vector<Bullet> getBullets() { return bullets; };
    private int playerNum;
    public int getPlayerNum() { return playerNum; }
    private int aliveNum;
    public int getAliveNum() { return aliveNum; }
    public void setAliveNum(int value) { aliveNum = value; }
    public boolean gameOver;

    private boolean drag;
    private float dragX, dragY, touchX, touchY;
    private float angle;
    private Rect atkBtnRect, reloadBtnRect;

    private int playerId;
    private int winner;

    //control state
    private boolean key_up;
    private boolean key_down;
    private boolean key_left;
    private boolean key_right;
    private boolean key_attack;
    private boolean key_reload;

    private static final int[][] playerPoints = {
            {50, 50},
            {700, 50},
            {50, 500},
            {700, 500}
    };

    public ClientPlayingState(GameStateManager gsm) {
        this.gsm = gsm;
        playerId = gsm.getCsm().getPlayerId();
        playerNum = gsm.getCsm().getPlayerNum();
        aliveNum = playerNum;
        tileMap = new TileMap(this);
        players = new ClientPlayer[4];
        bullets = new Vector<Bullet>();
        gameOver = false;
        key_up = key_down = key_left = key_right = key_attack = key_reload = false;

        for (int i = 0; i < playerNum; i++)
        {
            players[i] = new ClientPlayer(this, i, playerPoints[i][0], playerPoints[i][1]);
        }

        drag = false;
        dragX = dragY = touchX = touchY = 0;
        angle = 0;


        int buttonRadius = gsm.getDrawView().getHeight()*2/32;
        atkBtnRect = new Rect(gsm.getDrawView().getWidth()*28/32 - 2*buttonRadius, gsm.getDrawView().getHeight()*28/32 - 2*buttonRadius, gsm.getDrawView().getWidth()*28/32 + 2*buttonRadius, gsm.getDrawView().getHeight()*28/32 + 2*buttonRadius);
        reloadBtnRect = new Rect(gsm.getDrawView().getWidth()*30/32 - buttonRadius, gsm.getDrawView().getHeight()*28/32 - 4*buttonRadius, gsm.getDrawView().getWidth()*30/32 + buttonRadius, gsm.getDrawView().getHeight()*28/32 - 2*buttonRadius);

        winner = 0;;
        gsm.getCsm().AddMessageToSend("Ready," + Boolean.toString(true));
    }

    @Override
    public void Update() {
        if(!gsm.getCsm().getConnected()) {
            gameOver = true;
        }
        if (!gameOver)
        {
            for(int i = 0; i < 20 && gsm.getCsm().recivedMessages.size() > 0; i++) {
                String message = gsm.getCsm().recivedMessages.remove(0);
                String[] messages = message.split(";");
                for (int j = 0; j < messages.length; j++)
                {
                    String[] messageArgs = messages[j].split(",");

                    if (messageArgs[0].equals("Move"))
                    {
                        players[Integer.parseInt(messageArgs[1])].SetDestinationPoint(Integer.parseInt(messageArgs[2]), Integer.parseInt(messageArgs[3]));
                    }
                    else if (messageArgs[0].equals("Face"))
                    {
                        players[Integer.parseInt(messageArgs[1])].setDirection(Integer.parseInt(messageArgs[2]));
                    }
                    else if (messageArgs[0].equals("Attack"))
                    {
                        players[Integer.parseInt(messageArgs[1])].Attack();;
                    }
                    else if (messageArgs[0].equals("Reload"))
                    {
                        players[Integer.parseInt(messageArgs[1])].setReload(true);
                    }
                    else if (messageArgs[0].equals("ReloadDone"))
                    {
                        players[Integer.parseInt(messageArgs[1])].setReloadDone(true);
                    }
                    else if (messageArgs[0].equals("Hitted"))
                    {
                        players[Integer.parseInt(messageArgs[1])].Hitted(Integer.parseInt(messageArgs[2]));
                    }
                }
            }
            if (!gsm.getCsm().getConnected())
            {
                gsm.SetState(GameStateManager.MENUSTATE);
            }

            players[playerId].setKey_Up(key_up);
            players[playerId].setKey_Down(key_down);
            players[playerId].setKey_Left(key_left);
            players[playerId].setKey_Right(key_right);
            players[playerId].setAttack(key_attack);
            players[playerId].setReload(key_reload);

            for (int i = 0; i < playerNum; i++)
            {
                players[i].Update();
            }
            for (int i = 0; i < bullets.size(); i++)
            {
                bullets.elementAt(i).Update();
                if(bullets.elementAt(i).getEnd()){
                    bullets.remove(i);
                    i--;
                }
            }
            if (aliveNum < 2 || !gsm.getCsm().getGameStart())
            {
                gameOver = true;
                winner = 0;
                for (int i = 0; i < playerNum; i++)
                {
                    if (players[i].getAlive())
                    {
                        winner = i + 1;
                    }
                }
            }
        }
    }

    @Override
    public void Draw(Canvas canvas) {
        Paint paint = new Paint();
        if(!gameOver) {
            tileMap.Draw(canvas);
            for (int i = 0; i < playerNum; i++) {
                players[i].Draw(canvas);
            }
            for (int i = 0; i < bullets.size(); i++) {
                bullets.elementAt(i).Draw(canvas);
            }

            paint.setColor(Color.BLACK);
            paint.setTextAlign(Paint.Align.CENTER);
            paint.setTextSize(50);
            if(!players[playerId].getReload())
                canvas.drawText(players[playerId].getBulletCount() + "", players[playerId].getDrawRectangle().centerX(), players[playerId].getDrawRectangle().top - 5  - paint.getFontMetrics().ascent / 2, paint);
            else
                canvas.drawText("R", players[playerId].getDrawRectangle().centerX(), players[playerId].getDrawRectangle().top - 5  - paint.getFontMetrics().ascent / 2, paint);

            if(key_attack)
                paint.setColor(Color.argb(0.5f, 0.3f, 0.3f, 0.3f));
            else
                paint.setColor(Color.argb(0.5f, 0.5f, 0.5f, 0.5f));
            canvas.drawCircle(atkBtnRect.centerX(), atkBtnRect.centerY(), atkBtnRect.width()/2, paint);
            if(key_reload)
                paint.setColor(Color.argb(0.5f, 0.3f, 0.3f, 0.3f));
            else
                paint.setColor(Color.argb(0.5f, 0.5f, 0.5f, 0.5f));
            canvas.drawCircle(reloadBtnRect.centerX(), reloadBtnRect.centerY(), reloadBtnRect.width()/2, paint);

            if(drag){
                paint.setColor(Color.argb(0.5f, 0.5f, 0.5f, 0.5f));
                canvas.drawCircle(dragX, dragY, 100, paint);
                paint.setStrokeWidth(10);
                Path path = new Path();
                path.moveTo(dragX + (float)Math.cos((angle+90) / 180 * Math.PI) * 100, dragY + -(float)Math.sin((angle+90) / 180 * Math.PI) * 100);
                path.quadTo(touchX, touchY, dragX + (float)Math.cos((angle-90) / 180 * Math.PI) * 100, dragY + -(float)Math.sin((angle-90) / 180 * Math.PI) * 100);
                canvas.drawPath(path, paint);
            }

        }
        else {
            paint.setColor(Color.BLACK);
            paint.setTextAlign(Paint.Align.CENTER);
            paint.setTextSize(50);
            if(winner != 0)
                canvas.drawText("P" + winner + " is winner", gsm.getDrawView().getWidth()/2, gsm.getDrawView().getHeight()/2  - paint.getFontMetrics().ascent / 2, paint);
            else
                canvas.drawText("Tie", gsm.getDrawView().getWidth()/2, gsm.getDrawView().getHeight()/2  - paint.getFontMetrics().ascent / 2, paint);
        }
    }

    @Override
    public void TouchEvent(MotionEvent event) {
        int ind = -1;
        float x, y;
        switch(event.getAction()) {
            case MotionEvent.ACTION_DOWN:
                if(ind == -1)
                    ind = 0;
            case MotionEvent.ACTION_POINTER_2_DOWN:
                if(ind == -1)
                    ind = 1;
                x = event.getX(event.findPointerIndex(event.getPointerId(ind)));
                y = event.getY(event.findPointerIndex(event.getPointerId(ind)));
                if (!gameOver) {
                    if(!drag && x < gsm.getDrawView().getWidth()/2) {
                        touchX = dragX = x;
                        touchY = dragY = y;
                        drag = true;
                    }
                    if(!key_attack){
                        if(atkBtnRect.contains((int)x, (int)y)) {
                            key_attack = true;
                            gsm.getCsm().AddMessageToSend("Attack,true");
                        }
                    }
                    if(!key_reload){
                        if(reloadBtnRect.contains((int)x, (int)y)) {
                            key_reload = true;
                            gsm.getCsm().AddMessageToSend("Reload,true");
                        }
                    }
                }
                else {
                    gsm.getCsm().Disconnet();
                    gsm.SetState(GameStateManager.MENUSTATE);
                }
                break;
            case MotionEvent.ACTION_UP:
                if (!gameOver) {
                    if(drag){
                        drag = false;
                        if (key_up) {
                            key_up = false;
                            gsm.getCsm().AddMessageToSend("Up,false");
                        }
                        if (key_down) {
                            key_down = false;
                            gsm.getCsm().AddMessageToSend("Down,false");
                        }
                        if (key_left) {
                            key_left = false;
                            gsm.getCsm().AddMessageToSend("Left,false");
                        }
                        if (key_right) {
                            key_right = false;
                            gsm.getCsm().AddMessageToSend("Right,false");
                        }
                    }
                    if (key_attack) {
                        key_attack = false;
                        gsm.getCsm().AddMessageToSend("Attack,false");
                    }
                    if (key_reload) {
                        key_reload = false;
                        gsm.getCsm().AddMessageToSend("Reload,false");
                    }
                }
                break;
            case MotionEvent.ACTION_MOVE:
                if (!gameOver) {
                    if(drag) {
                        touchX = event.getX(0);
                        touchY = event.getY(0);
                        float dx = touchX - dragX;
                        float dy = -(touchY - dragY);
                        if(dx != 0) {
                            angle = (float) (Math.atan(dy / dx) / Math.PI * 180);
                        }
                        else {
                            if(dy > 0)
                                angle = 90;
                            else
                                angle = 270;
                        }
                        if(dx < 0) {
                            angle += 180;
                        }
                        angle = (angle + 360) % 360;

                        if((angle >= 0 && angle < 20)  || (angle >= 340 && angle < 360)) {
                            if(!key_right) {
                                key_right = true;
                                gsm.getCsm().AddMessageToSend("Right,true");
                            }
                            if(key_up) {
                                key_up = false;
                                gsm.getCsm().AddMessageToSend("Up,false");
                            }
                            if(key_down) {
                                key_down = false;
                                gsm.getCsm().AddMessageToSend("Down,false");
                            }
                            if(key_left) {
                                key_left = false;
                                gsm.getCsm().AddMessageToSend("Left,false");
                            }
                        }
                        else if(angle >= 20 && angle < 70) {
                            if(!key_right) {
                                key_right = true;
                                gsm.getCsm().AddMessageToSend("Right,true");
                            }
                            if(!key_up) {
                                key_up = true;
                                gsm.getCsm().AddMessageToSend("Up,true");
                            }
                            if(key_down) {
                                key_down = false;
                                gsm.getCsm().AddMessageToSend("Down,false");
                            }
                            if(key_left) {
                                key_left = false;
                                gsm.getCsm().AddMessageToSend("Left,false");
                            }
                        }
                        else if(angle >= 70 && angle < 110) {
                            if(!key_up) {
                                key_up = true;
                                gsm.getCsm().AddMessageToSend("Up,true");
                            }
                            if(key_down) {
                                key_down = false;
                                gsm.getCsm().AddMessageToSend("Down,false");
                            }
                            if(key_left) {
                                key_left = false;
                                gsm.getCsm().AddMessageToSend("Left,false");
                            }
                            if(key_right) {
                                key_right = false;
                                gsm.getCsm().AddMessageToSend("Right,false");
                            }
                        }
                        else if(angle >= 110 && angle < 160) {
                            if(!key_left) {
                                key_left = true;
                                gsm.getCsm().AddMessageToSend("Left,true");
                            }
                            if(!key_up) {
                                key_up = true;
                                gsm.getCsm().AddMessageToSend("Up,true");
                            }
                            if(key_down) {
                                key_down = false;
                                gsm.getCsm().AddMessageToSend("Down,false");
                            }
                            if(key_right) {
                                key_right = false;
                                gsm.getCsm().AddMessageToSend("Right,false");
                            }
                        }
                        else if(angle >= 160 && angle < 200) {
                            if(!key_left) {
                                key_left = true;
                                gsm.getCsm().AddMessageToSend("Left,true");
                            }
                            if(key_up) {
                                key_up = false;
                                gsm.getCsm().AddMessageToSend("Up,false");
                            }
                            if(key_down) {
                                key_down = false;
                                gsm.getCsm().AddMessageToSend("Down,false");
                            }
                            if(key_right) {
                                key_right = false;
                                gsm.getCsm().AddMessageToSend("Right,false");
                            }
                        }
                        else if(angle >= 200 && angle < 250) {
                            if(!key_left) {
                                key_left = true;
                                gsm.getCsm().AddMessageToSend("Left,true");
                            }
                            if(!key_down) {
                                key_down = true;
                                gsm.getCsm().AddMessageToSend("Down,true");
                            }
                            if(key_up) {
                                key_up = false;
                                gsm.getCsm().AddMessageToSend("Up,false");
                            }
                            if(key_right) {
                                key_right = false;
                                gsm.getCsm().AddMessageToSend("Right,false");
                            }
                        }
                        else if(angle >= 250 && angle < 290) {
                            if(!key_down) {
                                key_down = true;
                                gsm.getCsm().AddMessageToSend("Down,true");
                            }
                            if(key_up) {
                                key_up = false;
                                gsm.getCsm().AddMessageToSend("Up,false");
                            }
                            if(key_left) {
                                key_left = false;
                                gsm.getCsm().AddMessageToSend("Left,false");
                            }
                            if(key_right) {
                                key_right = false;
                                gsm.getCsm().AddMessageToSend("Right,false");
                            }
                        }
                        else {
                            if(!key_down) {
                                key_down = true;
                                gsm.getCsm().AddMessageToSend("Down,true");
                            }
                            if(!key_right) {
                                key_right = true;
                                gsm.getCsm().AddMessageToSend("Right,true");
                            }
                            if(key_up) {
                                key_up = false;
                                gsm.getCsm().AddMessageToSend("Up,false");
                            }
                            if(key_left) {
                                key_left = false;
                                gsm.getCsm().AddMessageToSend("Left,false");
                            }
                        }
                    }
                    if(key_attack) {
                        key_attack = false;
                        gsm.getCsm().AddMessageToSend("Attack,false");
                    }
                    if(key_reload) {
                        key_reload = false;
                        gsm.getCsm().AddMessageToSend("Reload,false");
                    }
                }
                break;
        }
    }
}
