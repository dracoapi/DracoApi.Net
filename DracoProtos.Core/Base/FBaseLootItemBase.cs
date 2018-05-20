using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FBaseLootItemBase : FObject
	{
        public int qty;

        public virtual void ReadExternal(FInputStream stream)
		{
			this.qty = stream.ReadInt32();
		}

		public virtual void WriteExternal(FOutputStream stream)
		{
			stream.WriteInt32(this.qty);
		}
	}
}
