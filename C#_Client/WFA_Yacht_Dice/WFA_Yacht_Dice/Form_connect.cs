using System;
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
    public partial class Form_connect : Form
    {
        private client_send_recive client_Send_;

        public client_send_recive returnClient {
            get { return client_Send_; }
            //서버 연결 객체를 만드는 것 을 이 대화상자 에서 수행.
            //대화상자 가 닫히면 이 속성을 통해 form 에서 연결객채를 가져감.
        }
        public Form_connect()
        {
            InitializeComponent();
            client_Send_ = null;
        }

        private void button_connect_Click(object sender, EventArgs e)
        {
            textBox_connectLog.Text += "connect to ip : " + textBox_contextip.Text + " port : 9290\r\n";
            client_Send_ = new client_send_recive(textBox_contextip.Text);
            textBox_connectLog.Text += "server connect success!";

        }
    }
}
