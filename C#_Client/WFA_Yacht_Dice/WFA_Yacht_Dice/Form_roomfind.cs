using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace WFA_Yacht_Dice
{
    public partial class Form_roomfind : Form
    {
        private game Game;
        private client_send_recive conn;
        private List<room> rooms;
        private string serverReturn;

        public game returngame
        {
            get { return Game; }
        }

        public Form_roomfind(client_send_recive conn)
        {
            InitializeComponent();
            this.conn = conn;
            rooms = new List<room>();

            conn.send_recive("rf");
            timer_receive.Start();
            button_refresh.Enabled = false;
            button_Entry.Enabled = false;
        }

        private void button_Entry_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(rooms[listBox_roomlist.SelectedIndex].roomName);
            //game 객체 반환하여야함
            textBox_rog.Text += "방 이름 : " + rooms[listBox_roomlist.SelectedIndex].roomName + "\r\n";
            textBox_rog.Text += "서버에 요청 보내는 중...\r\n";
            conn.send_recive("rj" + rooms[listBox_roomlist.SelectedIndex].fd.ToString() + textBox_nickname.Text);//해당 fd(호스트) 와 자신의 닉네임을 를 같이보냄
            timer_join.Start();

        }

        private void button_refresh_Click(object sender, EventArgs e)
        {
            conn.send_recive("rf");
            timer_receive.Start();
            button_refresh.Enabled = false;
            button_Entry.Enabled = false;
        }

        private class room {
            public string roomName;
            public int fd;

            public room(string roomName, int fd) {
                this.roomName = roomName;
                this.fd = fd;
            }
        }

        private void timer_receive_Tick(object sender, EventArgs e)
        {
            //받아오는 패킷 문자열 : 방이름|fd번호&반복...&반복...eof
            //ex : room1|5&room2|2
            string[] substr;//&을 기준으로 잘려진 문자열 들을 저장할 배열
            string[] fdSubStr;//| 을 기준으로 잘라진 방이름과 fd번호 문자열을 저장
            serverReturn = conn.thread_send_end();
            if (serverReturn != null) {
                timer_receive.Stop();
                substr = serverReturn.Split('&');
                for (int i = 0; i < substr.Length; i++) { 
                    fdSubStr = substr[i].Split('|');
                    if (fdSubStr.Length > 1) {
                        rooms.Add(new room(fdSubStr[0], int.Parse(fdSubStr[1])));
                        listBox_roomlist.Items.Add(rooms[i].roomName);
                    }
                }
                button_Entry.Enabled = true;
                button_refresh.Enabled = true;
            }

        }

        private void timer_join_Tick(object sender, EventArgs e)
        {
            serverReturn = conn.thread_send_end();
            if (serverReturn != null) {
                timer_join.Stop();
                if (int.Parse(serverReturn[0].ToString()) == 2)
                {
                    Game = new game(rooms[listBox_roomlist.SelectedIndex].roomName, textBox_nickname.Text);
                    textBox_rog.Text += "참가 성공!\r\n";
                }
                else if (int.Parse(serverReturn[0].ToString()) == -1)
                {
                    textBox_rog.Text += "방을 생성한 호스트가 없거나 이미 다른 클라이언트와 게임을 시작하였습니다\r\n";
                }
                else 
                {
                    textBox_rog.Text += "알수 없는 패킷!\r\n";
                }
            }
        }
    }

}
