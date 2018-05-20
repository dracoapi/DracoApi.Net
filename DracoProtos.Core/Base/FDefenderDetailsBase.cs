using DracoProtos.Core.Enums;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FDefenderDetailsBase : FObject
	{
        public AllianceType allianceType;
        public string creatureAlias;
        public int creatureCp;
        public CreatureType creatureName;
        public ElementType elementType;
        public int ownerLevel;
        public string ownerName;
        public int userAppearance;

        public void ReadExternal(FInputStream stream)
		{
			this.allianceType = (AllianceType)stream.ReadEnum(typeof(AllianceType));
			this.creatureAlias = (string)stream.ReadDynamicObject();
			this.creatureCp = stream.ReadInt32();
			this.creatureName = (CreatureType)stream.ReadEnum(typeof(CreatureType));
			this.elementType = (ElementType)stream.ReadEnum(typeof(ElementType));
			this.ownerLevel = stream.ReadInt32();
			this.ownerName = stream.ReadUtfString();
			this.userAppearance = stream.ReadInt32();
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteEnum(this.allianceType);
			stream.WriteDynamicObject(this.creatureAlias);
			stream.WriteInt32(this.creatureCp);
			stream.WriteEnum(this.creatureName);
			stream.WriteEnum(this.elementType);
			stream.WriteInt32(this.ownerLevel);
			stream.WriteUtfString(this.ownerName);
			stream.WriteInt32(this.userAppearance);
		}
	}
}
