using System.Collections.Generic;
using DracoProtos.Core.Enums;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class ProductGroupBase : FObject
	{
        public ItemType itemType;
        public List<ProductLot> productLots;

        public void ReadExternal(FInputStream stream)
		{
			this.itemType = (ItemType)stream.ReadEnum(typeof(ItemType));
			this.productLots = stream.ReadStaticList<ProductLot>(true);
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteEnum(this.itemType);
			stream.WriteStaticCollection(this.productLots, true);
		}
	}
}
