using System.Collections.Generic;
using DracoProtos.Core.Enums;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FDailyQuestBase : FObject
	{
        public int count;
        public ElementType? elementType;
        public string id;
        public int nextDailyQuestIn;
        public List<IdAndCoords> pitstopPath;
        public int progress;
        public QuestType? type;

        public void ReadExternal(FInputStream stream)
		{
			this.count = stream.ReadInt32();
			this.elementType = (ElementType?)stream.ReadDynamicObject();
			this.id = (string)stream.ReadDynamicObject();
			this.nextDailyQuestIn = stream.ReadInt32();
			this.pitstopPath = stream.ReadDynamicList<IdAndCoords>(true);
			this.progress = stream.ReadInt32();
			this.type = (QuestType?)stream.ReadDynamicObject();
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteInt32(this.count);
			stream.WriteDynamicObject(this.elementType);
			stream.WriteDynamicObject(this.id);
			stream.WriteInt32(this.nextDailyQuestIn);
			stream.WriteDynamicCollection(this.pitstopPath, true);
			stream.WriteInt32(this.progress);
			stream.WriteDynamicObject(this.type);
		}
	}
}
