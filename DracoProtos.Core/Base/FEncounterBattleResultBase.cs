using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FEncounterBattleResultBase : FBaseItemUpdate
	{
        public FLoot loot;
        public float resultScreenDelay;
        public bool victory;

        public override void ReadExternal(FInputStream stream)
		{
			base.ReadExternal(stream);
			this.loot = (FLoot)stream.ReadStaticObject(typeof(FLoot));
			this.resultScreenDelay = stream.ReadFloat();
			this.victory = stream.ReadBoolean();
		}

		public override void WriteExternal(FOutputStream stream)
		{
			base.WriteExternal(stream);
			stream.WriteStaticObject(this.loot);
			stream.WriteFloat(this.resultScreenDelay);
			stream.WriteBoolean(this.victory);
		}
	}
}
