using DracoProtos.Core.Enums;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FEggBase : FObject
	{
        public ItemType eggType;
        public string id;
        public string incubatorId;
        public ItemType incubatorType;
        public bool isEggForRoost;
        public bool isHatching;
        public float passedDistance;
        public float totalDistance;
        public long totalIncubationTime;

        public void ReadExternal(FInputStream stream)
		{
			this.eggType = (ItemType)stream.ReadEnum(typeof(ItemType));
			this.id = stream.ReadUtfString();
			this.incubatorId = (string)stream.ReadDynamicObject();
            this.incubatorType = (ItemType)stream.ReadDynamicObject();
			this.isEggForRoost = stream.ReadBoolean();
			this.isHatching = stream.ReadBoolean();
			this.passedDistance = stream.ReadFloat();
			this.totalDistance = stream.ReadFloat();
			this.totalIncubationTime = stream.ReadInt64();
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteEnum(this.eggType);
			stream.WriteUtfString(this.id);
			stream.WriteDynamicObject(this.incubatorId);
            stream.WriteDynamicObject(this.incubatorType);
			stream.WriteBoolean(this.isEggForRoost);
			stream.WriteBoolean(this.isHatching);
			stream.WriteFloat(this.passedDistance);
			stream.WriteFloat(this.totalDistance);
			stream.WriteInt64(this.totalIncubationTime);
		}
	}
}
