using DracoProtos.Core.Enums;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class BuffConfigBase : FObject
	{
        public long durationMs;
        public bool isOffer;
        public BuffType type;
        public int valuePercent;

        public void ReadExternal(FInputStream stream)
		{
			this.durationMs = stream.ReadInt64();
            this.isOffer = stream.ReadBoolean();
			this.type = (BuffType)stream.ReadEnum(typeof(BuffType));
			this.valuePercent = stream.ReadInt32();
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteInt64(this.durationMs);
            stream.WriteBoolean(this.isOffer);
			stream.WriteEnum(this.type);
			stream.WriteInt32(this.valuePercent);
		}
	}
}
