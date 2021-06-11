#include "ClientHandler.h"

#include <fcntl.h>
#include "Utils.h"

bool ClientHandler::Connected() { return connected; }
bool ClientHandler::DisConnectChecked() { return disConnectChecked; }

int ClientHandler::Id() { return id; }
bool ClientHandler::ReadyToStart() { return readyToStart; }
bool ClientHandler::Up() { return up; }
bool ClientHandler::Down() { return down; }
bool ClientHandler::Left() { return left; }
bool ClientHandler::Right() { return right; }
bool ClientHandler::Attack() { return attack; }
bool ClientHandler::Reload() { return reload; }
bool ClientHandler::StartGameRequest() { return startGameRequest; }

void ClientHandler::SetId(int value) {
	id = value;
	SendMessage("Id," + to_string(id));
}
void ClientHandler::SetReadyToStart(bool value) { readyToStart = value; }
void ClientHandler::SetUp(bool value) { up = value; }
void ClientHandler::SetDown(bool value) { down = value; }
void ClientHandler::SetLeft(bool value) { left = value; }
void ClientHandler::SetRight(bool value) { right = value; }
void ClientHandler::SetAttack(bool value) { attack = value; }
void ClientHandler::SetReload(bool value) { reload = value; }
void ClientHandler::SetStartGameRequest(bool value) {
	//unique_lock<mutex> locker(startGameRequest_mu);
	sem_wait(&startGameRequest_mu);
	startGameRequest = value;
	sem_post(&startGameRequest_mu);
	//locker.unlock();
}

ClientHandler::ClientHandler() {}

ClientHandler::ClientHandler(int clientId, int clientSocket)
{
	id = clientId;
	sockfd = clientSocket;
	up = down = left = right = attack = reload = false;

	sem_init(&startGameRequest_mu, 0, 1);
	sem_init(&send_mu, 0, 1);

	connected = true;
	disConnectChecked = false;
	readyToStart = false;
	startGameRequest = false;

}


void ClientHandler::RecvMessage(string message)
{
	if (connected)
	{
		vector<string> messages = Utils::split(message, ";");
		for (int i = 0; i < messages.size(); i++)
		{
			vector<string> messageArgs = Utils::split(messages[i], ",");
			if (messageArgs[0] == ("Up"))
			{
				if (messageArgs[1] == "True")
				{
					SetUp(true);
				}
				else
				{
					SetUp(false);
				}
			}
			else if (messageArgs[0] == ("Down"))
			{
				if (messageArgs[1] == "True")
				{
					SetDown(true);
				}
				else
				{
					SetDown(false);
				}
			}
			else if (messageArgs[0] == ("Left"))
			{
				if (messageArgs[1] == "True")
				{
					SetLeft(true);
				}
				else
				{
					SetLeft(false);
				}
			}
			else if (messageArgs[0] == ("Right"))
			{
				if (messageArgs[1] == "True")
				{
					SetRight(true);
				}
				else
				{
					SetRight(false);
				}
			}
			else if (messageArgs[0] == ("Attack"))
			{
				if (messageArgs[1] == "True")
				{
					SetAttack(true);
				}
				else
				{
					SetAttack(false);
				}
			}
			else if (messageArgs[0] == ("Reload"))
			{
				if (messageArgs[1] == "True")
				{
					SetReload(true);
				}
				else
				{
					SetReload(false);
				}
			}
			else if (messageArgs[0] == ("StartGame"))
			{
				SetStartGameRequest(true);
			}
			else if (messageArgs[0] == ("Ready"))
			{
				if (messageArgs[1] == "True")
				{
					SetReadyToStart(true);
				}
				else
				{
					SetReadyToStart(false);
				}
			}
		}
	}
}

bool ClientHandler::SendMessage(string message)
{
	sem_wait(&send_mu);
	bool result = false;
	cout << "send client = " << id << " sock = " << sockfd << " message = " <<  message << "\n";
	if (connected)
	{
		if (message.back() != ';')
			message.push_back(';');
		try
		{
			if(send(sockfd, message.c_str(), message.length(), 0) == -1){
				perror("others send");
				connected = false;
				close(sockfd);
			}
			result = true;
		}
		catch (exception e)
		{
			cout << e.what();
			connected = false;
			result = false;
		}
	}
	sem_post(&send_mu);
	return result;
}

void ClientHandler::CheckConnection()
{
	SendMessage("0");
}

void ClientHandler::Dispose()
{
	connected = false;
	disConnectChecked = true;
	sem_destroy(&startGameRequest_mu);
	sem_destroy(&send_mu);
	close(sockfd);
}
