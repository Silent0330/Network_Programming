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

#define PORT "6666"
#define BACKLOG 10

int sockfd, sever_fd;
struct addrinfo hints, *servinfo, *p;
struct sockaddr_storage their_addr;
socklen_t sin_size;
struct sigaction sa;
int yes=1;
char s[INET_ADDRSTRLEN];
std::string sever_ip;
int rv;

pthread_t waitForConnect_thread; // 宣告 pthread 變數
bool waitingForConnect;
	
void sigchld_handler(int s){
	while(waitpid(-1, NULL, WNOHANG) > 0);
}

void *get_in_addr(struct sockaddr *sa){
	if(sa -> sa_family == AF_INET){
		return &(((struct sockaddr_in*)sa) -> sin_addr);
	}
	return &(((struct sockaddr_in6*)sa) -> sin6_addr);
}

void* waitForConnect(void* arg) {
	int datasize = 2048, numbytes;
	char buf[datasize];
	waitingForConnect = true;
	while(1){
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
				waitingForConnect = false;
				pthread_exit(NULL);
			}
			std:: string receivemessage(buf);
			printf("%s\n", buf);
			std::cout << receivemessage << "\n";
			if(receivemessage == "BeSever") {
				std::string message = "Success;";
				if(send(new_fd, message.c_str(), message.length(), 0) == -1){
					perror("sever send");
					close(new_fd);
					std::cout << "sever send" << "\n";
				}
				else {
					sever_fd = new_fd;
					sever_ip = std::string(s);
					std::cout << "sever" << "\n";
				}
				close(new_fd);
			}
			else if(receivemessage == "BeClient"){
				std::string message = "Success," + sever_ip + ";";
				if(send(new_fd, message.c_str(), message.length(), 0) == -1){
					perror("client send");
					close(new_fd);
				}
				else {
					std::cout << "client" << "\n";
				}
			}
			else {
				std::string message = "Fail";
				if(send(new_fd, message.c_str(), message.length(), 0) == -1){
					perror("others send");
				}
				close(new_fd);
			}
		}
		catch(std::exception &e){
			break;
		}
	}
	waitingForConnect = false;
	pthread_exit(NULL); // 離開子執行緒
}

int main(void){
	waitingForConnect = false;
	
	
	
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
	}
	close(sockfd);
	close(sever_fd);
	
	return 0;
}
