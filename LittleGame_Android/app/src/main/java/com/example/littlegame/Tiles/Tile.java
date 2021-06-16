package com.example.littlegame.Tiles;

public class Tile {
    //tile types
    public static final int AIR = 0;
    public static final int WALL = 1;

    private int tileType;
    private boolean blocked;

    public Tile(int type)
    {
        setTileType(type);
    }

    public void setTileType(int type)
    {
        tileType = type;
        if (type == AIR)
        {
            blocked = false;
        }
        else if(type == WALL)
        {
            blocked = true;
        }
    }

    public int getTileType()
    {
        return tileType;
    }

    public boolean getBlocked()
    {
        return blocked;
    }
}
