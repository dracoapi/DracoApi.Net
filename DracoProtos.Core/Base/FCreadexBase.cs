using System.Collections.Generic;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FCreadexBase : FObject
	{
        public List<FCreadexEntry> entries;

        public void ReadExternal(FInputStream stream)
		{
			this.entries = stream.ReadStaticList<FCreadexEntry>(true);
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteStaticCollection(this.entries, true);
		}
	}
}
