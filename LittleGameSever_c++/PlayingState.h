#pragma once
#include <list>
#include <vector>
#include <iostream>
#include <string>
#include <fstream>
#include "TileMap.h"
#include "Player.h"
#include "Bullet.h"


class LittleGameSever;
class SeverSocketManager;
class PlayingState
{
public:
	bool gameOver;
	int winner;
	std::vector<std::string> maps =
	{
		"Map\\Map1_1.txt"
	};


	static const int maxPlayerNum = 4;
	SeverSocketManager *ssm;
	LittleGameSever *gameSever;

	TileMap tileMap;
	std::vector<Bullet> bullet_List;

	Player players[maxPlayerNum];
	std::string clientMessages[maxPlayerNum];
	int playerNum;
	int aliveNum;

	int playerX[maxPlayerNum] = { 50, 700, 50, 700 };
	int playerY[maxPlayerNum] = { 50, 50, 500, 500 };

	bool GameOver();

	PlayingState();
	PlayingState(LittleGameSever *littlegameSever, SeverSocketManager *ssm, int player_num);
	void AddMessage(int i, std::string message);

	void Update();
};