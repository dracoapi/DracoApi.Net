using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FArenaBattleResultBase : FBaseItemUpdate
	{
        public bool allyArena;
        public int combinedName;
        public int creaturesDefeated;
        public int currentPrestige;
        public int level;
        public FLoot loot;
        public int nextLevelPrestige;
        public int prestigeBonusKillAll;
        public int prestigeBonusKillStronger;
        public int prestigeChange;
        public int prestigeEarned;
        public float resultScreenDelay;
        public int userExpBonusKillAll;
        public int userExpBonusKillStronger;
        public int userExpGained;
        public bool victory;

        public override void ReadExternal(FInputStream stream)
		{
			base.ReadExternal(stream);
			this.allyArena = stream.ReadBoolean();
			this.combinedName = stream.ReadInt32();
			this.creaturesDefeated = stream.ReadInt32();
			this.currentPrestige = stream.ReadInt32();
			this.level = stream.ReadInt32();
			this.loot = (FLoot)stream.ReadStaticObject(typeof(FLoot));
			this.nextLevelPrestige = stream.ReadInt32();
			this.prestigeBonusKillAll = stream.ReadInt32();
			this.prestigeBonusKillStronger = stream.ReadInt32();
			this.prestigeChange = stream.ReadInt32();
			this.prestigeEarned = stream.ReadInt32();
			this.resultScreenDelay = stream.ReadFloat();
			this.userExpBonusKillAll = stream.ReadInt32();
			this.userExpBonusKillStronger = stream.ReadInt32();
			this.userExpGained = stream.ReadInt32();
			this.victory = stream.ReadBoolean();
		}

		public override void WriteExternal(FOutputStream stream)
		{
			base.WriteExternal(stream);
			stream.WriteBoolean(this.allyArena);
			stream.WriteInt32(this.combinedName);
			stream.WriteInt32(this.creaturesDefeated);
			stream.WriteInt32(this.currentPrestige);
			stream.WriteInt32(this.level);
			stream.WriteStaticObject(this.loot);
			stream.WriteInt32(this.nextLevelPrestige);
			stream.WriteInt32(this.prestigeBonusKillAll);
			stream.WriteInt32(this.prestigeBonusKillStronger);
			stream.WriteInt32(this.prestigeChange);
			stream.WriteInt32(this.prestigeEarned);
			stream.WriteFloat(this.resultScreenDelay);
			stream.WriteInt32(this.userExpBonusKillAll);
			stream.WriteInt32(this.userExpBonusKillStronger);
			stream.WriteInt32(this.userExpGained);
			stream.WriteBoolean(this.victory);
		}
	}
}
