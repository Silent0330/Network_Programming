#include "PlayingState.h"
#include "LittleGameSever.h"
#include <pthread.h>

using namespace std;

void *mainLoop(void* arg);

int main()
{
	LittleGameSever littleGameSever = LittleGameSever();
	littleGameSever.StartSever();
	pthread_t mainLoop_t;
	pthread_create(&mainLoop_t, NULL, mainLoop, &littleGameSever); // 建立子執行緒
	int in;
	while (true) {
		cin >> in;
		if (in == 0) {
			break;
		}
		if (in == 1) {
			if(littleGameSever.Playing())
				littleGameSever.StopGame();
			littleGameSever.StopSever();
		}
		if (in == 2) {
			if (littleGameSever.Playing()) {
				littleGameSever.StopGame();
				littleGameSever.StopSever();
			}
			littleGameSever.StartSever();
		}
	}
	littleGameSever.Dispose();
}

void *mainLoop(void* arg) {
	LittleGameSever* littleGameSever = (LittleGameSever*)arg;
	littleGameSever->Update();
	pthread_exit(NULL); // 離開子執行緒
}
