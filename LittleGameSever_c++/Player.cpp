#include "Player.h"

#include "PlayingState.h"
#include "Bullet.h"
#include <fcntl.h>
#include "Utils.h"

int Player::Id() { return id; }
bool Player::Alive() { return alive; }
int Player::Hp() { return hp; }
bool Player::Key_Attack() { return key_attack; }
bool Player::Key_Reload() { return key_reload; }

Player::Player() {}

Player::Player(int updateTime, int id, int x, int y)
{
	//parants
	//position
	this->updateTime = updateTime;
	px = x;
	py = y;
	vx = 0;
	vy = 0;
	stepSize = 10;
	moveDelay = 0;
	moveSpeed = (int)((float)updateTime / 16 * 2);

	//info
	this->id = id;
	alive = true;
	hp = 1;

	//action
	key_up = false;
	key_down = false;
	key_left = false;
	key_right = false;
	key_attack = false;
	key_reload = false;
	face = DOWN;

	//attack
	attackSpeed = (int)((float)updateTime / 16 * 50);
	attackDelay = 0;
	maxBulletCount = 6;
	bulletCount = maxBulletCount;
	reloadingTime = (int)((float)updateTime / 16 * 100);
	reloadingDownCount = 0;

	//rectangle
	width = 40;
	height = 40;

}

void Player::Hited(int damage, PlayingState *playingState)
{
	if (alive)
	{
		hp -= damage;
		if (hp <= 0)
		{
			alive = false;
			playingState->aliveNum--;
			for (int i = 0; i < playingState->playerNum; i++)
			{
				playingState->AddMessage(i, "Dead," + to_string(id));
			}
		}
	}
}

bool Player::SetAction()
{
	if (key_up)
	{
		if (face != UP)
		{
			face = UP;
			return true;
		}
	}
	else if (key_down)
	{
		if (face != DOWN)
		{
			face = DOWN;
			return true;
		}
	}
	else if (key_left)
	{
		if (face != LEFT)
		{
			face = LEFT;
			return true;
		}
	}
	else if (key_right)
	{
		if (face != RIGHT)
		{
			face = RIGHT;
			return true;
		}
	}
	return false;
}

void Player::SetControll(SeverSocketManager *ssm)
{
	if (id < ssm->clientHandler_List.size() && ssm->clientHandler_List[id].Connected())
	{
		key_up = ssm->clientHandler_List[id].Up();
		key_down = ssm->clientHandler_List[id].Down();
		key_left = ssm->clientHandler_List[id].Left();
		key_right = ssm->clientHandler_List[id].Right();
		key_attack = ssm->clientHandler_List[id].Attack();
		key_reload = ssm->clientHandler_List[id].Reload();
	}
	
}

void Player::Attack(PlayingState *playingState)
{
	if (reloadingDownCount == 0)
	{
		if (key_attack)
		{
			if (bulletCount > 0 && attackDelay == 0)
			{
				playingState->bullet_List.push_back(Bullet(updateTime, id, face, px + width / 2, py + height / 2));
				bulletCount--;
				attackDelay = attackSpeed;
				for (int i = 0; i < playingState->playerNum; i++)
				{
					playingState->AddMessage(i, "Attack," + std::to_string(id));
				}
			}
		}
		if (key_reload)
		{
			if (bulletCount != maxBulletCount)
			{
				reloadingDownCount = reloadingTime;
				for (int i = 0; i < playingState->playerNum; i++)
				{
					playingState->AddMessage(i, "Reload," + std::to_string(id));
				}
			}
			key_reload = false;
		}
	}
	else
	{
		reloadingDownCount--;
		if (reloadingDownCount == 0)
		{
			bulletCount = maxBulletCount;
			for (int i = 0; i < playingState->playerNum; i++)
			{
				playingState->AddMessage(i, "ReloadDone," + std::to_string(id));
			}
		}
	}
	if (attackDelay > 0)
		attackDelay--;
}

void Player::Update(PlayingState *playingState)
{
	if (Alive())
	{
		SetControll(playingState->ssm);
		if (Move(playingState))
		{
			for (int i = 0; i < playingState->playerNum; i++)
			{
				playingState->AddMessage(i, "Move," + std::to_string(id) + "," + std::to_string(px) + "," + std::to_string(py));
			}
		}
		Attack(playingState);
		if (SetAction())
		{
			for (int i = 0; i < playingState->playerNum; i++)
			{
				playingState->AddMessage(i, "Face," + std::to_string(id) + "," + std::to_string(face));
			}
		}
	}
}
