using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;
using System.Collections.Generic;

namespace DracoProtos.Core.Base
{
    public abstract class FCollectorRatingBase : FObject
    {
        public List<FCollectorRatingListRecord> topRecords;

        public void ReadExternal(FInputStream stream)
        {
            this.topRecords = stream.ReadStaticList<FCollectorRatingListRecord>(true);
        }

        public void WriteExternal(FOutputStream stream)
        {
            stream.WriteStaticCollection(this.topRecords, true);
        }
    }
}
