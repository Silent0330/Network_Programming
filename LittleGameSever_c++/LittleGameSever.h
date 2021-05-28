#pragma once
#include <time.h>
#include <list>
#include <vector>
#include <iostream>
#include <string>
#include <pthread.h>
#include <sys/time.h>
#include <mutex>
#include <unistd.h>
#include "SeverSocketManager.h"
#include "PlayingState.h"

using namespace std;

static void* Loop(void* arg);

class LittleGameSever {
private:
	int updateTime = 16;
	bool playing;
	bool end;
	pthread_t gameThread;


public:
	PlayingState playingState;
	SeverSocketManager ssm;
	int UpdateTime();
	bool Playing();

	void Update();
	double fps;

	LittleGameSever();

	void StartSever();

	void StopSever();

	void StartGame();

	void StopGame();

	void Dispose();
};