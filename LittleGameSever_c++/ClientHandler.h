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
#include <semaphore.h>

using namespace std;

class ClientHandler
{
private:
	int id;
	bool disConnectChecked;

	bool up;
	bool down;
	bool left;
	bool right;
	bool attack;
	bool reload;
	bool readyToStart;
	bool startGameRequest;

	sem_t startGameRequest_mu;
	sem_t send_mu;

public:
	int sockfd;
	bool connected;

	bool Connected();
	bool DisConnectChecked();

	int Id();
	bool ReadyToStart();
	bool Up();
	bool Down();
	bool Left();
	bool Right();
	bool Attack();
	bool Reload();
	bool StartGameRequest();

	void SetId(int value);
	void SetReadyToStart(bool value);
	void SetUp(bool value);
	void SetDown(bool value);
	void SetLeft(bool value);
	void SetRight(bool value);
	void SetAttack(bool value);
	void SetReload(bool value);
	void SetStartGameRequest(bool value);


	ClientHandler();

	ClientHandler(int clientId, int clientSocket);


	void RecvMessage(string message);
	bool SendMessage(string message);

	void CheckConnection();

	void Dispose();
};

