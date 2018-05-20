using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FFightItemBase : FObject
	{
        public float attackerDamageReceived;
        public string attackerId;
        public float defenderDamageReceived;
        public string defenderId;

        public void ReadExternal(FInputStream stream)
		{
			this.attackerDamageReceived = stream.ReadFloat();
			this.attackerId = stream.ReadUtfString();
			this.defenderDamageReceived = stream.ReadFloat();
			this.defenderId = stream.ReadUtfString();
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteFloat(this.attackerDamageReceived);
			stream.WriteUtfString(this.attackerId);
			stream.WriteFloat(this.defenderDamageReceived);
			stream.WriteUtfString(this.defenderId);
		}
	}
}
