#include "SeverSocketManager.h"#include "LittleGameSever.h"

vector<string> split(const string& str, const string& delim) {
	vector<string> res;
	if ("" == str) return res;
	char * strs = new char[str.length() + 1];
	strcpy(strs, str.c_str());

	char * d = new char[delim.length() + 1];
	strcpy(d, delim.c_str());

	char *p = strtok(strs, d);
	while (p) {
		string s = p;
		res.push_back(s);
		p = strtok(NULL, d);
	}

	return res;
}

//Connection Properties
int SeverSocketManager::MaxConnectionNum() { return maxConnectionNum; }
int SeverSocketManager::CurConnectionNum() { return curConnectionNum; }
void SeverSocketManager::SetCurConnectionNum(int value) {
	curConnectionNum = value;
	for (int i = 0; i < clientHandler_List.size(); i++)
	{
		clientHandler_List[i].SendMessage("PlayerNum," + to_string(curConnectionNum));
	}
}
bool SeverSocketManager::Listening() { return listening; }

SeverSocketManager::SeverSocketManager() {}
SeverSocketManager::SeverSocketManager(LittleGameSever *littleGameSever, int maxConnectionNum)
{
	gameSever = littleGameSever;
	severSockfd = -1;

	if (maxConnectionNum > 0)
	{
		this->maxConnectionNum = maxConnectionNum;
	}
	else
	{
		maxConnectionNum = 0;
	}

	clientHandler_List = vector<ClientHandler>();
	clientId_List = vector<int>();

	curConnectionNum = 0;
	listening = false;

	//pthread_create(&recvThread, NULL, RecvMessage, this); // 建立子執行緒
	cout << ("Sever is ready for start\n");
}

void sigchld_handler(int s) {
	while (waitpid(-1, NULL, WNOHANG) > 0);
}

void *get_in_addr(struct sockaddr *sa) {
	if (sa->sa_family == AF_INET) {
		return &(((struct sockaddr_in*)sa)->sin_addr);
	}
	return &(((struct sockaddr_in6*)sa)->sin6_addr);
}



int SeverSocketManager::StartListening()
{
	int sockfd;
	memset(&hints, 0, sizeof hints);
	hints.ai_family = AF_INET;
	hints.ai_socktype = SOCK_STREAM;
	hints.ai_flags = AI_PASSIVE;

	if ((rv = getaddrinfo(NULL, "7777", &hints, &servinfo)) != 0) {
		fprintf(stderr, "getaddrinfo: %s\n", gai_strerror(rv));
		return 1;
	}

	for (p = servinfo; p != NULL; p = p->ai_next) {
		if ((sockfd = socket(p->ai_family, p->ai_socktype, p->ai_protocol)) == -1) {
			perror("server: socket");
			continue;
		}
		if (setsockopt(sockfd, SOL_SOCKET, SO_REUSEADDR, &yes, sizeof(int)) == -1) {
			perror("setsockopt");
			return 2;
		}
		if (bind(sockfd, p->ai_addr, p->ai_addrlen) == -1) {
			close(sockfd);
			perror("server: bind");
			continue;
		}

		break;
	}
	if (p == NULL) {
		fprintf(stderr, "server: failed to bind\n");
		return 3;
	}

	freeaddrinfo(servinfo);

	if (listen(sockfd, BACKLOG) == -1) {
		perror("listen");
		return 4;
	}

	sa.sa_handler = sigchld_handler;
	sigemptyset(&sa.sa_mask);
	sa.sa_flags = SA_RESTART;

	if (sigaction(SIGCHLD, &sa, NULL) == -1) {
		perror("sigaction");
		return 5;
	}
	severSockfd = sockfd;

	listening = true;
	pthread_create(&listeningThread, NULL, ListeningAcept, this); // 建立子執行緒
	cout << "Sever is listening\n";

	return 0;
}

