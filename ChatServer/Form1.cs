using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatServer
{
    public partial class Form1 : Form
    {
        TcpListener listener;
        List<Client> clients = new List<Client>();

        public Form1()
        {
            InitializeComponent();
        }

        private async void AcceptConnection()
        {
            listener.Start();

            while (true)
            {
                var tcpClient = await listener.AcceptTcpClientAsync();

                Client client = new Client(tcpClient);
                client.MsgReceived += Client_MsgReceived;
                clients.Add(client);
            }
        }

        private void Client_MsgReceived(Client sender, string msg)
        {
            txtReceivedMsgs.Text += msg;
            
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            listener = new TcpListener(ip, 5000);
            AcceptConnection();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {

            foreach (var client in clients)
            {
                client.SendMsgAsync(txtMsg.Text);
            }

            txtMsg.Clear();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (clients.Count > 0)
            {
                foreach (var client in clients)
                {
                    client.SendMsgAsync("close");
                }
            }
        }
    }
}
