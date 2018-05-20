using System.Collections.Generic;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FFightUpdateBase : FBaseItemUpdate
	{
        public List<FFightCreature> attackers;
        public float autoChangeAttackerHpPercent;
        public string battleId;
        public string defenderNickname;
        public List<FFightCreature> defenders;
        public float dodgeChance;

        public override void ReadExternal(FInputStream stream)
		{
			base.ReadExternal(stream);
			this.attackers = stream.ReadStaticList<FFightCreature>(true);
			this.autoChangeAttackerHpPercent = stream.ReadFloat();
            this.battleId = stream.ReadUtfString();
			this.defenderNickname = (string)stream.ReadDynamicObject();
			this.defenders = stream.ReadStaticList<FFightCreature>(true);
			this.dodgeChance = stream.ReadFloat();
		}

		public override void WriteExternal(FOutputStream stream)
		{
			base.WriteExternal(stream);
			stream.WriteStaticCollection(this.attackers, true);
			stream.WriteFloat(this.autoChangeAttackerHpPercent);
            stream.WriteUtfString(this.battleId);
			stream.WriteDynamicObject(this.defenderNickname);
			stream.WriteStaticCollection(this.defenders, true);
			stream.WriteFloat(this.dodgeChance);
		}
	}
}
