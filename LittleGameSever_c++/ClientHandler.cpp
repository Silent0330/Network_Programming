#include "ClientHandler.h"


/*static void* RecvMessage(void* arg)
{
	ClientHandler* handler = (ClientHandler*)arg;
	int datasize = 2048, numbytes;
	char buf[datasize];
	string message;
	while (handler->connected)
	{
		try
		{
			cout << handler->sockfd << "recving\n";
			numbytes = recv(handler->sockfd, buf, datasize - 1, 0);
			cout << "recv ";
			if (numbytes == -1 || numbytes == 0) {
				perror("recv");
				handler->connected = false;
				break;
			}
			message = string(buf);
			cout << message;
		}
		catch (exception e)
		{
			cout << e.what();
			handler->connected = false;
			break;
		}
		vector<string> messages = split(message, ";");
		for (int i = 0; i < messages.size(); i++)
		{
			vector<string> messageArgs = split(messages[i], ",");
			if (messageArgs[0] == ("Up"))
			{
				if (messageArgs[1] == "true")
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
				if (messageArgs[1] == "true")
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
				if (messageArgs[1] == "true")
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
				if (messageArgs[1] == "true")
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
				if (messageArgs[1] == "true")
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
				if (messageArgs[1] == "true")
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
				if (messageArgs[1] == "true")
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
	
	handler->connected = false;
	pthread_exit(NULL); // Â÷¶}¤l°õ¦æºü
}
*/

bool ClientHandler::Connected() { return connected; }
bool ClientHandler::DisConnectChecked() { return disConnectChecked; }

bool ClientHandler::ReadyToStart() { return readyToStart; }
bool ClientHandler::Up() { return up; }
bool ClientHandler::Down() { return down; }
bool ClientHandler::Left() { return left; }
bool ClientHandler::Right() { return right; }
bool ClientHandler::Attack() { return attack; }
bool ClientHandler::Reload() { return reload; }

ClientHandler::ClientHandler() {}

ClientHandler::ClientHandler(int clientId, int clientSocket)
{
	id = clientId;
	sockfd = clientSocket;
	up = down = left = right = false;


	connected = true;
	disConnectChecked = false;
	readyToStart = false;
	startGameRequest = false;

}

void ClientHandler::ChangeId(int new_id)
{
	id = new_id;
	SendMessage("Id," + to_string(id));
}

bool ClientHandler::SendMessage(string message)
{
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
		}
		catch (exception e)
		{
			cout << e.what();
			connected = false;
			return false;
		}
		return true;
	}
	return false;
}

void ClientHandler::CheckConnection()
{
	SendMessage("0");
}

void ClientHandler::Dispose()
{
	connected = false;
	disConnectChecked = true;
	close(sockfd);
}
