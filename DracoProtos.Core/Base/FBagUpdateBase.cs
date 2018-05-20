using System.Collections.Generic;
using DracoProtos.Core.Enums;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FBagUpdateBase : FObject
	{
        public int allowedItemsSize;
        public List<FBagItem> items;
        public Dictionary<ItemType, int> lockedRunes;

        public void ReadExternal(FInputStream stream)
		{
			this.allowedItemsSize = stream.ReadInt32();
			this.items = stream.ReadStaticList<FBagItem>(true);
			this.lockedRunes = stream.ReadStaticMap<ItemType, int>(true, true);
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteInt32(this.allowedItemsSize);
			stream.WriteStaticCollection(this.items, true);
			stream.WriteStaticMap(this.lockedRunes, true, true);
		}
	}
}
