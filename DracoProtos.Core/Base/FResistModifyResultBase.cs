using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;
using System;
using System.Collections.Generic;
using System.Text;

namespace DracoProtos.Core.Base
{
    public abstract class FResistModifyResultBase : FObject
    {
        public FUserCreature creature;
        public FResistModifyDetails modifyDetails;
        public bool newResistValue;

        public void ReadExternal(FInputStream stream)
        {
            this.creature = (FUserCreature)stream.ReadStaticObject(typeof(FUserCreature));
            this.modifyDetails = (FResistModifyDetails)stream.ReadStaticObject(typeof(FResistModifyDetails));
            this.newResistValue = stream.ReadBoolean();
        }

        public void WriteExternal(FOutputStream stream)
        {
            stream.WriteStaticObject(this.creature);
            stream.WriteStaticObject(this.modifyDetails);
            stream.WriteBoolean(this.newResistValue);
        }
    }
}
