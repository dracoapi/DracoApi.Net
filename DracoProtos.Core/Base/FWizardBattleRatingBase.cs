using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;
using System;
using System.Collections.Generic;
using System.Text;

namespace DracoProtos.Core.Base
{
    public abstract class FWizardBattleRatingBase : FObject
    {
        public List<FWizardBattleRatingListRecord> topRecords;

        public void ReadExternal(FInputStream stream)
        {
            this.topRecords = stream.ReadStaticList<FWizardBattleRatingListRecord>(true);
        }

        public void WriteExternal(FOutputStream stream)
        {
            stream.WriteStaticCollection(this.topRecords, true);
        }
    }
}
