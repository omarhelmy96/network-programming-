using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    class Client
    {
        StreamReader streamReader;
        StreamWriter streamWriter;

        public event Action<Client, string> MsgReceived;

        public Client(TcpClient tcpClient)
        {
            var stream = tcpClient.GetStream();
            streamReader = new StreamReader(stream);
            streamWriter = new StreamWriter(stream);
            streamWriter.AutoFlush = true;

            ReadMsgs();
        }

        private async void ReadMsgs()
        {
            while (true)
            {

                var msg = await streamReader.ReadLineAsync();

                if (MsgReceived != null)
                {
                    if (msg == "close")
                        break;
                    MsgReceived(this,msg);
                }
            }
        }
        //private void ReadMsgs()
        //{
        //    streamReader.ReadLineAsync().GetAwaiter().OnCompleted(() => MsgReceived(this, " msg"));
        //}

        public async void SendMsgAsync(string msg)
        {
            await streamWriter.WriteLineAsync(msg);
        }
    }
}
