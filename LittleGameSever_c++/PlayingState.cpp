#include "PlayingState.h"

#include "LittleGameSever.h"
#include "SeverSocketManager.h"
#include "Player.h"
#include "Bullet.h"

bool PlayingState::GameOver() { return gameOver; }

PlayingState::PlayingState() {}

PlayingState::PlayingState(LittleGameSever *littlegameSever, SeverSocketManager *ssm, int player_num)
{
	this->gameSever = littlegameSever;
	this->ssm = ssm;
	tileMap = TileMap(maps[0]);
	bullet_List = vector<Bullet>();

	this->playerNum = player_num;
	for (int i = 0; i < playerNum; i++)
	{
		players[i] = Player(littlegameSever->UpdateTime(), i, playerX[i], playerY[i]);
	}
	aliveNum = playerNum;
	gameOver = false;
	winner = 0;
}

void PlayingState::AddMessage(int i, string message)
{
	clientMessages[i] += message + ";";
}

void PlayingState::Update()
{
	if (!gameOver)
	{
		for (int i = 0; i < playerNum; i++)
		{
			clientMessages[i] = "";
		}
		for (int i = 0; i < playerNum; i++)
		{
			players[i].Update(this);
		}
		for (int i = 0; i < bullet_List.size(); i++)
		{
			bullet_List[i].Update(this);
			if (bullet_List[i].End())
			{
				bullet_List[i].~Bullet();
				bullet_List.erase(bullet_List.begin() + i);
				for (int j = 0; j < playerNum; j++)
				{
					AddMessage(j, "BulletRemove," + to_string(i));
				}
				i--;
			}
		}
		if (aliveNum <= 1)
		{
			for (int i = 0; i < playerNum; i++)
			{
				if (players[i].Alive())
				{
					winner = i + 1;
				}
			}
			gameOver = true;
			for (int i = 0; i < playerNum; i++)
			{
				AddMessage(i, "GameOver");
			}
		}
		if (ssm->CurConnectionNum() < 1)
		{
			gameOver = true;
		}
	}
	for (int i = 0; i < playerNum; i++)
	{
		if (clientMessages[i].length() > 0)
		{
			if (!ssm->SendToClient(i, clientMessages[i]))
			{
				cout << i + "failed ";
			}
		}
	}
}