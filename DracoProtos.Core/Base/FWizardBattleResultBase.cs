using System.Collections.Generic;
using DracoProtos.Core.Enums;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FWizardBattleResultBase : FBaseItemUpdate
	{
        public List<float> attackerHps;
        public List<CreatureType> attackerTypes;
        public FAvaUpdate avaUpdate;
        public List<CreatureType> candies;
        public FLoot levelUpLoot;
        public FLoot loot;
        public float resultScreenDelay;
        public float rewardPercent;
        public bool victory;

        public override void ReadExternal(FInputStream stream)
		{
			base.ReadExternal(stream);
			this.attackerHps = stream.ReadStaticList<float>(true);
            this.attackerTypes = stream.ReadStaticList<CreatureType>(true);
            this.avaUpdate = (FAvaUpdate)stream.ReadStaticObject(typeof(FAvaUpdate));
            this.candies = stream.ReadStaticList<CreatureType>(true);
 			this.levelUpLoot = (FLoot)stream.ReadStaticObject(typeof(FLoot));
			this.loot = (FLoot)stream.ReadStaticObject(typeof(FLoot));
			this.resultScreenDelay = stream.ReadFloat();
            this.rewardPercent = stream.ReadFloat();
            this.victory = stream.ReadBoolean();
		}

		public override void WriteExternal(FOutputStream stream)
		{
			base.WriteExternal(stream);
			stream.WriteStaticCollection(this.attackerHps, true);
			stream.WriteStaticCollection(this.attackerTypes, true);
            stream.WriteStaticCollection(this.candies, true);
			stream.WriteStaticObject(this.levelUpLoot);
			stream.WriteStaticObject(this.loot);
			stream.WriteFloat(this.resultScreenDelay);
            stream.WriteFloat(this.rewardPercent);
            stream.WriteBoolean(this.victory);
		}
	}
}
