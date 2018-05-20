using DracoProtos.Core.Enums;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FLootItemInstantUseItemBase : FBaseLootItem
	{
        //public int qty;
        public InstantUseItem item;

        public override void ReadExternal(FInputStream stream)
		{
			base.ReadExternal(stream);
            //this.qty = stream.ReadInt32();
			this.item = (InstantUseItem)stream.ReadEnum(typeof(InstantUseItem));
		}

		public override void WriteExternal(FOutputStream stream)
		{
			base.WriteExternal(stream);
            //stream.WriteInt32(this.qty);
			stream.WriteEnum(this.item);
		}
	}
}
