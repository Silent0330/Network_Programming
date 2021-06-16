package com.example.littlegame.Tiles;

import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Canvas;
import android.graphics.Paint;
import android.graphics.Rect;

import com.example.littlegame.R;
import com.example.littlegame.States.ClientPlayingState;

public class TileMap {
    private ClientPlayingState state;
    public static final int numCols = 16;
    public static final int numRows = 12;
    private Tile[][] tiles;
    public static final int TILE_SIZE = 50;

    private Bitmap bitmap;

    public int getWidth() { return numCols * TILE_SIZE; }
    public int getHeight() { return numRows * TILE_SIZE; }

    public static final int[][] map1 = {
        {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        {0,0,1,0,0,1,1,0,0,1,1,0,0,0,0,0},
        {0,0,0,0,0,1,1,0,0,1,1,0,0,0,1,0},
        {0,0,0,1,1,1,0,0,0,0,1,1,1,0,0,0},
        {0,0,0,1,1,0,0,0,0,0,0,1,1,0,0,1},
        {1,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0},
        {0,0,0,1,1,0,0,0,0,0,0,1,1,0,0,0},
        {0,0,0,1,1,1,0,0,0,0,1,1,1,0,0,0},
        {0,1,0,0,0,1,1,0,0,1,1,0,0,0,0,0},
        {0,0,0,0,0,1,1,0,0,1,1,0,0,0,0,0},
        {1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1}
    };

    public Rect getBitMapRect() {
        return new Rect(0,0, bitmap.getWidth(), bitmap.getHeight());
    }

    public Rect getDrawRect() {
        return new Rect(0,0, state.getGsm().getDrawView().getWidth(), state.getGsm().getDrawView().getHeight());
    }

    public TileMap(ClientPlayingState state)
    {
        this.state = state;
        bitmap = BitmapFactory.decodeResource(state.getGsm().getDrawView().getResources(), R.drawable.map1_1);
        tiles = new Tile[numRows][numCols];
        for (int i = 0; i < numRows; i++)
        {
            for(int j = 0; j < numCols; j++)
            {
                tiles[i][j] = new Tile(map1[i][j]);
            }
        }
    }

    public void Draw(Canvas canvas) {
        Paint paint = new Paint();
        canvas.drawBitmap(bitmap, getBitMapRect(), getDrawRect(), paint);
    }


    public boolean getBlocked(int row, int col)
    {
        if(row >= 0 && row < numRows && col >= 0 && col < numCols)
            return tiles[row][col].getBlocked();
        return true;
    }

    public int getTileType(int row, int col)
    {
        return tiles[row][col].getTileType();
    }
}
