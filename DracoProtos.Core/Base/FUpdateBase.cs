using System.Collections.Generic;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FUpdateBase : FObject
	{
        public List<FBaseItemUpdate> items;
        public long serverTime;

        public void ReadExternal(FInputStream stream)
		{
			this.items = stream.ReadStaticList<FBaseItemUpdate>(false);
			this.serverTime = stream.ReadInt64();
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteStaticCollection(this.items, false);
			stream.WriteInt64(this.serverTime);
		}
	}
}
