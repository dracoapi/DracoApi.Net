using System.Collections.Generic;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FChestUpdateBase : FBaseItemUpdate
	{
        public List<FChest> chests;

        public override void ReadExternal(FInputStream stream)
		{
			base.ReadExternal(stream);
			this.chests = stream.ReadStaticList<FChest>(true);
		}

		public override void WriteExternal(FOutputStream stream)
		{
			base.WriteExternal(stream);
			stream.WriteStaticCollection(this.chests, true);
		}
	}
}