static void* ListeningAcept(void* arg)
{
	SeverSocketManager* ssm = (SeverSocketManager*)arg;
	int datasize = 2048;
	char buf[datasize];
	string message;
	memset(buf, 0, sizeof(buf));

	//文件描述符集合
	fd_set read_fds;
	fd_set  exception_fds;
	FD_ZERO(&read_fds);
	FD_ZERO(&exception_fds);

	struct timeval timeout;
	timeout.tv_sec = 30;             //妙
	timeout.tv_usec = 0;            //微秒

	int max_fd = ssm->severSockfd;

	while (ssm->Listening()) {
		FD_ZERO(&read_fds);
		FD_ZERO(&exception_fds);
		FD_SET(ssm->severSockfd, &read_fds);
		for (int i = 0;i < ssm->clientHandler_List.size();++i)
		{
			if (ssm->clientHandler_List[i].Connected()) {
				FD_SET(ssm->clientHandler_List[i].sockfd, &read_fds);
				FD_SET(ssm->clientHandler_List[i].sockfd, &exception_fds);
			}
		}


		int ret = select(max_fd + 1, &read_fds, NULL, &exception_fds, NULL);
		if (ret == -1)
		{
			perror("select");
			break;
		}
		else if (ret == 0)
		{
			perror("timeout");
			break;
		}

		cout << "client nums = " << ssm->clientHandler_List.size() << "\n";
		for (int i = 0; i < ssm->clientHandler_List.size(); i++)
		{
			ClientHandler* handler = &(ssm->clientHandler_List[i]);
			memset(buf, 0, sizeof(buf));
			cout << "sockfd = " << handler->sockfd << "\n";
			if (FD_ISSET(handler->sockfd, &read_fds))
			{
				int ret = recv(handler->sockfd, buf, sizeof(buf) - 1, 0);
				if (ret <= 0)
				{
					perror("recv");
					close(handler->sockfd);
					FD_CLR(handler->sockfd, &read_fds);
					if (!handler->DisConnectChecked()) {
						handler->Dispose();
						ssm->SetCurConnectionNum(ssm->CurConnectionNum() - 1);
					}
					if (!ssm->gameSever->Playing())
					{
						ssm->clientHandler_List.erase(ssm->clientHandler_List.begin() + i);
						ssm->clientId_List.erase(ssm->clientId_List.begin() + i);
						for (int j = i; j < ssm->clientHandler_List.size(); j++)
						{
							ssm->clientHandler_List[j].ChangeId(j);
							ssm->clientId_List[j] = j;
						}
					}
					i--;
					printf("A client disconnectd\n");
					continue;
				}

				buf[ret] = ';';
				message = string(buf);
				cout << "recv client = " << i << " sock = " << handler->sockfd << " message = " << message << "\n";
				if (handler->connected)
				{
					vector<string> messages = split(message, ";");
					for (int i = 0; i < messages.size(); i++)
					{
						vector<string> messageArgs = split(messages[i], ",");
						if (messageArgs[0] == ("Up"))
						{
							if (messageArgs[1] == "True")
							{
								handler->up = true;
							}
							else
							{
								handler->up = false;
							}
						}
						else if (messageArgs[0] == ("Down"))
						{
							if (messageArgs[1] == "True")
							{
								handler->down = true;
							}
							else
							{
								handler->down = false;
							}
						}
						else if (messageArgs[0] == ("Left"))
						{
							if (messageArgs[1] == "True")
							{
								handler->left = true;
							}
							else
							{
								handler->left = false;
							}
						}
						else if (messageArgs[0] == ("Right"))
						{
							if (messageArgs[1] == "True")
							{
								handler->right = true;
							}
							else
							{
								handler->right = false;
							}
						}
						else if (messageArgs[0] == ("Attack"))
						{
							if (messageArgs[1] == "True")
							{
								handler->attack = true;
							}
							else
							{
								handler->attack = false;
							}
						}
						else if (messageArgs[0] == ("Reload"))
						{
							if (messageArgs[1] == "True")
							{
								handler->reload = true;
							}
							else
							{
								handler->reload = false;
							}
						}
						else if (messageArgs[0] == ("StartGame"))
						{
							handler->startGameRequest = true;
							cout << "recv start : " << handler->startGameRequest << "\n";
						}
						else if (messageArgs[0] == ("Ready"))
						{
							if (messageArgs[1] == "True")
							{
								handler->readyToStart = true;
							}
							else
							{
								handler->readyToStart = false;
							}
						}
					}
				}
			}
			else  if (FD_ISSET(handler->sockfd, &exception_fds))
			{
				int ret = recv(handler->sockfd, buf, sizeof(buf) - 1, MSG_OOB);
				if (ret < 0)
				{
					perror("exception");
				}
				else
				{
					buf[ret] = '\0';
					printf("%s", buf);
				}

			}
		}

		if (FD_ISSET(ssm->severSockfd, &read_fds)) {
			ssm->sin_size = sizeof ssm->their_addr;
			int new_fd = accept(ssm->severSockfd, (struct sockaddr *)&(ssm->their_addr), &(ssm->sin_size));
			if (new_fd == -1) {
				perror("accept");
				break;
			}
			inet_ntop(ssm->their_addr.ss_family, get_in_addr((struct sockaddr *)&(ssm->their_addr)), ssm->s, sizeof ssm->s);
			printf("server: got connection form %s\n", ssm->s);

			if (!ssm->Listening())
			{
				string message = "Fail";
				if (send(new_fd, message.c_str(), message.length(), 0) == -1) {
					perror("others send");
				}
				close(new_fd);
				break;
			}

			if (ssm->CurConnectionNum() < ssm->MaxConnectionNum())
			{
				max_fd = new_fd;
				int clientId = ssm->clientId_List.size();
				for (int i = 0; i < ssm->clientId_List.size(); i++)
				{
					if (ssm->clientId_List[i] != i)
						clientId = i;
				}
				ssm->clientHandler_List.push_back(ClientHandler(clientId, new_fd));
				ssm->clientId_List.push_back(clientId);
				ssm->clientHandler_List[clientId].SendMessage("Id," + to_string(clientId));
				ssm->SetCurConnectionNum(ssm->CurConnectionNum() + 1);
				for (int i = 0; i < ssm->clientHandler_List.size(); i++)
				{
					ssm->clientHandler_List[i].SendMessage("PlayerNum," + to_string(ssm->CurConnectionNum()));
				}
			}
			else
			{
				string message = "Full";
				if (send(new_fd, message.c_str(), message.length(), 0) == -1) {
					perror("others send");
				}
				close(new_fd);
			}
		}
	}
	ssm->listening = false;
	pthread_exit(NULL); // 離開子執行緒
}

bool SeverSocketManager::SendToClient(int clientId, string message)
{
	if (clientId >= clientHandler_List.size() || !clientHandler_List[clientId].Connected())
		return false;
	bool connectState = clientHandler_List[clientId].SendMessage(message);
	return connectState;
}

void SeverSocketManager::Dispose()
{
	listening = false;
	while (clientHandler_List.size() > 0)
	{
		clientHandler_List.back().Dispose();
		clientHandler_List.back().~ClientHandler();
		clientHandler_List.pop_back();
	}
	curConnectionNum = 0;
	clientId_List.clear();
	close(severSockfd);
	severSockfd = -1;
}
