﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LittleGame.TileMaps
{
    class TileMap
    {
        public const int numCols = 32;
        public const int numRows = 24;
        private Tile[,] tiles;
        public int[,] objMap;
        public const int TILE_SIZE = 25;

        public TileMap(string map)
        {
            objMap = new int[numRows, numCols];

            StreamReader str = new StreamReader(map);
            string read;
            tiles = new Tile[numRows , numCols];
            for (int i = 0; i < numRows; i++)
            {
                read = str.ReadLine();
                for(int j = 0; j < numCols; j++)
                {
                    tiles[i,j] = new Tile(read[j]- '0');
                    this.objMap[i, j] = 0;
                }
            }
        }

        public bool getBlocked(int row, int col)
        {
            return tiles[row,col].getBlocked() || (objMap[row, col] != 0);
        }

        public int getTileType(int row, int col)
        {
            return tiles[row, col].getTileType();
        }
        
    }
}