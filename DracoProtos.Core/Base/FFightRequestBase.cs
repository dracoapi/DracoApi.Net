using System.Collections.Generic;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FFightRequestBase : FObject
	{
        public string battleId;
        public int chargedAttacksByAi;
        public int dodges;
        public List<FFightItem> items;
        public bool leaveBattle;
        public int successfulDodges;
        public bool timeout;

        public void ReadExternal(FInputStream stream)
		{
            this.battleId = stream.ReadUtfString();
			this.chargedAttacksByAi = stream.ReadInt32();
			this.dodges = stream.ReadInt32();
			this.items = stream.ReadStaticList<FFightItem>(true);
			this.leaveBattle = stream.ReadBoolean();
			this.successfulDodges = stream.ReadInt32();
            this.timeout = stream.ReadBoolean();
        }

		public void WriteExternal(FOutputStream stream)
		{
            stream.WriteUtfString(this.battleId);
			stream.WriteInt32(this.chargedAttacksByAi);
			stream.WriteInt32(this.dodges);
			stream.WriteStaticCollection(this.items, true);
			stream.WriteBoolean(this.leaveBattle);
			stream.WriteInt32(this.successfulDodges);
            stream.WriteBoolean(this.timeout);
		}
	}
}
