using DracoProtos.Core.Serializer;
using System;
using System.Collections.Generic;
using System.Text;

namespace DracoProtos.Core.Base
{
    public abstract class FCollectorRatingListRecordBase : FObject
    {
        public int level;
        public string nickName;
        public int place;
        public float score;
        public int openCreaturesCount;
        public int topQualityCreaturesCount;
        public int topQualityPoweredupCreaturesCount;

        public void ReadExternal(FInputStream stream)
        {
            this.level = stream.ReadInt32();
            this.nickName = stream.ReadUtfString();
            this.place = stream.ReadInt32();
            this.score = stream.ReadFloat();
            this.openCreaturesCount = stream.ReadInt32();
            this.topQualityCreaturesCount = stream.ReadInt32();
            this.topQualityPoweredupCreaturesCount = stream.ReadInt32();
        }

        public void WriteExternal(FOutputStream stream)
        {
            stream.WriteInt32(this.level);
            stream.WriteUtfString(this.nickName);
            stream.WriteInt32(this.place);
            stream.WriteFloat(this.score);
            stream.WriteInt32(this.openCreaturesCount);
            stream.WriteInt32(this.topQualityCreaturesCount);
            stream.WriteInt32(this.topQualityPoweredupCreaturesCount);
        }
    }
}
