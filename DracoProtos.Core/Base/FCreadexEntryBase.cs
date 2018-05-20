using System.Collections.Generic;
using DracoProtos.Core.Enums;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FCreadexEntryBase : FObject
	{
        public int caughtQuantity;
        public List<FCreadexChain> chain;
        public ElementType element;
        public bool hasGolden;
        public CreatureType name;
        public bool seen;
        public int tier;

        public void ReadExternal(FInputStream stream)
		{
			this.caughtQuantity = stream.ReadInt32();
			this.chain = stream.ReadDynamicList<FCreadexChain>(true);
			this.element = (ElementType)stream.ReadEnum(typeof(ElementType));
            this.hasGolden = stream.ReadBoolean();
			this.name = (CreatureType)stream.ReadEnum(typeof(CreatureType));
			this.seen = stream.ReadBoolean();
			this.tier = stream.ReadInt32();
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteInt32(this.caughtQuantity);
			stream.WriteDynamicCollection(this.chain, true);
			stream.WriteEnum(this.element);
            stream.WriteBoolean(this.hasGolden);
			stream.WriteEnum(this.name);
			stream.WriteBoolean(this.seen);
			stream.WriteInt32(this.tier);
		}
	}
}
