#include "Bullet.h"

#include "MapObject.h"
#include "PlayingState.h"

bool Bullet::End() { return end; }

Bullet::Bullet(int updateTime, int id, int face, int x, int y)
{
	// parants
	owner = id;

	// position
	px = x;
	py = y;
	vx = 0;
	vy = 0;
	stepSize = 20;
	moveDelay = 0;
	moveSpeed = (int)((float)updateTime / 16 * 1);

	// action
	this->face = face;
	damage = 1;
	blocked = false;

	key_up = key_down = key_left = key_right = false;
	if (face == UP)
	{
		key_up = true;
		width = 5;
		height = 20;
	}
	else if (face == DOWN)
	{
		key_down = true;
		width = 5;
		height = 20;
	}
	else if (face == LEFT)
	{
		key_left = true;
		width = 20;
		height = 5;
	}
	else if (face == RIGHT)
	{
		key_right = true;
		width = 20;
		height = 5;
	}

	end = false;

}

Bullet::~Bullet() {}

void Bullet::Update(PlayingState *playingState)
{
	if (!end)
	{
		Move(playingState);
		if (blocked)
			end = true;
	}
}

bool Bullet::InterSection(MapObject obj)
{
	int x = px, y = py, w = width, h = height;
	if (face == UP)
	{
		h = height + (py - dy);
		y = dy;
	}
	else if (face == DOWN)
	{
		h = height + (dy - py);
	}
	else if (face == LEFT)
	{
		w = width + (px - dx);
		x = dx;
	}
	else if (face == RIGHT)
	{
		w = width + (dx - px);
	}

	int dif_x = abs(obj.Px() + obj.Width() / 2 - (x + w / 2));
	int dif_y = abs(obj.Py() + obj.Height() / 2 - (y + h / 2));
	return dif_x < (w + obj.Width()) / 2 && dif_y < (h + obj.Height()) / 2;
}

void Bullet::Attack(PlayingState *playingState)
{
	for (int i = 0; i < playingState->playerNum; i++)
	{
		if (owner != i && playingState->players[i].Alive() && InterSection(playingState->players[i]))
		{
			playingState->players[i].Hited(damage, playingState);
			end = true;
			return;
		}
	}
}

bool Bullet::Move(PlayingState *playingState)
{
	GetNextPosition();
	blocked = CheckMapCollision(playingState);
	Attack(playingState);
	return SetPosition();
}

