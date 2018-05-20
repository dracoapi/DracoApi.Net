using DracoProtos.Core.Enums;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FBuffBase : FObject
	{
        public BuffType buffType;
        public long duration;
        public long timeLeft;
        public long timeToActivation;
        public int valuePercent;

        public void ReadExternal(FInputStream stream)
		{
			this.buffType = (BuffType)stream.ReadEnum(typeof(BuffType));
			this.duration = stream.ReadInt64();
			this.timeLeft = stream.ReadInt64();
            this.timeToActivation = stream.ReadInt64();
            this.valuePercent = stream.ReadInt32();
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteEnum(this.buffType);
			stream.WriteInt64(this.duration);
			stream.WriteInt64(this.timeLeft);
            stream.WriteInt64(this.timeToActivation);
			stream.WriteInt32(this.valuePercent);
		}
	}
}
