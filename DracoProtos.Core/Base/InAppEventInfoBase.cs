using System.Collections.Generic;
using DracoProtos.Core.Enums;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class InAppEventInfoBase : FObject
	{
        public Dictionary<string, string> eventItems;
        public InAppEventType eventType;
        public string userId;

        public void ReadExternal(FInputStream stream)
		{
			this.eventItems = stream.ReadStaticMap<string, string>(true, true);
			this.eventType = (InAppEventType)stream.ReadEnum(typeof(InAppEventType));
			this.userId = stream.ReadUtfString();
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteStaticMap(this.eventItems, true, true);
			stream.WriteEnum(this.eventType);
			stream.WriteUtfString(this.userId);
		}
	}
}
