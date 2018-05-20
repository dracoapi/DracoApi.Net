using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace DracoProtos.Core.Serializer
{
    public class BigEndianBinaryWriter : BinaryWriter
    {
        public BigEndianBinaryWriter(Stream output)
            : base(output)
        {
        }

        public override void Write(int value)
        {
            base.Write(IPAddress.HostToNetworkOrder(value));
        }

        public override void Write(uint value)
        {
            this.Write((int)value);
        }

        public override void Write(long value)
        {
            base.Write(IPAddress.HostToNetworkOrder(value));
        }

        public override void Write(short value)
        {
            base.Write(IPAddress.HostToNetworkOrder(value));
        }

        public override void Write(ushort value)
        {
            base.Write(IPAddress.HostToNetworkOrder((int)value));
        }

        public override void Write(float value)
        {
            this.Write(new BigEndianBinaryWriter.IntFloat
            {
                floatValue = value
            }.intValue);
        }

        public override void Write(double value)
        {
            this.Write(new BigEndianBinaryWriter.LongDouble
            {
                doubleValue = value
            }.longValue);
        }

        public override void Write(string value)
        {
            short num = (short)Encoding.UTF8.GetBytes(value, 0, value.Length, this.buff, 0);
            this.Write(num);
            this.Write(this.buff, 0, (int)num);
        }

        public const int MAX_SEND_BYTES = 10240;

        private readonly byte[] buff = new byte[10240];

        [StructLayout(LayoutKind.Explicit)]
        private struct IntFloat
        {
            [FieldOffset(0)]
            public float floatValue;

            [FieldOffset(0)]
            public readonly int intValue;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct LongDouble
        {
            [FieldOffset(0)]
            public double doubleValue;

            [FieldOffset(0)]
            public readonly long longValue;
        }
    }
}
