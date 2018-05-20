using DracoProtos.Core.Enums;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;
using System;
using System.Collections.Generic;
using System.Text;

namespace DracoProtos.Core.Base
{
    public abstract class FContestUpdateBase : FObject
    {
        public List<FContestBattle> battles;
        public bool canStart;
        public string contestId;
        public bool hideContestScreen;
        public string ownerNickname;
        public float participantTtl;
        public List<string> participants;
        public string pendingBattle;
        public bool showContestScreen;
        public ContestStage stage;
        public List<FContestStats> stats;
        public int totalBattlesCount;
        public int userBattlesCount;

        public void ReadExternal(FInputStream stream)
        {
            this.battles = stream.ReadStaticList<FContestBattle>(true);
            this.canStart = stream.ReadBoolean();
            this.contestId = stream.ReadUtfString();
            this.hideContestScreen = stream.ReadBoolean();
            this.ownerNickname = stream.ReadUtfString();
            this.participantTtl = stream.ReadFloat();
            this.participants = stream.ReadStaticList<string>(true);
            this.pendingBattle = (string)stream.ReadDynamicObject();
            this.showContestScreen = stream.ReadBoolean();
            this.stage = (ContestStage)stream.ReadDynamicObject();
            this.stats = stream.ReadStaticList<FContestStats>(true);
            this.totalBattlesCount = stream.ReadInt32();
            this.userBattlesCount = stream.ReadInt32();
        }

        public void WriteExternal(FOutputStream stream)
        {
            stream.WriteStaticCollection(this.battles, true);
            stream.WriteBoolean(this.canStart);
            stream.WriteUtfString(this.contestId);
            stream.WriteBoolean(this.hideContestScreen);
            stream.WriteUtfString(this.ownerNickname);
            stream.WriteFloat(this.participantTtl);
            stream.WriteStaticCollection(this.participants, true);
            stream.WriteDynamicObject(this.pendingBattle);
            stream.WriteBoolean(this.showContestScreen);
            stream.WriteDynamicObject(this.stage);
            stream.WriteStaticCollection(this.stats, true);
            stream.WriteInt32(this.totalBattlesCount);
            stream.WriteInt32(this.userBattlesCount);
        }
    }
}
