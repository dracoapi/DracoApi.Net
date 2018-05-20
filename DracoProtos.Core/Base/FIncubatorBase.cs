using DracoProtos.Core.Enums;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FIncubatorBase : FObject
	{
        public string eggId;
        public string incubatorId;
        public ItemType itemType;
        public string roostBuildingId;
        public int slotIndex;
        public int usagesLeft;

        public void ReadExternal(FInputStream stream)
		{
			this.eggId = (string)stream.ReadDynamicObject();
			this.incubatorId = stream.ReadUtfString();
            this.itemType = (ItemType)stream.ReadDynamicObject();
			this.roostBuildingId = (string)stream.ReadDynamicObject();
			this.slotIndex = stream.ReadInt32();
			this.usagesLeft = stream.ReadInt32();
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteDynamicObject(this.eggId);
			stream.WriteUtfString(this.incubatorId);
            stream.WriteDynamicObject(this.itemType);
			stream.WriteDynamicObject(this.roostBuildingId);
			stream.WriteInt32(this.slotIndex);
			stream.WriteInt32(this.usagesLeft);
		}
	}
}
