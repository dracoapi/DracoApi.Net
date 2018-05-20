using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class ProductLotBase : FObject
	{
        public int price;
        public int qty;

        public void ReadExternal(FInputStream stream)
		{
			this.price = stream.ReadInt32();
			this.qty = stream.ReadInt32();
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteInt32(this.price);
			stream.WriteInt32(this.qty);
		}
	}
}
