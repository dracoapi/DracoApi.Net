using DracoProtos.Core.Serializer;
using System;
using System.Collections.Generic;
using System.Text;

namespace DracoProtos.Core.Base
{
    public abstract class FContestStatsBase : FObject
    {
        public float hpPercentTotal;
        public int lostAsOpponentCount;
        public int lostCount;
        public string nickname;
        public int winAsOpponentCount;
        public int winCount;

        public void ReadExternal(FInputStream stream)
        {
            this.hpPercentTotal = stream.ReadFloat();
            this.lostAsOpponentCount = stream.ReadInt32();
            this.lostCount = stream.ReadInt32();
            this.nickname = stream.ReadUtfString();
            this.winAsOpponentCount = stream.ReadInt32();
            this.winCount = stream.ReadInt32();
        }

        public void WriteExternal(FOutputStream stream)
        {
            stream.WriteFloat(this.hpPercentTotal);
            stream.WriteInt32(this.lostAsOpponentCount);
            stream.WriteInt32(this.lostCount);
            stream.WriteUtfString(this.nickname);
            stream.WriteInt32(this.winAsOpponentCount);
            stream.WriteInt32(this.winCount);
        }
    }
}
