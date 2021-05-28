#include "TileMap.h"

TileMap::TileMap() {};

TileMap::TileMap(string map)
{
	fstream mapfile(map);
	if (!mapfile.is_open()) {
		cout << "no map\n";
		exit(-1);
	}
	char buff[256];
	for (int i = 0; i < numRows; i++)
	{
		mapfile.getline(buff, sizeof(buff));
		for (int j = 0; j < numCols; j++)
		{
			tiles[i][j] = Tile(buff[j]-'0');
		}
	}
}

bool TileMap::getBlocked(int row, int col)
{
	if (row >= 0 && row < numRows && col >= 0 && col < numCols)
		return tiles[row][col].Blocked();
	return true;
}

int TileMap::getTileType(int row, int col)
{
	return tiles[row][col].TileType();
}
