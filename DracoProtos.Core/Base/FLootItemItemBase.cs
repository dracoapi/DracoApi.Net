using DracoProtos.Core.Enums;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FLootItemItemBase : FBaseLootItem
	{
        //public int qty;
        public bool isStreak;
        public ItemType item;

        public override void ReadExternal(FInputStream stream)
		{
			base.ReadExternal(stream);
            //this.qty = stream.ReadInt32();
			this.isStreak = stream.ReadBoolean();
			this.item = (ItemType)stream.ReadEnum(typeof(ItemType));
		}

		public override void WriteExternal(FOutputStream stream)
		{
			base.WriteExternal(stream);
            //stream.WriteInt32(this.qty);
			stream.WriteBoolean(this.isStreak);
			stream.WriteEnum(this.item);
		}
	}
}
