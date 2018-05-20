using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FTransferMonsterToCandiesResponseBase : FBaseItemUpdate
	{
        public FLoot loot;

        public override void ReadExternal(FInputStream stream)
		{
			base.ReadExternal(stream);
			this.loot = (FLoot)stream.ReadStaticObject(typeof(FLoot));
		}

		public override void WriteExternal(FOutputStream stream)
		{
			base.WriteExternal(stream);
			stream.WriteStaticObject(this.loot);
		}
	}
}
