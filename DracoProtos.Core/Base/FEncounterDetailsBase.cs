using DracoProtos.Core.Enums;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FEncounterDetailsBase : FBaseItemUpdate
	{
        public GeoCoords coords;
        public int creatureCp;
        public ElementType creatureElementType;
        public CreatureType creatureName;
        public bool extraEncounter;
        public string id;
        public int level;

        public override void ReadExternal(FInputStream stream)
		{
			base.ReadExternal(stream);
			this.coords = (GeoCoords)stream.ReadStaticObject(typeof(GeoCoords));
			this.creatureCp = stream.ReadInt32();
			this.creatureElementType = (ElementType)stream.ReadEnum(typeof(ElementType));
			this.creatureName = (CreatureType)stream.ReadEnum(typeof(CreatureType));
            this.extraEncounter = stream.ReadBoolean();
            this.id = stream.ReadUtfString();
			this.level = stream.ReadInt32();
		}

		public override void WriteExternal(FOutputStream stream)
		{
			base.WriteExternal(stream);
			stream.WriteStaticObject(this.coords);
			stream.WriteInt32(this.creatureCp);
			stream.WriteEnum(this.creatureElementType);
			stream.WriteEnum(this.creatureName);
            stream.WriteBoolean(this.extraEncounter);
			stream.WriteUtfString(this.id);
			stream.WriteInt32(this.level);
		}
	}
}
