#include "Tile.h"
#include <iostream>

Tile::Tile() {}

Tile::Tile(int type = 0)
{
	setTileType(type);
}

int Tile::TileType() { return tileType; }
bool Tile::Blocked() { return blocked; }

void Tile::setTileType(int type)
{
	tileType = type;
	if (type == AIR)
	{
		blocked = false;
	}
	else if (type == WALL)
	{
		blocked = true;
	}
}
