#pragma once

#include "ClientHandler.h"
#include <vector>
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
#include <iostream>
#include <string>
#include <pthread.h>

#define BACKLOG 5

using namespace std;

class LittleGameSever;

static void* ListeningAcept(void* arg);

static void* RecvMessage(void* arg);

class SeverSocketManager
{
private:

	pthread_t listeningThread;
	int maxConnectionNum;
	int curConnectionNum;

public:
	LittleGameSever *gameSever;
	vector<ClientHandler> clientHandler_List;
	vector<int> clientId_List;

	int severSockfd;
	struct addrinfo hints, *servinfo, *p;
	struct sockaddr_storage their_addr;
	socklen_t sin_size;
	struct sigaction sa;
	int yes = 1;
	char s[INET_ADDRSTRLEN];
	int rv;
	bool listening;

	//Connection Properties
	int MaxConnectionNum();
	int CurConnectionNum();
	void SetCurConnectionNum(int value);
	bool Listening();

	SeverSocketManager();
	SeverSocketManager(LittleGameSever *littleGameSever, int maxConnectionNum);

	int StartListening();


	bool SendToClient(int clientId, string message);

	void Dispose();
};

