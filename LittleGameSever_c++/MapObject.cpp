#include "MapObject.h"

#include "PlayingState.h"

MapObject::MapObject() {}

bool MapObject::CheckMapCollision(PlayingState *playingState)
{
	bool blocked = false;
	bool leftBlocked, rightBlocked, upBlocked, downBlocked;
	int l, r, u, d;

	//up down
	l = px / TileMap::TILE_SIZE;
	r = (px + width) / TileMap::TILE_SIZE;
	u = dy / TileMap::TILE_SIZE;
	d = (dy + height) / TileMap::TILE_SIZE;
	if (dy >= 0)
		upBlocked = playingState->tileMap.getBlocked(u, l) || playingState->tileMap.getBlocked(u, r);
	else
		upBlocked = true;
	if (d < TileMap::numRows)
		downBlocked = playingState->tileMap.getBlocked(d, l) || playingState->tileMap.getBlocked(d, r);
	else
		downBlocked = true;
	if ((vy < 0 && upBlocked) || (vy > 0 && downBlocked))
	{
		dy = py;
		vy = 0;
		blocked = true;
	}

	//left right
	l = dx / TileMap::TILE_SIZE;
	r = (dx + width) / TileMap::TILE_SIZE;
	u = dy / TileMap::TILE_SIZE;
	d = (dy + height) / TileMap::TILE_SIZE;
	if (dx >= 0)
		leftBlocked = playingState->tileMap.getBlocked(u, l) || playingState->tileMap.getBlocked(d, l);
	else
		leftBlocked = true;
	if (r < TileMap::numCols)
		rightBlocked = playingState->tileMap.getBlocked(u, r) || playingState->tileMap.getBlocked(d, r);
	else
		rightBlocked = true;
	if ((vx < 0 && leftBlocked) || (vx > 0 && rightBlocked))
	{
		dx = px;
		vx = 0;
		blocked = true;
	}
	return blocked;
}

void MapObject::SetSpeed()
{
	vx = vy = 0;
	if (key_up)
	{
		vy -= stepSize;
	}
	if (key_down)
	{
		vy += stepSize;
	}
	if (key_left)
	{
		vx -= stepSize;
	}
	if (key_right)
	{
		vx += stepSize;
	}
}

void MapObject::GetNextPosition()
{
	if (moveDelay == 0)
	{
		SetSpeed();
		dx = px + vx;
		dy = py + vy;
		if (vx != 0 || vy != 0)
			moveDelay = moveSpeed;
	}
	else moveDelay--;
}

bool MapObject::SetPosition()
{
	bool changed = (px != dx || py != dy);
	if (changed)
	{
		px = dx;
		py = dy;
	}
	return changed;
}

bool MapObject::Move(PlayingState *playingState)
{
	GetNextPosition();
	CheckMapCollision(playingState);
	return SetPosition();
}

int MapObject::Face() { return face; }
int MapObject::Px() { return px; }
int MapObject::Py() { return py; }
int MapObject::Width() { return width; }
int MapObject::Height() { return height; }


bool MapObject::InterSection(MapObject obj)
{
	int dif_x = abs(obj.Px() + obj.Width() / 2 - (px + width / 2));
	int dif_y = abs(obj.Py() + obj.Height() / 2 - (py + height / 2));
	return dif_x < (width + obj.Width()) / 2 && dif_y < (height + obj.Height()) / 2;
}