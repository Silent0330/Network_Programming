#pragma once
#include "TileMap.h"

class PlayingState;

class MapObject
{
protected:
	//position and move
	int updateTime;
	int px;
	int py;
	int height, width;
	int vx, vy;
	int stepSize = 5;
	int dx, dy;
	int moveDelay;
	int moveSpeed;
	int face;

	bool CheckMapCollision(PlayingState *playingState);

	void SetSpeed();

	void GetNextPosition();

	bool SetPosition();

	bool Move(PlayingState *playingState);

public:
	static const int UP = 0;
	static const int DOWN = 1;
	static const int LEFT = 2;
	static const int RIGHT = 3;
	static const int MOVEUP = 4;
	static const int MOVEDOWN = 5;
	static const int MOVELEFT = 6;
	static const int MOVERIGHT = 7;

	bool key_up;
	bool key_down;
	bool key_left;
	bool key_right;

	int Face();
	int Px();
	int Py();
	int Width();
	int Height();

	MapObject();

	bool InterSection(MapObject obj);

};

