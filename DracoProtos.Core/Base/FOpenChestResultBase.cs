using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FOpenChestResultBase : FObject
	{
        public FLoot levelUpLoot;
        public FLoot loot;

        public void ReadExternal(FInputStream stream)
		{
			this.levelUpLoot = (FLoot)stream.ReadDynamicObject();
			this.loot = (FLoot)stream.ReadStaticObject(typeof(FLoot));
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteDynamicObject(this.levelUpLoot);
			stream.WriteStaticObject(this.loot);
		}
	}
}
