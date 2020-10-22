using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BlackBird
{
    public class Command
    {
        public string command;
        public string parameters;
        public string workingDirector;

        public Command(string command, string parameters, string workingDirectory)
        {
            this.command = command;
            this.parameters = parameters;
            this.workingDirector= workingDirectory;
        }
    }

    public static class ICommandExtensions
    {
        public static byte[] ToBytes(this Command command)
        {
            List<byte[]> buffers = new List<byte[]>();

            buffers.Add(Encoding.UTF8.GetBytes(command.command.ToCharArray()));
            buffers.Add(Encoding.UTF8.GetBytes(command.parameters.ToCharArray()));
            buffers.Add(Encoding.UTF8.GetBytes(command.workingDirector.ToCharArray()));

            byte[] finalBuffer = new byte[(sizeof(int) * 3) + buffers[0].Length + buffers[1].Length + buffers[2].Length];
            int offset = 0;

            Buffer.BlockCopy(BitConverter.GetBytes(buffers[0].Length), 0, finalBuffer, offset, sizeof(int));
            offset += sizeof(int);

            Buffer.BlockCopy(buffers[0], 0, finalBuffer, offset, buffers[0].Length);
            offset += buffers[0].Length;

            Buffer.BlockCopy(BitConverter.GetBytes(buffers[1].Length), 0, finalBuffer, offset, sizeof(int));
            offset += sizeof(int);

            Buffer.BlockCopy(buffers[1], 0, finalBuffer, offset, buffers[1].Length);
            offset += buffers[1].Length;

            Buffer.BlockCopy(BitConverter.GetBytes(buffers[2].Length), 0, finalBuffer, offset, sizeof(int));
            offset += sizeof(int);

            Buffer.BlockCopy(buffers[2], 0, finalBuffer, offset, buffers[2].Length);
            offset += buffers[2].Length;

            return finalBuffer;
        }

        public static Message ToMessage(this Command command)
        {
            Message newMessage = new Message
            {
                message = command.ToBytes()
            };

            newMessage.header = new Message.Header
            {
                messageSize = newMessage.message.Length,
                totalSize = newMessage.message.Length + Message.Header.GetMarshalSize()
            };

            return newMessage;
        }

        public static Command ToCommand(this Message msg)
        {
            byte[] sizeOfString = new byte[sizeof(int)];

            byte[] readBuffer = msg.message;

            //Command

            int offset = 0;
            Buffer.BlockCopy(readBuffer, offset, sizeOfString, 0, sizeof(int));
            offset += sizeof(int);

            int size = BitConverter.ToInt32(sizeOfString, 0);
            byte[] stringBuffer = new byte[size];
            Buffer.BlockCopy(readBuffer, offset, stringBuffer, 0, size);
            offset += size;

            string commmand = Encoding.UTF8.GetString(stringBuffer);

            //Parameters

            Buffer.BlockCopy(readBuffer, offset, sizeOfString, 0, sizeof(int));
            offset += sizeof(int);

            size = BitConverter.ToInt32(sizeOfString, 0);
            stringBuffer = new byte[size];
            Buffer.BlockCopy(readBuffer, offset, stringBuffer, 0, size);
            offset += size;

            string parameters = Encoding.UTF8.GetString(stringBuffer);

            //Working directory

            Buffer.BlockCopy(readBuffer, offset, sizeOfString, 0, sizeof(int));
            offset += sizeof(int);

            size = BitConverter.ToInt32(sizeOfString, 0);
            stringBuffer = new byte[size];
            Buffer.BlockCopy(readBuffer, offset, stringBuffer, 0, size);
            offset += size;

            string workingDirectory = Encoding.UTF8.GetString(stringBuffer);

            return new Command(commmand, parameters, workingDirectory);

        }
    }
        
}
