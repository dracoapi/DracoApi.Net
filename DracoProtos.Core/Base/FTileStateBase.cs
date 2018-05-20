using System.Collections.Generic;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FTileStateBase : FObject
	{
        public List<FBuilding> buildings;
        public long time;

        public void ReadExternal(FInputStream stream)
		{
			this.buildings = stream.ReadStaticList<FBuilding>(true);
			this.time = stream.ReadInt64();
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteStaticCollection(this.buildings, true);
			stream.WriteInt64(this.time);
		}
	}
}
