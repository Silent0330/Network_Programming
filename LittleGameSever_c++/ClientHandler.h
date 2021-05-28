#pragma once

#include <string>
#include <vector>
#include <iostream>
#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <errno.h>
#include <string.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <netdb.h>
#include <arpa/inet.h>
#include <sys/wait.h>
#include <signal.h>
#include <pthread.h>
#include <mutex>

using namespace std;


class ClientHandler
{
private:
	int id;
	bool disConnectChecked;


public:
	int sockfd;
	bool connected;

	bool Connected();
	bool DisConnectChecked();

	bool readyToStart;
	bool up;
	bool down;
	bool left;
	bool right;
	bool attack;
	bool reload;
	bool ReadyToStart();
	bool Up();
	bool Down();
	bool Left();
	bool Right();
	bool Attack();
	bool Reload();
	bool startGameRequest;

	ClientHandler();

	ClientHandler(int clientId, int clientSocket);

	void ChangeId(int new_id);

	bool SendMessage(string message);

	void CheckConnection();

	void Dispose();
};

