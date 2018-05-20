using System;
using System.IO;
using System.Net;
using System.Text;

namespace DracoProtos.Core.Serializer
{
    public class BigEndianBinaryReader : BinaryReader
    {
        public BigEndianBinaryReader(Stream input)
            : base(input)
        {
        }

        public override int ReadInt32()
        {
            return IPAddress.NetworkToHostOrder(base.ReadInt32());
        }

        public override uint ReadUInt32()
        {
            return (uint)this.ReadInt32();
        }

        public override short ReadInt16()
        {
            return IPAddress.NetworkToHostOrder(base.ReadInt16());
        }

        public override long ReadInt64()
        {
            return IPAddress.NetworkToHostOrder(base.ReadInt64());
        }

        public override string ReadString()
        {
            short num = this.ReadInt16();
            if (num >= 10240)
            {
                throw new Exception("got too long string, len: " + num);
            }
            if (num == 0)
            {
                return string.Empty;
            }
            int num2 = 0;
            do
            {
                num2 = this.Read(this.buff, num2, (int)num);
            }
            while (num2 < (int)num);
            return Encoding.UTF8.GetString(this.buff, 0, (int)num);
        }

        public override float ReadSingle()
        {
            this.buff[3] = base.ReadByte();
            this.buff[2] = base.ReadByte();
            this.buff[1] = base.ReadByte();
            this.buff[0] = base.ReadByte();
            return BitConverter.ToSingle(this.buff, 0);
        }

        public override double ReadDouble()
        {
            this.buff[7] = base.ReadByte();
            this.buff[6] = base.ReadByte();
            this.buff[5] = base.ReadByte();
            this.buff[4] = base.ReadByte();
            this.buff[3] = base.ReadByte();
            this.buff[2] = base.ReadByte();
            this.buff[1] = base.ReadByte();
            this.buff[0] = base.ReadByte();
            return BitConverter.ToDouble(this.buff, 0);
        }

        public const short MAX_BUF_SIZE = 10240;

        private readonly byte[] buff = new byte[10240];
    }
}
