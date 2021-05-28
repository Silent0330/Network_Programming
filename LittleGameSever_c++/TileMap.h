#pragma once

#include <string>
#include <fstream>
#include <iostream>
#include "Tile.h"


using namespace std;

class TileMap
{
public:
	static const int numCols = 16;
	static const int numRows = 12;
	static const int TILE_SIZE = 50;
	Tile tiles[numRows][numCols];

	TileMap();

	TileMap(string map);

	bool getBlocked(int row, int col);

	int getTileType(int row, int col);

};

