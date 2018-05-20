using System.Collections.Generic;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FInAppEventUpdateBase : FBaseItemUpdate
	{
        public List<InAppEventInfo> events;

        public override void ReadExternal(FInputStream stream)
		{
			base.ReadExternal(stream);
			this.events = stream.ReadStaticList<InAppEventInfo>(true);
		}

		public override void WriteExternal(FOutputStream stream)
		{
			base.WriteExternal(stream);
			stream.WriteStaticCollection(this.events, true);
		}
	}
}
