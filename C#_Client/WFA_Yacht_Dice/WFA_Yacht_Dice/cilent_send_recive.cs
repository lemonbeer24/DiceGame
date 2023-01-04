using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WFA_Yacht_Dice
{
    public class client_send_recive
    {
        public const int BUFSIZE = 48;

        private Socket client;
        private sendReceiveObj send, receive;
        private Thread thread_send, thread_receive_only;

        public byte[] receiveBuf, receiveBuf2;

        public client_send_recive(string ipaddr)
        {
            //클라이언트 소켓 초기화
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            receiveBuf = new byte[BUFSIZE];
            receiveBuf2 = new byte[BUFSIZE];

            client.Connect(ipaddr, 9290);

            send = new sendReceiveObj("", client, receiveBuf);
            receive = new sendReceiveObj(client, receiveBuf2);
        }

        public void client_close() {
            receive_cancel();
            receive_only_cancel();
            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }

        public void send_recive(string msg/*, int requestOpt*/) {//전송하고 값을 받는 스레드를 생성
            send = new sendReceiveObj("", client, receiveBuf);
            send.receiveBuf = new byte[BUFSIZE];
            send.sendMsg = msg;
            thread_send = new Thread(new ThreadStart(send.server_send));
            thread_send.Start();
        }

        public string thread_send_end()//전송한후 전송받은 값을 받고 스레드를 끝냄
        {
            if (thread_send.IsAlive == false) {
                thread_send.Join(1);
                return Encoding.ASCII.GetString(send.receiveBuf).Trim();
            }

            return null;
        }

        public void receiveOnly() {//오로직 받기만함.
            //send_recive 가 사용하는 sendReceiveObj, thread 객체가 다르므로
            //send_recive, receiveOnly 이 메소드 들을 동시에 사용이 가능.
            receive = new sendReceiveObj(client, receiveBuf2);
            thread_receive_only = new Thread(new ThreadStart(receive.receive_only));
            thread_receive_only.Start();
        }

        public string thread_receive_end() {
            if (thread_receive_only.IsAlive == false) {
                thread_receive_only.Join(1);
                return Encoding.ASCII.GetString(receive.receiveBuf);
            }
            return null;
        }

        public void receive_cancel() {//전송 스레드(받는쪽) 강제종료
            if (thread_send != null)
            {
                if (thread_send.IsAlive == true)
                {
                    thread_send.Abort();
                    Debug.WriteLine("receive thread1 cancel\n");
                }
            }
        }

        public void receive_only_cancel() {
            if (thread_receive_only != null)
            {
                if (thread_receive_only.IsAlive == true)
                {
                    thread_receive_only.Abort();
                    Debug.WriteLine("receive thread2 cancel\n");
                }
            }
        }

        private class sendReceiveObj {

            public int receiveSize;
            public byte[] receiveBuf;
            
            private Socket client;

            public string sendMsg;

            public sendReceiveObj(string msg, Socket client, byte[] buf) {
                this.client = client;
                receiveBuf = buf;
                sendMsg = msg;
            }

            public sendReceiveObj(Socket client, byte[] buf) {//receive only
                this.client = client;
                receiveBuf = buf;
            }

            public void receive_only() {
                receiveSize = client.Receive(receiveBuf);
                Debug.WriteLine("receive thread end\n");
            }

            public void server_send() {
                client.Send(Encoding.ASCII.GetBytes(sendMsg));
                
                receiveSize = client.Receive(receiveBuf);
                Debug.WriteLine("thread end\n");
            }

        }

    }
}
