#pragma once

#include "MapObject.h"
#include "SeverSocketManager.h"

class Player : public MapObject
{
	// info
private:
	int id;
	bool alive;
	int hp;
	bool key_attack;
	bool key_reload;
	int attackDelay;
	int attackSpeed;
	int bulletCount;
	int maxBulletCount;
	int reloadingTime;
	int reloadingDownCount;

public:
	int Id();
	bool Alive();
	int Hp();
	bool Key_Attack();
	bool Key_Reload();

	Player();

	Player(int updateTime, int id, int x, int y);

	void Hited(int damage, PlayingState *playingState);

	bool SetAction();

	void SetControll(SeverSocketManager *ssm);

	void Attack(PlayingState *playingState);

	void Update(PlayingState *playingState);

};
