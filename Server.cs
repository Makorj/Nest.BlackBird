using System;
using System.Text;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace BlackBird
{
    public struct ServerInfo
    {
        public IPAddress ipAddress;
        public int port;
    }

    public class Server
    {
        public static int VerboseLevel = 0;

        private ServerInfo infos;
        private TcpListener listener;

        private List<Socket> connections;

        public Server(ServerInfo infos, bool startNow = true)
        {
            this.infos = infos;
            listener = new TcpListener(infos.ipAddress, infos.port);
            connections = new List<Socket>();

            if (startNow)
                listener.Start();
        }

        public Message ReceiveMessageFromSocket(Socket s)
        {
            byte[] headerBuffer = new byte[Message.Header.GetMarshalSize()];

            int totalReceived = 0;
            int received = 0;
            if (received < headerBuffer.Length)
            {
                received += s.Receive(headerBuffer, received, headerBuffer.Length - received, SocketFlags.None);
            }

            totalReceived += received;

            Message.Header header = new Message.Header();
            header.FromBytes(headerBuffer);
            byte[] messageBuffer = new byte[header.messageSize];

            received = 0;
            if (received < header.messageSize)
            {
                received += s.Receive(messageBuffer, received, header.messageSize - received, SocketFlags.None);
            }

            Message tmp = new Message
            {
                header = header,
                message = messageBuffer
            };

            Log(new string[]
            {
                null,
                null,
                "Message received",
                $"Received {received + totalReceived} bytes",
                $"Received this message : \n{tmp} \nfrom : \n{(IPEndPoint)s.RemoteEndPoint}"
            });

            return tmp;
        }

        public void SendMessage(Message message, Socket s)
        {
            byte[] buffer = message.ToBytes();
            int sent = 0;
            while (sent < buffer.Length)
            {
                sent += s.Send(buffer, sent, buffer.Length - sent, SocketFlags.None);
            }

            Log(new string[]
            {
                null,
                null,
                "Message sent",
                $"Sent {sent} bytes",
                $"Sent this message : \n{message} \nto : \n{(IPEndPoint)s.RemoteEndPoint}"
            });
        }

        public bool PendingNewConnection => listener.Pending();
        public Socket GetPendingSocket => listener.AcceptSocket();

        public Task<Socket> GetPendingSocketAsync => listener.AcceptSocketAsync();

        public void Stop()
        {
            listener.Stop();

            Log(new string[]
            {
                null,
                null,
                null,
                "Stopped listening"
            });
        }

        public void Start()
        {
            listener.Start();

            Log(new string[]
            {
                null,
                null,
                null,
                "Started listening"
            });
        }

        ~Server()
        {
            listener.Stop();
        }

        private void Log(string[] message)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            if(message.Length-1 >= VerboseLevel)
            {
                if(message[VerboseLevel] != null)
                {
                    Console.WriteLine($"[{VerboseLevel}] BlackBird.Server : {message[VerboseLevel]}");
                }
            }
            else
            {
                if (message[message.Length-1] != null)
                {
                    Console.WriteLine($"[{VerboseLevel}] BlackBird.Server : {message[message.Length-1]}");
                }
            }
            Console.ResetColor();
        }
    }
}

