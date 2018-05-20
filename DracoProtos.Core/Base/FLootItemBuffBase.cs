using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FLootItemBuffBase : FBaseLootItem
	{
        //public int qty;
        public BuffConfig buff;

        public override void ReadExternal(FInputStream stream)
		{
			base.ReadExternal(stream);
            //this.qty = stream.ReadInt32();
			this.buff = (BuffConfig)stream.ReadStaticObject(typeof(BuffConfig));
		}

		public override void WriteExternal(FOutputStream stream)
		{
			base.WriteExternal(stream);
            //stream.WriteInt32(this.qty);
			stream.WriteStaticObject(this.buff);
		}
	}
}
