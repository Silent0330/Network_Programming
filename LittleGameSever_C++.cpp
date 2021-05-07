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
#include <vector>
#include <pthread.h>
#include <mutex>

#define PORT "6666"
#define BACKLOG 10

using namespace std;

int sockfd;
struct addrinfo hints, *servinfo, *p;
struct sockaddr_storage their_addr;
socklen_t sin_size;
struct sigaction sa;
int yes=1;
char s[INET_ADDRSTRLEN];
vector<string> sever_ips;
vector<int> sever_fds;
vector<pthread_t> sever_threads;
int rv;

mutex mu;

pthread_t waitForConnect_thread; // 宣告 pthread 變數
bool waitingForConnect;
	
vector<string> split(const string& str, const string& delim) {
	vector<string> res;
	if("" == str) return res;
	//先将要切割的字符串从string类型转换为char*类型
	char * strs = new char[str.length() + 1] ; //不要忘了
	strcpy(strs, str.c_str()); 
 
	char * d = new char[delim.length() + 1];
	strcpy(d, delim.c_str());
 
	char *p = strtok(strs, d);
	while(p) {
		string s = p; //分割得到的字符串转换为string类型
		res.push_back(s); //存入结果数组
		p = strtok(NULL, d);
	}
 
	return res;
}

void sigchld_handler(int s){
	while(waitpid(-1, NULL, WNOHANG) > 0);
}

void *get_in_addr(struct sockaddr *sa){
	if(sa -> sa_family == AF_INET){
		return &(((struct sockaddr_in*)sa) -> sin_addr);
	}
	return &(((struct sockaddr_in6*)sa) -> sin6_addr);
}

void* handleSever(void* arg) {
	int sever_fd = *(int*)arg;
	int datasize = 2048, numbytes;
	char buf[datasize];
	bool connected = true;
	while(connected) {
		try {
			numbytes = recv(sever_fd, buf, datasize-1, 0);
			if (numbytes == -1 || numbytes == 0) {
				perror("recv");
				connected = false;
				break;
			}
			string receivemessage(buf);
			cout << receivemessage << "\n";
			vector<string> receivemessages = split(receivemessage, ";");
		}
		catch(exception &e) {
			connected = false;
			break;
		}
	}
	cout << sever_fd << " exit\n";
	close(sever_fd);
	//lock sever_fds and sever_ips
	unique_lock<mutex> locker(mu);
	int ind = -1;
	for(int i = 0; i < sever_fds.size(); i++) {
		if(sever_fds[i] == sever_fd) {
			ind = i;
			break;
		}
	}
	if(ind != -1) {
		sever_fds.erase(sever_fds.begin() + ind);
		sever_ips.erase(sever_ips.begin() + ind);
	}
	//unlock sever_fds and sever_ips
	locker.unlock();
	pthread_exit(NULL); // 離開子執行緒
}

bool checkConnection(string ip) {
	string message = " ";
	//lock sever_fds and sever_ips
	unique_lock<mutex> locker(mu);
	for(int i = 0; i < sever_fds.size(); i++) {
		if(send(sever_fds[i], message.c_str(), message.length(), 0) == -1){
			perror("sever disconnect");
			close(sever_fds[i]);
			sever_fds.erase(sever_fds.begin() + i);
			sever_ips.erase(sever_ips.begin() + i);
			i--;
		}
		if(sever_ips[i] == ip)
			return false;
	}
	//unlock sever_fds and sever_ips
	locker.unlock();
	return true;
}

