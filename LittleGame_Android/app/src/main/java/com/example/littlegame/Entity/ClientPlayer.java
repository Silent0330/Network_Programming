package com.example.littlegame.Entity;

import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Canvas;
import android.graphics.Paint;
import android.graphics.Rect;

import com.example.littlegame.R;
import com.example.littlegame.States.ClientPlayingState;

public class ClientPlayer extends ClientMapObject{
    private Bitmap pictureBox;
    private int id;
    private boolean alive;
    public boolean getAlive() { return alive; }
    private int hp;

    private boolean attack;
    public void setAttack(boolean value) { attack = value; }
    private boolean reload;
    public boolean getReload() { return reload; }
    public void setReload(boolean value) { reload = value; }
    private boolean reloadDone;
    public boolean getReloadDone() { return reloadDone; }
    public void setReloadDone(boolean value) { reloadDone = value; }
    private int bulletCount;
    public int getBulletCount() { return bulletCount; }
    private int maxBulletCount;

    private Bitmap[] bitmaps;

    public void setPoint(int x, int y)
    {
        px = x;
        py = y;
    }

    public static final int ACTION_UP = 0;
    public static final int ACTION_DOWN = 1;
    public static final int ACTION_LEFT = 2;
    public static final int ACTION_RIGHT = 3;
    public static final int ACTION_MOVE_UP = 4;
    public static final int ACTION_MOVE_DOWN = 5;
    public static final int ACTION_MOVE_LEFT = 6;
    public static final int ACTION_MOVE_RIGHT = 7;

    public ClientPlayer(ClientPlayingState state, int id, int x, int y)
    {
        this.state = state;
        this.tileMap  = state.getTileMap();
        this.id = id;
        alive = true;
        direction = DOWN;
        action = ACTION_DOWN;
        px = x;
        py = y;
        vx = vy = dx = dy = 0;
        stepSize = 20;
        width = 50;
        height = 50;
        hp = 1;
        attack = false;
        reload = false;
        reloadDone = false;
        bulletCount = 6;
        maxBulletCount = 6;

        key_up = key_down = key_left = key_right = false;

        LoadImage();
    }

    private Rect getBitmapRect() {
        return new Rect(0,0, bitmaps[action].getWidth(), bitmaps[action].getHeight());
    }

    private void LoadImage()
    {
        if(id == 0) {
            bitmaps = new Bitmap[8];
            bitmaps[0] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p1up);
            bitmaps[1] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p1down);
            bitmaps[2] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p1left);
            bitmaps[3] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p1right);
            bitmaps[4] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p1moveup);
            bitmaps[5] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p1movedown);
            bitmaps[6] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p1moveleft);
            bitmaps[7] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p1moveright);
        }
        else if(id == 1) {
            bitmaps = new Bitmap[8];
            bitmaps[0] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p2up);
            bitmaps[1] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p2down);
            bitmaps[2] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p2left);
            bitmaps[3] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p2right);
            bitmaps[4] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p2moveup);
            bitmaps[5] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p2movedown);
            bitmaps[6] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p2moveleft);
            bitmaps[7] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p2moveright);
        }
    }

    public void Hitted(int damage) {
        if (alive)
        {
            hp -= damage;
            if(hp <=0)
            {
                alive = false;
                state.setAliveNum(state.getAliveNum()-1);
            }
        }
    }

    public void Attack() {
        state.getBullets().add(new Bullet(state, id, direction, px + width / 2, py + height / 2));
        bulletCount--;
    }

    private void SetAction() {
        action = direction;
    }

    @Override
    public void Draw(Canvas canvas) {
        Paint paint = new Paint();
        if (alive) {
            canvas.drawBitmap(bitmaps[action], getBitmapRect(), getDrawRectangle(), paint);
        }
    }

    @Override
    public void Update() {
        if (alive)
        {
            SetAction();
            if(attack) {
                attack = false;
            }
            if (reloadDone)
            {
                bulletCount = maxBulletCount;
                reloadDone = false;
                reload = false;
            }
        }
    }
}
