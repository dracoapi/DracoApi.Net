using System.Collections.Generic;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FUserHatchingInfoBase : FObject
	{
        public List<FEgg> eggs;
        public List<FIncubator> incubators;
        public int max;
        public int maxRoost;

        public void ReadExternal(FInputStream stream)
		{
			this.eggs = stream.ReadStaticList<FEgg>(true);
			this.incubators = stream.ReadStaticList<FIncubator>(true);
			this.max = stream.ReadInt32();
			this.maxRoost = stream.ReadInt32();
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteStaticCollection(this.eggs, true);
			stream.WriteStaticCollection(this.incubators, true);
			stream.WriteInt32(this.max);
			stream.WriteInt32(this.maxRoost);
		}
	}
}
