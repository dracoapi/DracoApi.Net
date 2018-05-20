using System.Collections.Generic;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FLootBase : FBaseItemUpdate
	{
        public List<FBaseLootItem> lootList;
        public int streakIndex;

        public override void ReadExternal(FInputStream stream)
		{
			base.ReadExternal(stream);
			this.lootList = stream.ReadStaticList<FBaseLootItem>(false);
            this.streakIndex = stream.ReadInt32();
		}

		public override void WriteExternal(FOutputStream stream)
		{
			base.WriteExternal(stream);
			stream.WriteStaticCollection(this.lootList, false);
            stream.WriteInt32(this.streakIndex);
		}
    }
}
