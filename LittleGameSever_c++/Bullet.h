#pragma once
#include "MapObject.h"

class Bullet : public MapObject
{
private:
	bool end;
	int owner;
	int damage;
	bool blocked;

public:
	bool End();

	Bullet(int updateTime, int id, int face, int x, int y);

	~Bullet();

	void Update(PlayingState *playingState);

	bool InterSection(MapObject obj);

	void Attack(PlayingState *playingState);

	bool Move(PlayingState *playingState);

};


