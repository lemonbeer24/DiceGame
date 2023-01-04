struct playerData{
    int clientfd;//이 클라이언트 의 소켓fd
    int matchfd;//게임 진행시 이 클라와 게임하는 클라 의 fd
    int Dicescore;//주사위 굴려서 나온 합
    int totalscore;//승점.
    int nowstate;
    // 0 = 아무것도 아닌 그냥 서버에만 연결되어 있는 상태
    // 1 = 방을 생성하고(호스트) 다른 클라 와 연결 을 기다리는 상태
    // 2 = 방에 연결되었지만 호스트가 아닌경우
    // 3 = 게임중(호스트(방 만든 사람) 인 경우).
    // 4 = 게임중(호스트 아님).
    int nowready;//게임 시작하기전 : 준비상태 (1 : 준비, 0 : 아님)
    //게임 시작후 : 1 : 내턴 0 : 상대턴 2 : 내턴 진행완료
    int nowturn;//현재 게임에서 경과한 턴을 표현 
    //10턴 이 지나면 게임이 끝남

    char roomname[20];//방을 생성할시 방의 이름.
    char nikname[20];//게임 에서 사용할 닉네임
};