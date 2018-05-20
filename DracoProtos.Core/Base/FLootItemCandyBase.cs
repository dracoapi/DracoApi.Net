using DracoProtos.Core.Enums;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FLootItemCandyBase : FBaseLootItem
	{
        //public int qty;
        public CreatureType candyType;
        public bool fromBuddy;

        public override void ReadExternal(FInputStream stream)
		{
			base.ReadExternal(stream);
            //this.qty = stream.ReadInt32();
			this.candyType = (CreatureType)stream.ReadEnum(typeof(CreatureType));
            this.fromBuddy = stream.ReadBoolean();
		}

		public override void WriteExternal(FOutputStream stream)
		{
			base.WriteExternal(stream);
            //stream.WriteInt32(this.qty);
			stream.WriteEnum(this.candyType);
            stream.WriteBoolean(this.fromBuddy);
		}
	}
}
