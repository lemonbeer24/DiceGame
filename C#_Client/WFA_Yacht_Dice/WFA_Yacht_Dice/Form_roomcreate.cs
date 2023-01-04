using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFA_Yacht_Dice
{
    public partial class Form_roomcreate : Form
    {
        private game Game;

        private client_send_recive conn;
        private string serverReturn;

        private int time = 0;

        public game returngame {
            get { return Game; }
        }

        public Form_roomcreate(client_send_recive conn)
        {
            InitializeComponent();
            this.conn = conn;
            Game = null;
        }

        private void button_roomcreate_Click(object sender, EventArgs e)
        {
            textBox_nickname.Enabled = false;
            textBox_roomname.Enabled = false;
            textBox_rog.Text += "방이름 : " + textBox_roomname.Text + " 닉네임 :  " + textBox_nickname.Text + "\r\n";
            textBox_rog.Text += "서버에 요청을 전송...\r\n";
            conn.send_recive("rc" + textBox_roomname.Text + "&" + textBox_nickname.Text + "&");//패킷 문자열 조립하여 전송.
            timer_receiveTime.Start();
        }

        private void timer_receiveTime_Tick(object sender, EventArgs e)
            //cilent 객체 스레드 에서 서버한테 값이 왓는지 확인
        {
            serverReturn = conn.thread_send_end();
            time += 10;

            if (serverReturn != null)
            {
                int sig = int.Parse(serverReturn[0].ToString());
                if (sig == 1)
                {//방 만들기 성공
                    timer_receiveTime.Stop();
                    Game = new game(textBox_roomname.Text, textBox_nickname.Text);
                    Game.nowstate = 1;
                    textBox_rog.Text += "방 만들기 성공!\r\n";

                }
                else if (serverReturn.Equals("err"))
                {//실패
                    timer_receiveTime.Stop();
                    Game = null;
                    textBox_rog.Text += "error : 서버에서 방의 생성을 실패하였습니다.\r\n";
                }
                else if (serverReturn == null)
                {
                    if (time % 3000 == 0)
                    {//응답이 없음(3초 간격)
                        textBox_rog.Text += "....\r\n";
                        if (time >= 16000)
                        {//시간초과(16초)
                            timer_receiveTime.Stop();
                            Game = null;
                            textBox_rog.Text += "error : 서버에서의 응답이 없습니다.\r\n";
                        }
                    }
                }
                else {
                    timer_receiveTime.Stop();
                    Game = null;
                    textBox_rog.Text += "알수 없는 패킷 문자열! : &" + serverReturn + "&";
                }       

            }
        }

    }
}
