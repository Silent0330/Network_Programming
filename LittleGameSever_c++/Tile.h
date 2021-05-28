#pragma once

class Tile
{
private:
	int tileType;
	bool blocked;

public:
	//tile types
	static const int AIR = 0;
	static const int WALL = 1;


	Tile();
	Tile(int type);

	int TileType();
	bool Blocked();
	void setTileType(int type);
};
