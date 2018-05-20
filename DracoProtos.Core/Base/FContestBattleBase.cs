using DracoProtos.Core.Serializer;
using System;
using System.Collections.Generic;
using System.Text;

namespace DracoProtos.Core.Base
{
    public abstract class FContestBattleBase : FObject
    {
        public float hpPercent;
        public string nickname;
        public string nicknameOpponent;
        public bool victory;

        public void ReadExternal(FInputStream stream)
        {
            this.hpPercent = stream.ReadFloat();
            this.nickname = stream.ReadUtfString();
            this.nicknameOpponent = stream.ReadUtfString();
            this.victory = stream.ReadBoolean();
        }

        public void WriteExternal(FOutputStream stream)
        {
            stream.WriteFloat(this.hpPercent);
            stream.WriteUtfString(this.nickname);
            stream.WriteUtfString(this.nicknameOpponent);
            stream.WriteBoolean(this.victory);
        }
    }
}
