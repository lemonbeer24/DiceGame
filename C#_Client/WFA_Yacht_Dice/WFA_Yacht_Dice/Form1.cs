using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFA_Yacht_Dice
{
    public partial class Form1 : Form
    {
        //private const int 

        private Dice[] dicepool, enemy_diecpool;
        //form 에 있는 주사위 스프라이트와 그 동작을 표현하는 객체 의 배열
        private int timer_t = 0;
        //주사위 굴릴때 사용하는 타이머 시간 기록 변수
        private client_send_recive client;
        //실제로 서버와 통신시 사용되는 객체
        //서버와 통신하는 소켓 등을 여기에 저장, 서버에 요청이 있을시 이 객체를 통하여 요청
        private string serverReturn;
        //서버가 보낸 값을(정확힌 send_recive 의 결과값) 받은 변수.

        private Form_connect _Connect;
        //서버에 접속할시 사용하는 대화상자 객체(form)
        private Form_roomcreate _Roomcreate;
        //방을 만들시 사용하는 대화상자 객체
        private Form_roomfind _Roomfind;
        //방을 찾아서 접속할시 사용되는 대화상자 객체.

        private game Game;
        //현재 진행되는 게임 의 정보를 저장하는 객체 

        public Random random = new Random();//난수

    public Form1()
        {
            InitializeComponent();
            dicepool = new Dice[] { new Dice(picBox_dice1, random),
                                    new Dice(picBox_dice2, random),
                                    new Dice(picBox_dice3, random),
                                    new Dice(picBox_dice4, random),
                                    new Dice(picBox_dice5, random) };

            enemy_diecpool = new Dice[] { new Dice(picBox_enemyDice1, random),
                                          new Dice(picBox_enemyDice2, random),
                                          new Dice(picBox_enemyDice3, random),
                                          new Dice(picBox_enemyDice4, random),
                                          new Dice(picBox_enemyDice5, random) };


            //client = new client_send_recive();
            serverReturn = null;

            ToolStripMenuItem_createroom.Enabled = false;
            ToolStripMenuItem_findroom.Enabled = false;

            tableLayoutPanel_Entra2.Visible = false;
            button_ready.Visible = false;

            //게임에 사용될 컨트롤 들 숨기기
            for (int i = 0; i < 5; i++) {
                dicepool[i].getPicbox.Visible = false;
                enemy_diecpool[i].getPicbox.Visible = false;
            }

            label_enemy.Visible = false;
            label_enemyScore.Visible = false;
            label_enemy_Dicetotal.Visible = false;

            label_player.Visible = false;
            label_playerScore.Visible = false;
            label_playerTurn.Visible = false;
            label_player_Dicetotal.Visible = false;

            btn_roll.Visible = false;

            tableLayoutPanel_players.Visible = false;
            tableLayoutPanel_score.Visible = false;
            
            //테이블의 왼쪽 아래 부터 0.. 그 왼쪽으로 1~2~
            //왼쪽에 더 이상 칼럼이 없다면 그 위쪽 왼쪽 부터 3~4~5 이런식으로 흘러가는듯

            for (int i = 0; i < 30; i += 3) {
                tableLayoutPanel_score.Controls[i].Text = "null";
                tableLayoutPanel_score.Controls[i + 1].Text = "null";
            }

            textBox_log.Text += "주사위 게임 클라이언트 에 오신것을 환영합니다!!\r\n" + 
                "상단에 게임메뉴 에서 '서버에 연결' 을 선택하여 서버에 연결후 '방 만들기' 나 '방 찾기를' 클릭하여 게임을 시작하세요.\r\n";
        }

        private void Form1_close(object sender, EventArgs e)
        {
            if (client != null)
            {
                timer_diceroll.Stop();
                timer_enemyDicerool.Stop();
                timer_ready.Stop();
                timer_receiveOnly.Stop();
                client.client_close();
                client = null;
                _Connect = null;
                _Roomcreate = null;
                _Roomfind = null;
            }
        }

        private void btn_roll_Click(object sender, EventArgs e)//주사위 굴리기 버튼
        {
            timer_diceroll.Start();//주사위가 굴려지는 이밴트를 연출할 timer 를 활성화
            serverReturn = null;
            client.send_recive("d");//서버에 주사위 랜덤값을 요청
            btn_roll.Enabled = false;
        }

        private void timer_diceroll_Tick(object sender, EventArgs e)
        {
            timer_t += 50;
            serverReturn = client.thread_send_end();

            if (!(timer_t >= 1400 && serverReturn != null))
            //주사위 를 굴린지 1400mil초 and 서버에서 값이 왔을시 에 주사위를 굴리는 것을 멈춤
            {
                for (int i = 0; i < 5; i++)
                {
                    dicepool[i].diceroll();
                }
            }
            else
            {
                timer_diceroll.Stop();
                timer_t = 0;
                Debug.WriteLine(serverReturn + "\r\n");
                btn_roll.Enabled = true;

                for (int i = 0; i < 5; i++)
                {
                    Debug.WriteLine("str : " + serverReturn[i] + "\n");
                    dicepool[i].setDice(int.Parse(serverReturn[i + 1].ToString()));
                }

                Game.diceTotal = int.Parse(serverReturn.Substring(6));//주사위 합 표시

                if (Game.nowstate == 3)
                {//호스트 인 경우(선공)
                    Game.turn = 2;//자기 턴 진행 완료를 표시
                    client.receiveOnly();
                    timer_receiveOnly.Start();
                }
                else//아님(후공) 
                {//라운드가 끝남.
                    //호스트가 아니니 후공으로 표시
                    Game.turn = 0;
                    roundSet_score_table_update();
                    Game.nowturn += 1;
                    client.receiveOnly();
                    timer_receiveOnly.Start();
                }
                redraw_gameControl();

                if (serverReturn[0].Equals('e'))
                {//게임이 끝났을 경우
                    Gameover();
                }
            }

        }

        private void ToolStripMenuItem_createroom_Click(object sender, EventArgs e)
        {
            _Roomcreate = new Form_roomcreate(client);
            _Roomcreate.ShowDialog();
            Game = _Roomcreate.returngame;
            if (Game != null) {//방을 만드는데 성공하엿을 경우
                tableLayoutPanel_Entra2.Visible = true;
                button_ready.Visible = true;
                Game.nowstate = 1;//호스트 라는 걸 표시
                Game.turn = 0;//준비상태를 표시
                textBox_log.Text += "방생성 성공!\r\n";
                textBox_log.Text += "방이름 :"+ Game.RoomName +" \r\n";

                client.receiveOnly();//오로직 값을 받기만 하는 스레드를 실행
                //방에 들어오는 다른 클라이언트가 있는지 등등.
                timer_receiveOnly.Start();

                ToolStripMenuItem_createroom.Enabled = false;
            }
        }

        private void ToolStripMenuItem_findroom_Click(object sender, EventArgs e)
        {
            _Roomfind = new Form_roomfind(client);
            _Roomfind.ShowDialog();
            Game = _Roomfind.returngame;
            if (Game != null) {
                Game.nowstate = 2;//호스트가 아님을 표시
                Game.turn = 0;
                textBox_log.Text += "방입장 성공!";
                tableLayoutPanel_Entra2.Visible = true;
                button_ready.Visible = true;
                ToolStripMenuItem_createroom.Enabled = false;
                ToolStripMenuItem_findroom.Enabled = false;
            }
        }

        private void button_ready_Click(object sender, EventArgs e)//게임준비 또는 게임준비 취소 버튼
        {
            if (Game.turn == 0)//버튼을 누르기전 게임준비가 되지않은 상태일 경우 
            {
                client.send_recive("l");
                timer_ready.Start();
            }
            else//준비가 된 상태일 경우(준비취소) 
            {
                client.receive_cancel();
                client.send_recive("c");
                timer_ready.Start();
            }

        }

        private void timer_ready_Tick(object sender, EventArgs e)
        {
            serverReturn = client.thread_send_end();
            if (serverReturn != null) {
                timer_ready.Stop();
                if (int.Parse(serverReturn[0].ToString()) == 1)//클라 둘다 준비됨
                {
                    if (Game.nowstate == 1)
                    {//호스트 일 경우
                        Game.turn = 1;//호스트가 선공권을 가짐
                        Game.nowstate = 3;//자기가 게임을 진행중인 호스트임을 표시
                    }
                    else if (Game.nowstate == 2) 
                    {//호스트 아님
                        Game.turn = 0;//호스트 진행 후
                        Game.nowstate = 4;//일반 유저
                        client.receiveOnly();
                        timer_receiveOnly.Start();
                    }

                    tableLayoutPanel_Entra2.Visible = false;
                    button_ready.Visible = false;

                    for (int i = 0; i < 5; i++)//게임에 사용되는 컨트롤을 표시
                    {
                        dicepool[i].getPicbox.Visible = true;
                        enemy_diecpool[i].getPicbox.Visible = true;
                    }

                    label_enemy.Visible = true;
                    label_enemyScore.Visible = true;
                    label_enemy_Dicetotal.Visible = true;

                    label_player.Visible = true;
                    label_playerScore.Visible = true;
                    label_playerTurn.Visible = true;
                    label_player_Dicetotal.Visible = true;

                    btn_roll.Visible = true;

                    tableLayoutPanel_players.Visible = true;
                    tableLayoutPanel_score.Visible = true;

                    textBox_log.Text += "모든 플레이어 준비 완료\r\n";
                    redraw_gameControl();
                }
                else if (int.Parse(serverReturn[0].ToString()) == 2)//혼자만 준비됨
                {
                    Game.turn = 1;
                    label_turnyou.Text = "READY";
                    button_ready.Text = "CANCEL";
                    client.receiveOnly();
                    timer_receiveOnly.Start();
                }
                else if (int.Parse(serverReturn[0].ToString()) == 3)//서버에서 준비취소가 정상처리된 경우 
                {
                    Game.turn = 0;
                    label_turnyou.Text = "WAIT";
                    button_ready.Text = "READY";
                }
            }
        }

        private void timer_receiveOnly_Tick(object sender, EventArgs e)
        {
            serverReturn = client.thread_receive_end();
            if (serverReturn != null) {
                timer_receiveOnly.Stop();
                Debug.WriteLine(serverReturn);
                if (serverReturn[0].Equals('d') || serverReturn[0].Equals('e'))
                {                                   //상대방에 주사위 롤 을 시작할경우
                                                    //상대방 주사위 를 표현한 컨트롤에 diceroll 을 호출하여 효과를 냄
                    timer_enemyDicerool.Start();    //효과를 연출하고 결과값을 받을 타이머

                }

                else if(int.Parse(serverReturn[0].ToString()) == 2)
                {
                    //방에 다른 클라가 들어온 경우
                    Debug.WriteLine(serverReturn);
                    Game.enemyNikname = serverReturn.Substring(1);
                    Debug.WriteLine(Game.enemyNikname);
                    textBox_log.Text += Game.enemyNikname + " 님이 게임에 참가하엿습니다." + "\r\n";
                }

                else if (int.Parse(serverReturn[0].ToString()) == 1)
                {
                    //모든 클라이언트가 ready = 1일 경우.
                    //timer_ready_Tick 에서 서버에서 받아온 값이 2일경우
                    //상대 클라의 ready 신호를 받기위하여 receiveOnly 를 호출한다.
                    if (Game.nowstate == 1)
                    {//호스트 일 경우
                        Game.turn = 1;//호스트가 선공권을 가짐
                        Game.nowstate = 3;//자기가 게임을 진행중인 호스트임을 표시
                    }
                    else if (Game.nowstate == 2)//그냥 방에 들어온 사람인 경우
                    {
                        Game.turn = 0;//호스트가 선공권을 가짐
                        Game.nowstate = 4;//자기가 게임을 진행중인 호스트임을 표시
                        client.receiveOnly();
                        timer_receiveOnly.Start();
                    }

                    tableLayoutPanel_Entra2.Visible = false;
                    button_ready.Visible = false;

                    //게임에 사용할 컨트롤 들을 이제 표시한다.
                    for (int i = 0; i < 5; i++)
                    {
                        dicepool[i].getPicbox.Visible = true;
                        enemy_diecpool[i].getPicbox.Visible = true;
                    }

                    label_enemy.Visible = true;
                    label_enemyScore.Visible = true;
                    label_enemy_Dicetotal.Visible = true;

                    label_player.Visible = true;
                    label_playerScore.Visible = true;
                    label_playerTurn.Visible = true;
                    label_player_Dicetotal.Visible = true;

                    btn_roll.Visible = true;

                    tableLayoutPanel_players.Visible = true;
                    tableLayoutPanel_score.Visible = true;

                    textBox_log.Text += "모든 플레이어 준비 완료\r\n";
                    redraw_gameControl();
                }
            }
        }

        private void toolStripMenuItem_connect_Click(object sender, EventArgs e)
        {
            _Connect = new Form_connect();//먼저 대화상자의 기반이 될 폼 객채를 생성후
            _Connect.ShowDialog();//대화상자 생성
            client = _Connect.returnClient;//그후 속성에서 객체 받아오기.
            if (client != null) {//클라이언트 객체가 있음 == 서버 연결 성공
                ToolStripMenuItem_createroom.Enabled = true;
                ToolStripMenuItem_findroom.Enabled = true;
                toolStripMenuItem_connect.Enabled = false;
            }
        }

        private void timer_enemyDicerool_Tick(object sender, EventArgs e)
        {
            timer_t += 50;
            serverReturn = client.thread_receive_end();

            if (!(timer_t >= 1400 && serverReturn != null))
            //주사위 를 굴린지 1400mil초 and 서버에서 값이 왔을시 에 주사위를 굴리는 것을 멈춤
            {
                for (int i = 0; i < 5; i++)
                {
                    enemy_diecpool[i].diceroll();
                }
            }
            else
            {
                timer_enemyDicerool.Stop();
                timer_t = 0;
                Debug.WriteLine(serverReturn + "\r\n");

                for (int i = 0; i < 5; i++)
                {
                    Debug.WriteLine("str : " + serverReturn[i] + "\n");
                    enemy_diecpool[i].setDice(int.Parse(serverReturn[i + 1].ToString()));
                }

                Game.enemyDiceTotal = int.Parse(serverReturn.Substring(6));//주사위 합 표시
                if (Game.turn == 2)
                {//자신의 턴이 진행완료 되고 이 메소드가 끝남 = 1라운드 종류
                    //score table update
                    roundSet_score_table_update();
                    Game.nowturn += 1;

                    if (Game.nowstate == 3)
                    {//호스트 인 경우
                        Game.turn = 1;//선공
                    }
                    else
                    {//아님
                        Game.turn = 0;//후공
                    }
                }
                else //  Game.turn == 0 턴 시작하기 전
                {
                    Game.turn = 1;
                }
                redraw_gameControl();

                if (serverReturn[0].Equals('e'))
                {//게임이 끝났을 경우
                    Gameover();
                }
            }

        }

        private void Gameover() {
            if (Game.total_score > Game.enemy_total_score)
            {//자신이 승자일 경우
                label_playerTurn.Text = "YOU WIN!!!!";
                
            }
            else if (Game.total_score < Game.enemy_total_score)
            {//상대가
                label_playerTurn.Text = "YOU LOSE!!!!";

            }
            else if (Game.total_score == Game.enemy_total_score)
            {//동점임
                label_playerTurn.Text = "DRAW!!!!";
            }

            btn_roll.Enabled = false;
        }

        private void roundSet_score_table_update() {//라운드가 끝날경우 가져온 값을 바탕으로 라운드 승자를 판단
            //score_table 에 승자를 표시
            if (Game.diceTotal > Game.enemyDiceTotal)
            {//자신이 승자일 경우
                Game.total_score += 1;
                tableLayoutPanel_score.Controls[29 - ((Game.nowturn - 1) * 3) - 2].Text = "Lose";
                tableLayoutPanel_score.Controls[29 - ((Game.nowturn - 1) * 3) - 1].Text = "Win";
            }
            else if (Game.diceTotal < Game.enemyDiceTotal)
            {//상대가
                Game.enemy_total_score += 1;
                tableLayoutPanel_score.Controls[29 - ((Game.nowturn - 1) * 3) - 2].Text = "Win";
                tableLayoutPanel_score.Controls[29 - ((Game.nowturn - 1) * 3) - 1].Text = "Lose";
            }
            else if (Game.diceTotal == Game.enemyDiceTotal) 
            {//동점임
                tableLayoutPanel_score.Controls[29 - ((Game.nowturn - 1) * 3) - 2].Text = "draw";
                tableLayoutPanel_score.Controls[29 - ((Game.nowturn - 1) * 3) - 1].Text = "draw";
            }
        }

        private void redraw_gameControl() {//게임을 제어 하는 값에 변경이 있고
            //그 값이 컨트롤에 표현되는 값 이면 컨트롤에 text 속성에 다시 삽입
            if (Game.turn == 0 && Game.turn == 2) {
                label_playerTurn.Text = "enemy turn";
            }
            else {
                label_playerTurn.Text = "my turn";
            }

            label_playerScore.Text = "score : " + Game.total_score.ToString();
            label_enemyScore.Text = "score : " + Game.enemy_total_score.ToString();

            label_player_Dicetotal.Text = "Dice total : " + Game.diceTotal.ToString();
            label_enemy_Dicetotal.Text = "Dice total : " + Game.enemyDiceTotal.ToString();

            if (Game.turn != 1)//자신의 턴이 아닌 경우
            {
                btn_roll.Enabled = false;
                label_playerTurn.Text = "round : " + Game.nowturn + " enemy turn";
            }
            else // 자기턴임
            {
                btn_roll.Enabled = true;
                label_playerTurn.Text = "round : " + Game.nowturn + " my turn";
            }

        }

    }
}
