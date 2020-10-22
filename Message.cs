using System;
using System.Runtime.InteropServices;
using System.Text;

namespace BlackBird
{

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Message
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Header
        {
            public static int GetMarshalSize() { return Marshal.SizeOf<Header>(); }

            public int totalSize;
            public int messageSize;

            public byte[] ToBytes()
            {
                int size = Marshal.SizeOf(this);
                byte[] arr = new byte[size];

                IntPtr ptr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(this, ptr, true);
                Marshal.Copy(ptr, arr, 0, size);
                Marshal.FreeHGlobal(ptr);
                return arr;
            }

            public void FromBytes(byte[] arr)
            {
                int size = Marshal.SizeOf(this);
                IntPtr ptr = Marshal.AllocHGlobal(size);

                Marshal.Copy(arr, 0, ptr, size);

                this = (Header)Marshal.PtrToStructure(ptr, this.GetType());
                Marshal.FreeHGlobal(ptr);
            }
        }

        public Header header;
        public byte[] message;

        public byte[] ToBytes()
        {
            header.totalSize = Marshal.SizeOf(this);
            header.messageSize = message.Length;

            byte[] headerToByte = header.ToBytes();
            byte[] ret = new byte[headerToByte.Length + message.Length];
            Buffer.BlockCopy(headerToByte, 0, ret, 0, headerToByte.Length);
            Buffer.BlockCopy(message, 0, ret, headerToByte.Length, message.Length);
            return ret;
        }

        public void FromBytes(byte[] arr)
        {
            int size = Marshal.SizeOf(header);
            byte[] tmp = new byte[size];

            Buffer.BlockCopy(arr, 0, tmp, 0, size);
            header.FromBytes(tmp);

            message = new byte[arr.Length - size];
            Buffer.BlockCopy(arr, size, message, 0, arr.Length - size);
        }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(message) ?? null;
        }
    }

    public static class MessageStringExtension
    {
        public static Message ToMessage(this string str)
        {
            Message newMessage = new Message
            {
                message = Encoding.UTF8.GetBytes(str.ToCharArray())
            };

            newMessage.header = new Message.Header
            {
                messageSize = newMessage.message.Length,
                totalSize = newMessage.message.Length + Message.Header.GetMarshalSize()
            };

            return newMessage;
        }

        
    }
}