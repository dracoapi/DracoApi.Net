using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;
using System;
using System.Collections.Generic;
using System.Text;

namespace DracoProtos.Core.Base
{
    public abstract class FPurchaseResultBase : FObject
    {
        public FAvaUpdate avaUpdate;
        public FCreadex cReadex;
        public Dictionary<FUserCreature, bool> creature;

        public void ReadExternal(FInputStream stream)
        {
            this.avaUpdate = (FAvaUpdate)stream.ReadStaticObject(typeof(FAvaUpdate));
            this.cReadex = (FCreadex)stream.ReadDynamicObject();
            this.creature = stream.ReadStaticMap<FUserCreature, bool>(true, true);
        }

        public void WriteExternal(FOutputStream stream)
        {
            stream.WriteStaticObject(this.avaUpdate);
            stream.WriteDynamicObject(this.cReadex);
            stream.WriteStaticMap(this.creature, true, true);
        }
    }
}
