#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include <signal.h>
#include <sys/wait.h>
#include <arpa/inet.h>
#include <sys/socket.h>
#include <sys/select.h>
#include <sys/msg.h>
#include <sys/types.h>
#include <sys/ipc.h>
#include <pthread.h>
#include <stdio_ext.h>
#include <mqueue.h>
#include <fcntl.h>
#include "playerData.h"

int searchPlayerdata(int, struct playerData*);
char * randomDice(void);
void error_handling(char *message);
void read_childproc(int sig);
void init_pDatacliFd(struct playerData data[]);
void *receive_pcmd(void*);

#define BUF_SIZE 48
#define player_Max 20

struct msg {
    long int type;
    char text[150];
};

//파이프라인 자식-부모 구현부
//원인을 알수없는 버그로 인하여 기능제거
//int receive_in = 0;//자식 ps 에서 사용하는 부모에서 명령어가 왔는지 확인하는 제어변수
//receive_pcmd 에서 파이프라인 에 데이터가 들어와 read 함수 의 대기 가 끝나고
//스레드 가 받은 데이터를 리턴. 그때 스레드가 끝난것을 알리기 위해 이 변수 를 1로. 

int main(int argc, char *argv[])
{
    struct sigaction act = {};
    pid_t pid;

	int ppipe[2], cpipe[2];

	//파이프라인 자식-부모 구현부
	//원인을 알수없는 버그로 인하여 기능제거
	//pipe(ppipe);//부모->자식
	//pipe(cpipe);//자식->부모

    printf("process signal setting...\n");

    act.sa_handler=read_childproc;
	sigemptyset(&act.sa_mask);
	act.sa_flags=0;
	if(sigaction(SIGCHLD, &act, 0) == -1){
        printf("signal set fail!\n");
        exit(1);
    }

    printf("process signal setting complete!\n");

	//파이프라인 자식-부모 구현부
	//원인을 알수없는 버그로 인하여 기능제거
	/*
    pid=fork();

    if(pid == -1){
        printf("prosess fork errno!\n");
        exit(1);
    }*/

    //if(pid == 0){//자식
        int serv_sock, clnt_sock;
	    struct sockaddr_in serv_adr, clnt_adr;
        socklen_t adr_sz;
	    int str_len;
	    char buf[BUF_SIZE] = {0};//클라 가 보낸 메시지 받아올때, 데이터를 보낼때 쓰는 공용버퍼
		char * funbuf;//함수에서 반환하는 char 포인터(동적할당)를 받는 포인터

		close(ppipe[1]);//사용하지 않는 파이프라인 처리
		close(cpipe[0]);

        serv_sock=socket(PF_INET, SOCK_STREAM, IPPROTO_TCP);
	    memset(&serv_adr, 0, sizeof(serv_adr));
	    serv_adr.sin_family=PF_INET;
	    serv_adr.sin_addr.s_addr=htonl(INADDR_ANY);
	    serv_adr.sin_port=htons(9290);
	
	    if(bind(serv_sock, (struct sockaddr*) &serv_adr, sizeof(serv_adr))==-1)
		    error_handling("bind() error");
	    if(listen(serv_sock, 5)==-1)
		    error_handling("listen() error");

		int fd_max, fd_num, i;
        struct timeval timeout;
        fd_set reads, cpy_reads;

        FD_ZERO(&reads);
        FD_SET(serv_sock, &reads);
        fd_max = serv_sock;

        struct playerData Parr[player_Max];//연결되는 클라이언트 들의 정보를 담을 객체
        init_pDatacliFd(Parr);
		char varroomname[20] = {0}, varnikname[20] = {0};
		int detaching;//&로 데이터가 분리되는 패킷에 데이터를 분리할 때 사용.
		int targetParr;//Parr 배열에 fd 조회로 알아낸 해당 Parr 구조체에 배열번호를 저장
		int mfd;//targetParr 의 matchfd 
		
		//파이프라인 자식-부모 구현부
		//원인을 알수없는 버그로 인하여 기능제거
		//pthread_t Ptreceive_cmd;//부모 ps(셸) 에 파이프라인 을 통한 명령어를 받을 스레드pid
		//void *receive_data;//스레드 에서 리턴값을 받을 void 포인터
		//pthread_create(&Ptreceive_cmd,NULL,receive_pcmd,(void*)&cpipe[1]);

		srand((unsigned int)time(NULL));//랜덤함수 시드 초기화

		int randnum;
		int randresult;
		int diceScore = 0;

        while(1)
        {
		//파이프라인 자식-부모 구현부
		//원인을 알수없는 버그로 인하여 기능제거
		//일단 부모에서 요청이 왔는지 확인하여야함.
		/*
		if(receive_in == 1){
			pthread_join(Ptreceive_cmd,&receive_data);
			char * cmd = (char*) receive_data;

			switch(cmd[0]){
				case 'p':
					//출력
					if(cmd[1] == 'u'){
						//현재 연결된 유져들의 상태를 출력
						for(int j = 0; j < player_Max; j++){
							if(Parr[j].clientfd != -1){
								printf("fd %d 번 state : %d\n", Parr[j].clientfd , Parr[j].nowstate);
							}
						}
					} else if(cmd[1] == 'r'){
						//현재 생성된 방 정보를 출력
						for(int j = 0; j < player_Max; j++){
							if(Parr[j].clientfd != -1){
								if(Parr[j].nowstate == 1 || Parr[j].nowstate == 3){
									printf("host : %d, ", Parr[j].clientfd);
									printf("roomName : %s, nickname : %s, ", Parr[j].roomname, Parr[j].nikname);
									printf("now ready : %d\n", Parr[j].nowready);
									if(Parr[j].matchfd == -1){
										printf("match client : null\n");
									} else {
										int mfd = searchPlayerdata(Parr[j].matchfd,Parr);
										printf("match cilent fd : %d, ", Parr[mfd].matchfd);
										printf("nickname : %s, ", Parr[mfd].nikname);
										printf("now ready : %d\n", Parr[mfd].nowready);
									}
								}
							}
						}
					}
					else if(cmd[1] == 'x'){
						for(int i = 0; i < player_Max; i++){
							printf("%d\n",Parr[i].clientfd);
						}
					}
					else {
						printf("????2\n");
					}
					break;
				default:
					printf("????1\n");
			}

			free(cmd);
			receive_in = 0;

			pthread_create(&Ptreceive_cmd,NULL,receive_pcmd,(void*)&cpipe[1]);
		}*/
		
        cpy_reads=reads;
		timeout.tv_sec=2;
		timeout.tv_usec=2000;

		if((fd_num=select(fd_max+1, &cpy_reads, 0, 0, &timeout))==-1)
			break;
		
		if(fd_num==0)
			continue;

		for(i=0; i<fd_max+1; i++)
		{
			if(FD_ISSET(i, &cpy_reads))
			{
				if(i==serv_sock)     // connection request!
				{
					adr_sz=sizeof(clnt_adr);
					clnt_sock=
						accept(serv_sock, (struct sockaddr*)&clnt_adr, &adr_sz);
					FD_SET(clnt_sock, &reads);//새로운 클라fd 를 fd_set 에 추가 

                    for(int j = 0; j <= 20; j++){
                        
                        if(j >= 20){//Parr 배열에 빈자리가 없을 경우
                            printf("\nParr is full!\n");
                            break;
                        }

                        if(Parr[j].clientfd == -1){
                            Parr[j].clientfd = clnt_sock;
							Parr[j].nowstate = 0;
                            break;
                        }
                    }

					if(fd_max<clnt_sock)
						fd_max=clnt_sock;
				}
				else    // read message!
				{
					memset(buf,0,BUF_SIZE);	
					str_len=read(i, buf, BUF_SIZE);
					if(str_len==0)    // close request!
					{
						targetParr = searchPlayerdata(i,Parr);
						Parr[targetParr].clientfd = -1;//해당 배열의 fd 를 -1로
						FD_CLR(i, &reads);//fd_set 에서 제외(0으로 바꿈)
						close(i);
					}
					else
					{
						//클라이언트 요청 
						//d : 주사위 랜덤값 요청(주사위 굴리기)
						switch(buf[0]) {
							case 'd' :
								targetParr = searchPlayerdata(i,Parr);
								mfd = searchPlayerdata(Parr[targetParr].matchfd,Parr);
								memset(buf,0,BUF_SIZE);//버퍼 비우기
								diceScore = 0;

								buf[0] = 'd';

								for(int i = 1; i < 6; i++){
									randnum = rand();
									randresult = ((int)randnum % 6) + 1;
									sprintf(&(buf[i]), "%d", randresult);
									diceScore += randresult;
								}

								char diceScstr[3];//주사위 총합을 더한 점수를 문자열로
								//저장할 문자열 변수
								sprintf(diceScstr,"%d", diceScore);
								Parr[targetParr].Dicescore = diceScore;

								buf[6] = diceScstr[0];//점수 넣는곳
								buf[7] = diceScstr[1];

								buf[8] = '\0';

								Parr[targetParr].nowready = 2;

								if(Parr[targetParr].nowready == 2 && Parr[mfd].nowready == 2){
									//두 클라이언트 모두 턴을 진행함 = 라운드 종류
									if(Parr[targetParr].Dicescore > Parr[mfd].Dicescore){
										//쿼리를 보낸 클라의 주사위의 합이 더 높음
										Parr[targetParr].totalscore += 1;
									} else if (Parr[targetParr].Dicescore > Parr[mfd].Dicescore) {
										Parr[mfd].totalscore += 1;
									} 

									if(Parr[targetParr].nowstate == 3){
										//쿼리를 요청한 클라가 호스트인 경우
										Parr[targetParr].nowready = 1;
										Parr[mfd].nowready = 0;
									} else {
										//아님
										Parr[targetParr].nowready = 0;
										Parr[mfd].nowready = 1;
									}

									Parr[targetParr].nowturn += 1;
									Parr[mfd].nowturn += 1;

									if(Parr[targetParr].nowturn >= 10){//10 라운드 가 지날경우
										//게임 끝남
										buf[0] = 'e';//패킷에 표시
									}
								}
								
								write(i, buf, BUF_SIZE);//요청한 fd 에게 전송
								write(Parr[targetParr].matchfd, buf, BUF_SIZE);
								//상대 fd 에게도 전달하여 상대방 주사위 컨트롤을 구현하게 함.
								break;

							//r : 방 에 관련된 요청들
							case 'r' :
								//c : 방 만들기
								if(buf[1] == 'c'){
									//패킷 형태 : rc방이름&(데이터 구분)닉네임&
									targetParr = searchPlayerdata(i,Parr);

									//쓰레기값 방지
									memset(Parr[targetParr].roomname,0,sizeof(Parr[targetParr].roomname));
									memset(Parr[targetParr].nikname,0,sizeof(Parr[targetParr].nikname));

									detaching = 2;
									for(int j = 0; j < 20; j++){
										if(buf[detaching] == '&'){//패킷 의 방이름 영역이 끝났다는것을 알림
											detaching++;
											break;
										} else {
											Parr[targetParr].roomname[j] = buf[detaching];//방이름 하나씩 입력
											detaching++;
										}
									}

									for(int j = 0; j < 20; j++){
										if(buf[detaching] == '&'){//닉네임 영역 문자열 추출
											detaching++;
											break;
										} else {
											Parr[targetParr].nikname[j] = buf[detaching];//방이름 하나씩 입력
											detaching++;
										}
									}
									
									Parr[targetParr].nowstate = 1;
									Parr[targetParr].matchfd = -1;//현재 연결된 클라가 없음을 표현
									Parr[targetParr].nowready = 0;//아직 준비 x

									memset(buf,0,BUF_SIZE);	
									sprintf(buf,"%d", 1);
									write(i, buf, BUF_SIZE);

								} else if(buf[1] == 'f'){
								//f : 현제 생성된 방(게임중이 아닌) 목록 들을 반환
								//방 구분 은 만든 클라의 fd 로 하는걸로...
								//nowstate 가 1 인 fd 들의 정보들을 아래에 문자열로 표현
								//보내는 패킷 문자열 : 방이름|fd번호&반복...&
								detaching = 0;
								char fdstr[3];
								memset(buf,0,BUF_SIZE);
								for(int j = 0; j < player_Max; j++){
									if(Parr[j].clientfd != -1){
										if(Parr[j].nowstate == 1){
											strcat(buf,Parr[j].roomname);
											buf[strlen(buf)] = '|';
											buf[strlen(buf)+1] = '\0';
											sprintf(fdstr,"%d", Parr[j].clientfd);
											strcat(buf,fdstr);
											buf[strlen(buf)] = '&';
											buf[strlen(buf)+1] = '\0';
										}
									}
								}

								buf[strlen(buf)] = '\0';
								write(i,buf,BUF_SIZE);

								} else if(buf[1] == 'j'){
								//j : 생성된 방에 참가.
								//받는 패킷 : rj'fd번호'(시간부족으로 한자리수로 고정)닉네임
								//방에 참가하는 동시에 참가하는 fd 의 Parr 배열에 필요한 데이터를 넣어주어야함
									int hostfd;//방 호스트 fd
									hostfd = atoi(&buf[2]);
									targetParr = searchPlayerdata(hostfd,Parr);
									//호스트의 fd 로 Parr배열번호 를 알아낸다
									if(Parr[targetParr].nowstate == 1){
										Parr[targetParr].matchfd = i;//호스트에 matchfd 에
										//자신의(방에 들어가는) fd를 넣는다
										targetParr = searchPlayerdata(i,Parr);
										Parr[targetParr].matchfd = hostfd;
										//마찬가지 로 자신의 matchfd 에도 상대에 fd 를 넣는다.

										memset(Parr[targetParr].nikname,0,sizeof(Parr[targetParr].nikname));
										//호스트 가 아닌 클라는 닉네임만 저장한다.
										for(int j = 3; j < strlen(buf); j++){//닉네임 추출
											Parr[targetParr].nikname[j - 3] = buf[j];
										}
										Parr[targetParr].nikname[strlen(Parr[targetParr].nikname)] = '\0';
										Parr[targetParr].nowstate = 2;
										Parr[targetParr].nowready = 0;

										sprintf(buf,"%d",2);
										write(i,buf,BUF_SIZE);
										//정상 처리됨을 클라(방에 들어갈려는)에게 알린다.

										buf[1] = '\0';
									    strcat(buf,Parr[targetParr].nikname);
									    //방에 들어가려는 클라의 닉네임을 패킷에 추가
									    buf[strlen(buf)] = '\0';
									    write(hostfd,buf,BUF_SIZE);
									    //상대 클라(호스트)에게도 알려줘야함...

									} else {//찾으려는 fd의 방이 없거나 이미 게임중인 경우(1이아닌)
										sprintf(buf,"%d",-1);
										write(i,buf,BUF_SIZE);
									}
								}
								break;

							case 'l':
								// l : 방에 참가한 클라가 준비 버튼을 누름
								targetParr = searchPlayerdata(i,Parr);
								Parr[targetParr].nowready = 1;
								int mfd = searchPlayerdata(Parr[targetParr].matchfd,Parr);
								if(Parr[targetParr].nowready == 1 &&  Parr[mfd].nowready == 1){
									//game start!
									//현재 쿼리를 날린 fd 가 호스트 인지 확인
									//선공은 호스트가 하는걸로 한다.
									if(Parr[targetParr].nowstate == 1){//호스트인 경우
										Parr[targetParr].nowstate = 3;
										Parr[targetParr].nowready = 1;//선공 표시
										Parr[targetParr].nowturn = 1;//턴 표시
										Parr[targetParr].Dicescore = 0;
										Parr[targetParr].totalscore = 0;
										Parr[mfd].nowstate = 4;
										Parr[mfd].nowready = 0;
										Parr[mfd].nowturn = 1;//턴 표시
										Parr[mfd].Dicescore = 0;
										Parr[mfd].totalscore = 0;
										sprintf(buf,"%d",1);//게임이 준비되었다는 신호를 클라에게 보냄
										write(i,buf,BUF_SIZE);
										sprintf(buf, "%d",1);
										write(Parr[targetParr].matchfd,buf,BUF_SIZE);
									} else {//쿼리를 날린 fd 가 호스트가 아님.
										Parr[targetParr].nowstate = 4;
										Parr[targetParr].nowready = 0;
										Parr[targetParr].nowturn = 1;//턴 표시
										Parr[targetParr].Dicescore = 0;
										Parr[targetParr].totalscore = 0;
										Parr[mfd].nowstate = 3;
										Parr[mfd].nowready = 1;//선공 표시
										Parr[mfd].nowturn = 1;//턴 표시
										Parr[mfd].Dicescore = 0;
										Parr[mfd].totalscore = 0;
										sprintf(buf,"%d",1);
										write(i,buf,BUF_SIZE);
										sprintf(buf, "%d",1);
										write(Parr[targetParr].matchfd,buf,BUF_SIZE);
									}

								} else {
									sprintf(buf,"%d",2);//모든 클라가 준비되었진 않았지만 그래도 요청은 처리
									//되었다는 것을 클라에게 알림
									write(i,buf,BUF_SIZE);
								}
								break;

							case 'c':
							//c : 준비취소
								targetParr = searchPlayerdata(i,Parr);
								Parr[targetParr].nowready = 0;
								sprintf(buf,"%d",3);//준비가 취소 되었다는것을 클라에게 알림 
								write(i,buf,BUF_SIZE);
								break;

							default : 
								printf("????\n");
						}

					}
				}

			}
		}

        }

	//파이프라인 자식-부모 구현부
	//원인을 알수없는 버그로 인하여 기능제거
	/*
    } else {//부모
	
	close(ppipe[0]);
	close(cpipe[1]);

    char inputstream[50];
	printf("input cmd :");

        while(1){
			
            fgets(inputstream, sizeof(inputstream), stdin);
			printf("\nwrite!\n");

			if(write(ppipe[1],inputstream,50) == -1){
				printf("ppipe error!\n");
			} else {
				if(read(cpipe[0], inputstream, 50) == -1){
					printf("cpipe error!\n");
				}

            	printf("input cmd :");
			}
			__fpurge(stdin);
			
        }
    }
	*/
	

    return 0;
}

int searchPlayerdata(int fd, struct playerData *parr){
	for(int i = 0; i < player_Max; i++){
		if(parr[i].clientfd == fd){
			return i;
		}
	}
	return -1;
}

//파이프라인 자식-부모 구현부
//원인을 알수없는 버그로 인하여 기능제거
/*
void * receive_pcmd(void *arg){

	int piperead = *((int*) arg);

	char *buf = malloc(sizeof(char[50]));
	buf[0] = '\0';

	read(piperead, buf, sizeof(buf) - 1);
	printf("cmd in : %s\n", buf);

	receive_in = 1;
	return buf;
}
*/

void init_pDatacliFd(struct playerData *data){
    for(int i = 0; i < 20; i++){
        data[i].clientfd = -1;
    }
}

void read_childproc(int sig)
{
	pid_t pid;
	int status;
	pid=waitpid(-1, &status, WNOHANG);
	printf("removed proc id: %d \n", pid);
}

void error_handling(char *message)
{
	fputs(message, stderr);
	fputc('\n', stderr);
	exit(1);
}