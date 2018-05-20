using DracoProtos.Core.Enums;
using DracoProtos.Core.Serializer;
using System;
using System.Collections.Generic;
using System.Text;

namespace DracoProtos.Core.Base
{
    public abstract class SaleSetConfigBase : FObject
    {
        public Dictionary<ItemType, int> items;
        public int price;
        public SaleSetType type;

        public void ReadExternal(FInputStream stream)
        {
            this.items = stream.ReadStaticMap<ItemType, int>(true, true);
            this.price = stream.ReadInt32();
            this.type = (SaleSetType)stream.ReadDynamicObject();
        }

        public void WriteExternal(FOutputStream stream)
        {
            stream.WriteStaticMap(this.items, true, true);
            stream.WriteInt32(this.price);
            stream.WriteDynamicObject(this.type);
        }
    }
}
