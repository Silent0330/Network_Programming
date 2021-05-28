#include "LittleGameSever.h"

#pragma region LittleGameSever_CPP

static void* Loop(void* arg)
{
	LittleGameSever* gameSever = (LittleGameSever*)arg;
	while (gameSever->Playing())
	{
		clock_t startTime = clock();
		gameSever->playingState.Update();
		double elapsedSeconds = (clock() - startTime) / CLOCKS_PER_SEC;
		if (elapsedSeconds * 1000 < gameSever->UpdateTime())
		{
			usleep(gameSever->UpdateTime() - (int)(elapsedSeconds * 1000));
		}
		elapsedSeconds = (clock() - startTime) / CLOCKS_PER_SEC;
		gameSever->fps = (1 / elapsedSeconds);
		usleep(30);
	}
	pthread_exit(NULL); // 離開子執行緒
}
void LittleGameSever::Update()
{
	while (!end) {
		if (!playing)
		{
			if (ssm.CurConnectionNum() > 0 && ssm.clientHandler_List[0].startGameRequest)
			{
				if (ssm.CurConnectionNum() > 1)
				{
					cout << "start\n";
					playing = true;
					StartGame();
				}
				ssm.clientHandler_List[0].startGameRequest = false;
			}
		}
		else
		{
			if (playingState.GameOver())
			{
				StopGame();
			}
		}
		sleep(1);
	}
}


int LittleGameSever::UpdateTime() { return updateTime; }
bool LittleGameSever::Playing() { return playing; }

LittleGameSever::LittleGameSever()
{
	playing = false;
	end = false;
	fps = 0;
	ssm = SeverSocketManager(this, 4);
}

void LittleGameSever::StartSever()
{
	int result = ssm.StartListening();
}

void LittleGameSever::StopSever()
{
	ssm.Dispose();
}

void LittleGameSever::StartGame()
{
	for (int i = 0; i < ssm.CurConnectionNum(); i++)
	{
		ssm.SendToClient(i, "Start");
	}
	int count = 0;
	while (true)
	{
		bool ready = true;
		for (int i = 0; i < ssm.CurConnectionNum(); i++)
			if (!ssm.clientHandler_List[i].ReadyToStart())
				ready = false;
		if (ready)
			break;
		cout << "send start\n";
		count++;
		if (count > 10)
		{
			for (int i = 0; i < ssm.CurConnectionNum(); i++)
			{
				ssm.SendToClient(i, "Start");
			}
			count = 0;
		}
		sleep(1);
	}
	cout << "send start done\n";
	playingState = PlayingState(this, &ssm, ssm.CurConnectionNum());
	playing = true;
	pthread_create(&gameThread, NULL, Loop, this); // 建立子執行緒
	cout << "game start done\n";
}

void LittleGameSever::StopGame()
{
	if (playing)
	{
		playing = false;
		for (int i = 0; i < ssm.CurConnectionNum(); i++)
		{
			ssm.SendToClient(i, "GameOver");
		}
	}
	ssm.Dispose();
	ssm.StartListening();
}

void LittleGameSever::Dispose()
{
	end = true;
	if (playing)
	{
		StopGame();
	}
	ssm.Dispose();
}

#pragma endregion
