using System.Collections.Generic;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FQuestUpdateBase : FBaseItemUpdate
	{
        public FQuestCompleted completed;
        public IdAndCoords highlightBuilding;
        public List<IdAndCoords> path;

        public override void ReadExternal(FInputStream stream)
		{
			base.ReadExternal(stream);
			this.completed = (FQuestCompleted)stream.ReadDynamicObject();
			this.highlightBuilding = (IdAndCoords)stream.ReadDynamicObject();
			this.path = stream.ReadDynamicList<IdAndCoords>(true);
		}

		public override void WriteExternal(FOutputStream stream)
		{
			base.WriteExternal(stream);
			stream.WriteDynamicObject(this.completed);
			stream.WriteDynamicObject(this.highlightBuilding);
			stream.WriteDynamicCollection(this.path, true);
		}
	}
}
