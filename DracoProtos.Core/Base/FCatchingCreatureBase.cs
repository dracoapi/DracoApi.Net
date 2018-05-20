using System.Collections.Generic;
using DracoProtos.Core.Enums;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FCatchingCreatureBase : FObject
	{
        public bool aggressive;
        public CreatureType candyType;
        public FCatchingConfig catching;
        public int cp;
        public ElementType element;
        public ItemType? feedFoodType;
        public int feedLeftTime;
        public string id;
        public bool isCreatureStorageFull;
        public Dictionary<ItemType, int> items;
        public CreatureType name;
        public float quality;

        public void ReadExternal(FInputStream stream)
		{
			this.aggressive = stream.ReadBoolean();
			this.candyType = (CreatureType)stream.ReadEnum(typeof(CreatureType));
			this.catching = (FCatchingConfig)stream.ReadDynamicObject();
			this.cp = stream.ReadInt32();
			this.element = (ElementType)stream.ReadEnum(typeof(ElementType));
			this.feedFoodType = (ItemType?)stream.ReadDynamicObject();
			this.feedLeftTime = stream.ReadInt32();
			this.id = stream.ReadUtfString();
			this.isCreatureStorageFull = stream.ReadBoolean();
			this.items = stream.ReadStaticMap<ItemType, int>(true, true);
			this.name = (CreatureType)stream.ReadEnum(typeof(CreatureType));
			this.quality = stream.ReadFloat();
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteBoolean(this.aggressive);
			stream.WriteEnum(this.candyType);
			stream.WriteDynamicObject(this.catching);
			stream.WriteInt32(this.cp);
			stream.WriteEnum(this.element);
			stream.WriteDynamicObject(this.feedFoodType);
			stream.WriteInt32(this.feedLeftTime);
			stream.WriteUtfString(this.id);
			stream.WriteBoolean(this.isCreatureStorageFull);
			stream.WriteStaticMap(this.items, true, true);
			stream.WriteEnum(this.name);
			stream.WriteFloat(this.quality);
		}
	}
}
