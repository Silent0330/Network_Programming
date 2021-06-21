package com.example.littlegame.Entity;

import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Canvas;
import android.graphics.Paint;
import android.graphics.Rect;
import android.util.Log;

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
    private int animateCount;

    private Bitmap[] bitmaps;


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
        animateCount = -1;

        px = x;
        py = y;
        dx = px;
        dy = py;
        width = 50;
        height = 50;
        vx = vy = 0;
        stepSize = 20;

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
            bitmaps = new Bitmap[12];
            bitmaps[0] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p1up);
            bitmaps[1] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p1moveup_1);
            bitmaps[2] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p1moveup_2);
            bitmaps[3] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p1down);
            bitmaps[4] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p1movedown_1);
            bitmaps[5] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p1movedown_2);
            bitmaps[6] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p1left);
            bitmaps[7] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p1moveleft_1);
            bitmaps[8] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p1moveleft_2);
            bitmaps[9] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p1right);
            bitmaps[10] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p1moveright_1);
            bitmaps[11] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p1moveright_2);
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
        else if(id == 2) {
            bitmaps = new Bitmap[8];
            bitmaps[0] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p3up);
            bitmaps[1] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p3down);
            bitmaps[2] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p3left);
            bitmaps[3] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p3right);
            bitmaps[4] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p3moveup);
            bitmaps[5] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p3movedown);
            bitmaps[6] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p3moveleft);
            bitmaps[7] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p3moveright);
        }
        else if(id == 3) {
            bitmaps = new Bitmap[8];
            bitmaps[0] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p4up);
            bitmaps[1] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p4down);
            bitmaps[2] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p4left);
            bitmaps[3] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p4right);
            bitmaps[4] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p4moveup);
            bitmaps[5] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p4movedown);
            bitmaps[6] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p4moveleft);
            bitmaps[7] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.p4moveright);
        }
    }

    public void SetDestinationPoint(int x, int y)
    {
       dx = x;
       dy = y;
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
        if(id == 0) {
            if(vx != 0 || vy != 0) {
                animateCount = (animateCount+1) % 2;
                action = direction*3 + 1 + animateCount;
            }
            else {
                action = action = direction*3;
                animateCount = -1;
            }
        }
    }

    @Override
    public void Draw(Canvas canvas) {
        Paint paint = new Paint();
        SetAction();
        if (alive) {
            canvas.drawBitmap(bitmaps[action], getBitmapRect(), getDrawRectangle(), paint);
        }
    }

    private void MoveToDestination() {
        if(px != dx){
            vx = dx - px;
            px = dx;
        }
        else {
            vx = 0;
        }
        if(py != dy){
            vy = dy - py;
            py = dy;
        }
        else {
            vy = 0;
        }
    }

    @Override
    public void Update() {
        if (alive)
        {
            MoveToDestination();
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
