using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient
{
    public delegate void alarm();
    public partial class Form1 : Form
    {
        
        TcpClient client;
        StreamReader streamReader;
        StreamWriter streamWriter;
        bool flag = true;
        public Form1()
        {
            InitializeComponent();

        }

        private async void Connect()
        {
            try
            {
                client = new TcpClient();
                await client.ConnectAsync("127.0.0.1", 5000);
                if (client.Connected)
                {
                    var stream = client.GetStream();
                    streamReader = new StreamReader(stream);
                    streamWriter = new StreamWriter(stream);
                    streamWriter.AutoFlush = true;

                    ReadMsgs();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("make sure from the server is opened");
            }
        }

        private async void ReadMsgs()
        {
            while (true)
            {
                var msg = await streamReader.ReadLineAsync();
                if (msg == "close")
                {
                    flag = false;
                    break;
                }
                txtReceivedMsgs.Text += msg;
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Connect();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if(client.Connected)
            {
                MessageBox.Show("connect");
                streamWriter.WriteLine(txtMsg.Text);
                txtMsg.Clear();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(client!=null)
            {
                if (flag && client.Connected)
                {
                    streamWriter.WriteLine("close");
                }
            }
                
        }
    }
}
