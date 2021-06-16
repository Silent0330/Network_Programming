package com.example.littlegame.Entity;

import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Canvas;
import android.graphics.Paint;
import android.graphics.Rect;
import android.os.Debug;
import android.util.Log;

import com.example.littlegame.R;
import com.example.littlegame.States.ClientPlayingState;

public class Bullet extends ClientMapObject{
    private boolean end;
    public boolean getEnd() { return end; }
    private Bitmap[] bitmaps;

    private int owner;

    public static final int ACTION_UP = 0;
    public static final int ACTION_DOWN = 1;
    public static final int ACTION_LEFT = 2;
    public static final int ACTION_RIGHT = 3;

    private Rect GetBitmapRect() {
        return new Rect(0,0, bitmaps[action].getWidth(), bitmaps[action].getHeight());
    }

    public Bullet(ClientPlayingState state, int owner, int direction, int x, int y) {
        this.state = state;
        this.tileMap  = state.getTileMap();
        this.owner = owner;
        this.direction = direction;
        px = x;
        py = y;
        vx = vy = dx = dy = 0;
        stepSize = 20;
        moveDelay = 0;
        moveSpeed = (int)(16 / (float)state.getGsm().getDrawView().getUpdateTime() * 1);
        end = false;

        moveDelay = 0;
        moveSpeed = 1;

        key_up = key_down = key_left = key_right = false;
        if(direction == ClientMapObject.UP) {
            key_up = true;
            action = ACTION_UP;
            width = 5;
            height = 20;
        }
        else if(direction == ClientMapObject.DOWN) {
            key_down = true;
            action = ACTION_DOWN;
            width = 5;
            height = 20;
        }
        else if(direction == ClientMapObject.LEFT) {
            key_left = true;
            action = ACTION_LEFT;
            width = 20;
            height = 5;
        }
        else if(direction == ClientMapObject.RIGHT) {
            key_right = true;
            action = ACTION_RIGHT;
            width = 20;
            height = 5;
        }

        LoadImage();
    }

    private void LoadImage()
    {
        bitmaps = new Bitmap[4];
        bitmaps[0] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.bulletup);
        bitmaps[1] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.bulletdown);
        bitmaps[2] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.bulletleft);
        bitmaps[3] = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.bulletright);
    }

    public Rect GetMoveRectangle()
    {
        int x = px, y = py, w = width, h = height;
        if(direction == UP)
        {
            h = height + (py - dy);
            y = dy;
        }
        else if (direction == DOWN)
        {
            h = height + (dy - py);
        }
        else if (direction == LEFT)
        {
            w = width + (px - dx);
            x = dx;
        }
        else if (direction == RIGHT)
        {
            w = width + (dx - px);
        }
        return new Rect(x, y, w, h);
    }

    private void Attack()
    {
        for(int i = 0; i < state.getPlayerNum(); i++)
        {
            if (owner != i && state.getPlayer(i).getAlive() && GetMoveRectangle().intersect(state.getPlayer(i).getRectangle()))
            {
                end = true;
                return;
            }
        }
    }

    @Override
    protected boolean Move()
    {
        GetNextPosition();
        blocked = CheckMapCollision();
        Attack();
        return SetPosition();
    }

    @Override
    public void Draw(Canvas canvas) {
        Paint paint = new Paint();
        if (!end) {
            canvas.drawBitmap(bitmaps[action], GetBitmapRect(), getDrawRectangle(), paint);
        }
    }

    @Override
    public void Update() {
        if (!end)
        {
            Move();
            if (blocked)
                end = true;
        }
    }
}
