using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFA_Yacht_Dice
{
    public class game
    {
        //game 에 관한 데이터를 저장하는 데이터 객체
        public int nowstate;
        // 0 = 아무것도 아닌 그냥 서버에만 연결되어 있는 상태
        // 1 = 방을 생성하고 다른 클라 와 연결 을 기다리는 상태
        // 2 = 방에 연결되었지만 호스트가 아닌경우
        // 3 = 게임중(호스트(방 만든 사람) 인 경우).
        // 4 = 게임중(호스트 아님).
        private string roomName;
        private string nikName;
        private string enemynikname;

        public int diceTotal;
        public int enemyDiceTotal;
        public int total_score;
        public int enemy_total_score;

        public int turn;// 게임을 시작하기 전 준비상태를 표현(1 : ready, 0 : wait)
        //게임이 시작된 후 에는 자신의 차레 를 표현 1 = 내턴, 0 = 상대턴, 2 = 내턴 진행완료.

        public int nowturn;// 게임에서 진행된 턴의 수를 표현

        public string RoomName {
            get { return roomName; }
            set { roomName = value; }
        }

        public string Nikname{
            get { return nikName; }
            set { nikName = value; }
        }

        public string enemyNikname
        {
            get { return enemynikname; }
            set { enemynikname = value; }
        }

        public game(string roomname, string nikname) {//방 만들기
            this.roomName = roomname;
            this.nikName = nikname;

            diceTotal = 0;
            enemyDiceTotal = 0;
            total_score = 0;
            enemy_total_score = 0;
            nowturn = 1;
    }

        public game(string nikname) {//방 에 들어가기
            this.nikName = nikname;

            diceTotal = 0;
            enemyDiceTotal = 0;
            total_score = 0;
            enemy_total_score = 0;
            nowturn = 1;
        }

    }
}
