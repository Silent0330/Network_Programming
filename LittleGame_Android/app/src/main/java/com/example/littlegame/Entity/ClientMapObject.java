package com.example.littlegame.Entity;

import android.graphics.Canvas;
import android.graphics.Rect;

import com.example.littlegame.States.ClientPlayingState;
import com.example.littlegame.Tiles.TileMap;

public abstract class ClientMapObject {
    protected ClientPlayingState state;
    protected TileMap tileMap;

    //position and move
    protected int px, py;
    public int getPx() { return px; }
    public int getPy() { return py; }
    protected int width, height;
    public int getWidth() { return width; }
    public int getHeight() { return height; }
    protected int vx, vy;
    protected int stepSize;
    protected int dx, dy;
    protected int moveDelay;
    protected int moveSpeed;
    protected boolean blocked;
    public boolean getBlocked() { return blocked; }

    //control
    protected boolean key_up;
    protected boolean key_down;
    protected boolean key_left;
    protected boolean key_right;


    public boolean getKey_Up() { return key_up; }
    public void setKey_Up(boolean value) {  key_up = value; }
    public boolean getKey_Down() { return key_down; }
    public void setKey_Down(boolean value) {  key_down = value; }
    public boolean getKey_Left() { return key_left; }
    public void setKey_Left(boolean value) {  key_left = value; }
    public boolean getKey_Right() { return key_right; }
    public void setKey_Right(boolean value) {  key_right = value; }

    // action
    protected int direction;
    public int getDirection() { return direction; }
    public void setDirection(int value) { direction = value; }
    protected int action;
    protected int getAction() { return action; };
    protected void setAction(int value) { action = value; };

    public static final int UP = 0;
    public static final int DOWN = 1;
    public static final int LEFT = 2;
    public static final int RIGHT = 3;

    public Rect getRectangle()
    {
        return new Rect(px, py, px + width, py + height);
    }
    public Rect getDrawRectangle()
    {
        return new Rect(px * state.getGsm().getDrawView().getWidth() / tileMap.getWidth(), py * state.getGsm().getDrawView().getHeight() / tileMap.getHeight(), (px + width) * state.getGsm().getDrawView().getWidth() / tileMap.getWidth(), (py + height) * state.getGsm().getDrawView().getHeight() / tileMap.getHeight());
    }

    public boolean InterSection(ClientMapObject obj)
    {
        Rect rec1 = getRectangle();
        Rect rec2 = obj.getRectangle();
        return rec1.intersect(rec2);
    }

    public void GetNextPosition()
    {
        if (moveDelay == 0)
        {
            SetSpeed();
            dx = px + vx;
            dy = py + vy;
            if (vx != 0 || vy != 0)
                moveDelay = moveSpeed;
        }
        else {
            moveDelay--;
        }
    }

    public void SetSpeed()
    {
        vx = vy = 0;
        if (key_up)
        {
            vy -= stepSize;
        }
        if (key_down)
        {
            vy += stepSize;
        }
        if (key_left)
        {
            vx -= stepSize;
        }
        if (key_right)
        {
            vx += stepSize;
        }
    }

    protected boolean CheckMapCollision()
    {
        boolean result = false;
        boolean leftBlocked, rightBlocked, upBlocked, downBlocked;
        int l, r, u, d;

        //up down
        l = px / TileMap.TILE_SIZE;
        r = (px + width) / TileMap.TILE_SIZE;
        u = dy / TileMap.TILE_SIZE;
        d = (dy + height) / TileMap.TILE_SIZE;
        if (dy >= 0)
            upBlocked = tileMap.getBlocked(u, l) || tileMap.getBlocked(u, r);
        else
            upBlocked = true;
        if (d < TileMap.numRows)
            downBlocked = tileMap.getBlocked(d, l) || tileMap.getBlocked(d, r);
        else
            downBlocked = true;
        if ((vy < 0 && upBlocked) || (vy > 0 && downBlocked))
        {
            dy = py;
            vy = 0;
            result = true;
        }

        //left right
        l = dx / TileMap.TILE_SIZE;
        r = (dx + width) / TileMap.TILE_SIZE;
        u = dy / TileMap.TILE_SIZE;
        d = (dy + height) / TileMap.TILE_SIZE;
        if (dx >= 0)
            leftBlocked = tileMap.getBlocked(u, l) || tileMap.getBlocked(d, l);
        else
            leftBlocked = true;
        if (r < TileMap.numCols)
            rightBlocked = tileMap.getBlocked(u, r) || tileMap.getBlocked(d, r);
        else
            rightBlocked = true;
        if ((vx < 0 && leftBlocked) || (vx > 0 && rightBlocked))
        {
            dx = px;
            vx = 0;
            result = true;
        }
        return result;
    }

    protected boolean SetPosition()
    {
        boolean changed = (px != dx || py != dy);
        if(changed)
        {
            px = dx;
            py = dy;
        }
        return changed;
    }

    protected boolean Move()
    {
        GetNextPosition();
        blocked = CheckMapCollision();
        return SetPosition();
    }

    public abstract void Draw(Canvas canvas);
    public abstract void Update();
}
