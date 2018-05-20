using DracoProtos.Core.Serializer;
using System;
using System.Collections.Generic;
using System.Text;

namespace DracoProtos.Core.Base
{
    public abstract class FResistModifyDetailsBase : FObject
    {
        public int matchingCreatures;
        public float resultResistMax;
        public float resultResistMin;

        public void ReadExternal(FInputStream stream)
        {
            this.matchingCreatures = stream.ReadInt32();
            this.resultResistMax = stream.ReadFloat();
            this.resultResistMin = stream.ReadFloat();
        }

        public void WriteExternal(FOutputStream stream)
        {
            stream.WriteInt32(this.matchingCreatures);
            stream.WriteFloat(this.resultResistMax);
            stream.WriteFloat(this.resultResistMin);
        }
    }
}
