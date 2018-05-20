using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FHatchingResultBase : FObject
	{
        public FAvaUpdate avaUpdate;
        public FCreadex cReadex;
        public FUserCreature creature;
        public FLoot levelUpLoot;
        public FLoot loot;

        public void ReadExternal(FInputStream stream)
		{
			this.avaUpdate = (FAvaUpdate)stream.ReadStaticObject(typeof(FAvaUpdate));
			this.cReadex = (FCreadex)stream.ReadDynamicObject();
			this.creature = (FUserCreature)stream.ReadStaticObject(typeof(FUserCreature));
			this.levelUpLoot = (FLoot)stream.ReadStaticObject(typeof(FLoot));
			this.loot = (FLoot)stream.ReadStaticObject(typeof(FLoot));
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteStaticObject(this.avaUpdate);
			stream.WriteDynamicObject(this.cReadex);
			stream.WriteStaticObject(this.creature);
			stream.WriteStaticObject(this.levelUpLoot);
			stream.WriteStaticObject(this.loot);
		}
	}
}