void* waitForConnect(void* arg) {
	int datasize = 2048, numbytes;
	char buf[datasize];
	waitingForConnect = true;
	while(waitingForConnect){
		try{
			sin_size = sizeof their_addr;
			int new_fd = accept(sockfd, (struct sockaddr *)&their_addr, &sin_size);
			if(new_fd == -1){
				perror("accept");
				continue;
			}
			inet_ntop(their_addr.ss_family, get_in_addr((struct sockaddr *)&their_addr), s, sizeof s);
			printf("server: got connection form %s\n", s);
			
			if ((numbytes = recv(new_fd, buf, datasize-1, 0)) == -1) {
				perror("recv");
			}
			string receivemessage(buf);
			cout << receivemessage << "\n";
			vector<string> receivemessages = split(receivemessage, ";");
			if(receivemessages[0] == "BeSever") {
				if(!checkConnection(string(s))) {
					string message = "Fail";
					if(send(new_fd, message.c_str(), message.length(), 0) == -1){
						perror("others send");
					}
					close(new_fd);
					continue;
				}
				string message	= "Success;";
				if(sever_ips.size() >= 10){
					message	= "Fail;";
					if(send(new_fd, message.c_str(), message.length(), 0) == -1){
						perror("sever send");
					}
					close(new_fd);
					continue;
				}
				if(send(new_fd, message.c_str(), message.length(), 0) == -1){
					perror("sever send");
					close(new_fd);
				}
				else {
					//lock
					unique_lock<mutex> locker(mu);
					sever_fds.push_back(new_fd);
					sever_ips.push_back(string(s));
					//unlock
					locker.unlock();
					pthread_t *handleSever_thread = new pthread_t;
					pthread_create(handleSever_thread, NULL, handleSever, &new_fd); // 建立子執行緒
					cout << "sever " << new_fd << " \n";
				}
			}
			else if(receivemessages[0] == "BeClient"){
				string message = "Success";
				//lock
				unique_lock<mutex> locker(mu);
				for(int i = 0; i < sever_ips.size(); i++) {
					message += "," + sever_ips[i];
				}
				//unlock
				locker.unlock();
				message += ";";
				if(send(new_fd, message.c_str(), message.length(), 0) == -1){
					perror("client send");
					close(new_fd);
				}
				else {
					cout << "client " << new_fd << " \n";
				}
			}
			else {
				string message = "Fail";
				if(send(new_fd, message.c_str(), message.length(), 0) == -1){
					perror("others send");
				}
				close(new_fd);
			}
		}
		catch(exception &e){
			break;
		}
	}
	waitingForConnect = false;
	pthread_exit(NULL); // 離開子執行緒
}

int main(void){
	waitingForConnect = false;
	sever_ips = vector<string>();
	sever_fds = vector<int>();
	sever_threads = vector<pthread_t>();
	
	
	
	memset(&hints, 0, sizeof hints);
	hints.ai_family =  AF_INET;
	hints.ai_socktype = SOCK_STREAM;
	hints.ai_flags = AI_PASSIVE;

	if((rv = getaddrinfo(NULL, PORT, &hints, &servinfo)) != 0){
		fprintf(stderr, "getaddrinfo: %s\n", gai_strerror(rv));
		return 1;
	}

	for(p = servinfo; p != NULL; p = p -> ai_next){
		if((sockfd = socket(p -> ai_family, p -> ai_socktype, p -> ai_protocol)) == -1){
			perror("server: socket");
			continue;
		}
		if(setsockopt(sockfd, SOL_SOCKET, SO_REUSEADDR, &yes, sizeof(int)) == -1){
			perror("setsockopt");
			exit(1);
		}
		if(bind(sockfd, p -> ai_addr, p -> ai_addrlen) == -1){
			close(sockfd);
			perror("server: bind");
			continue;
		}
		
		break;
	}
	if(p == NULL){
		fprintf(stderr, "server: failed to bind\n");
		return 2;
	}
	
	freeaddrinfo(servinfo);

	if(listen(sockfd, BACKLOG) == -1){
		perror("listen");
		exit(1);
	}

	sa.sa_handler = sigchld_handler;
	sigemptyset(&sa.sa_mask);
	sa.sa_flags = SA_RESTART;

	if(sigaction(SIGCHLD, &sa, NULL) == -1){
		perror("sigaction");
		exit(1);
	}

	printf("server: waiting for connections...\n");
	
	
	pthread_create(&waitForConnect_thread, NULL, waitForConnect, NULL); // 建立子執行緒
	while(1){
		int key;
		std::cin >> key;
		if(key == 0) {
			break;
		}
		else if(key == 1) {
			for(int i = 0; i < sever_ips.size(); i++){
				cout << sever_fds[i] << " ip = " << sever_ips[i] << "\n";
			}
		}
	}
	waitingForConnect = false;
	for(int i = 0; i < sever_fds.size(); i++) {
		close(sever_fds[i]);
	}
	close(sockfd);
	
	return 0;
}
