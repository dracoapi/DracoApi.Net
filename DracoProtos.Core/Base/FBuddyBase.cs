using DracoProtos.Core.Enums;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FBuddyBase : FObject
	{
        public string alias;
        public CreatureType candyType;
        public CreatureType creature;
        public int currentWalkedF;
        public int distanceForCandies;
        public string id;
        public int totalCandies;
        public int totalWalkedF;

        public void ReadExternal(FInputStream stream)
		{
			this.alias = (string)stream.ReadDynamicObject();
			this.candyType = (CreatureType)stream.ReadEnum(typeof(CreatureType));
			this.creature = (CreatureType)stream.ReadEnum(typeof(CreatureType));
			this.currentWalkedF = stream.ReadInt32();
			this.distanceForCandies = stream.ReadInt32();
			this.id = stream.ReadUtfString();
			this.totalCandies = stream.ReadInt32();
			this.totalWalkedF = stream.ReadInt32();
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteDynamicObject(this.alias);
			stream.WriteEnum(this.candyType);
			stream.WriteEnum(this.creature);
			stream.WriteInt32(this.currentWalkedF);
			stream.WriteInt32(this.distanceForCandies);
			stream.WriteUtfString(this.id);
			stream.WriteInt32(this.totalCandies);
			stream.WriteInt32(this.totalWalkedF);
		}
	}
}
