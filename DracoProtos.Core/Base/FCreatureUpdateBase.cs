using System.Collections.Generic;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FCreatureUpdateBase : FBaseItemUpdate
	{
        public List<FWildCreature> inRadar;
        public List<FWildCreature> wilds;

        public override void ReadExternal(FInputStream stream)
		{
			base.ReadExternal(stream);
			this.inRadar = stream.ReadStaticList<FWildCreature>(true);
			this.wilds = stream.ReadStaticList<FWildCreature>(true);
		}

		public override void WriteExternal(FOutputStream stream)
		{
			base.WriteExternal(stream);
			stream.WriteStaticCollection(this.inRadar, true);
			stream.WriteStaticCollection(this.wilds, true);
		}
	}
}
