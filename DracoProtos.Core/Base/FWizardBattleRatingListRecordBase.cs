using DracoProtos.Core.Serializer;
using System;
using System.Collections.Generic;
using System.Text;

namespace DracoProtos.Core.Base
{

    public abstract class FWizardBattleRatingListRecordBase : FObject
    {
        public int level;
        public string nickName;
        public int place;
        public float score;
        public int battleCount;
        public float savedHealthRate;
        public int winCount;

        public void ReadExternal(FInputStream stream)
        {
            this.level = stream.ReadInt32();
            this.nickName = stream.ReadUtfString();
            this.place = stream.ReadInt32();
            this.score = stream.ReadFloat();
            this.battleCount = stream.ReadInt32();
            this.savedHealthRate = stream.ReadFloat();
            this.winCount = stream.ReadInt32();
        }

        public void WriteExternal(FOutputStream stream)
        {
            stream.WriteInt32(this.level);
            stream.WriteUtfString(this.nickName);
            stream.WriteInt32(this.place);
            stream.WriteFloat(this.score);
            stream.WriteInt32(this.battleCount);
            stream.WriteFloat(this.savedHealthRate);
            stream.WriteInt32(this.winCount);
        }
    }
 }
