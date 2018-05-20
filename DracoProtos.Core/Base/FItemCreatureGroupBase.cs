using DracoProtos.Core.Enums;
using DracoProtos.Core.Serializer;
using System;
using System.Collections.Generic;
using System.Text;

namespace DracoProtos.Core.Base
{
    public abstract class FItemCreatureGroupBase : FObject
    {
        public bool active;
        public CreatureType creature;
        public Dictionary<ItemType, int> items;
        public int price;

        public void ReadExternal(FInputStream stream)
        {
            this.active = stream.ReadBoolean();
            this.creature = (CreatureType)stream.ReadDynamicObject();
            this.items = stream.ReadStaticMap<ItemType, int>(true, true);
            this.price = stream.ReadInt32();
        }

        public void WriteExternal(FOutputStream stream)
        {
            stream.WriteBoolean(this.active);
            stream.WriteDynamicObject(this.creature);
            stream.WriteStaticMap(this.items, true, true);
            stream.WriteInt32(this.price);
        }
    }
}
