using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

namespace BlackBird
{
    public class Client
    {
        private ServerInfo infos;
        private TcpClient client;

        private NetworkStream stream;

        public Client(ServerInfo infos)
        {
            this.infos = infos;
            client = new TcpClient();

            client.Connect(infos.ipAddress, infos.port);
            stream = client.GetStream();
        }

        public void WaitForMessage()
        {
            while(!stream.DataAvailable)
            {
                Thread.Sleep(50);
            }
        }

        public bool IsMessageAvailable()
        {
            return stream.DataAvailable;
        }

        public Message ReceiveMessage()
        {
            Message msg = new Message();
            int size = Marshal.SizeOf(msg.header);
            byte[] tmp = new byte[size];
            int received = 0;
            while(received < size)
            {
                received += stream.Read(tmp, received, size - received);
            }

            msg.header.FromBytes(tmp);

            tmp = new byte[msg.header.messageSize];

            received = 0;
            while (received < msg.header.messageSize)
            {
                received += stream.Read(tmp, received, msg.header.messageSize - received);
            }

            msg.message = tmp;

            return msg;
        }

        public void SendMessage(Message message)
        {
            byte[] buffer = message.ToBytes();
            stream.Write(buffer, 0, buffer.Length);
        }


        ~Client()
        {
            client.Close();
        }
    }
}

