using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FHatchedEggsBase : FBaseItemUpdate
	{
        public FEgg egg;
        public string incubatorId;
        public FLoot loot;

        public override void ReadExternal(FInputStream stream)
		{
			base.ReadExternal(stream);
			this.egg = (FEgg)stream.ReadStaticObject(typeof(FEgg));
			this.incubatorId = stream.ReadUtfString();
			this.loot = (FLoot)stream.ReadStaticObject(typeof(FLoot));
		}

		public override void WriteExternal(FOutputStream stream)
		{
			base.WriteExternal(stream);
			stream.WriteStaticObject(this.egg);
			stream.WriteUtfString(this.incubatorId);
			stream.WriteStaticObject(this.loot);
		}
	}
}
