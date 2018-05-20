using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FLootItemCoinsBase : FBaseLootItem
	{
        //public int qty;

		public override void ReadExternal(FInputStream stream)
		{
			base.ReadExternal(stream);
            //this.qty = stream.ReadInt32();
		}

		public override void WriteExternal(FOutputStream stream)
		{
			base.WriteExternal(stream);
            //stream.WriteInt32(this.qty);
		}
	}
}
