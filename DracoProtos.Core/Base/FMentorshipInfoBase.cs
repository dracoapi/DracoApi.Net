using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;
using System;
using System.Collections.Generic;
using System.Text;

namespace DracoProtos.Core.Base
{
    public abstract class FMentorshipInfoBase : FObject
    {
        public bool canBeMentor;
        public bool canChooseMentor;
        public bool hasParentMentor;
        public int levelRequiredToBeMentor;
        public string ownMentorCode;
        public string parentMentorId;
        public string parentMentorNickname;
        public List<FStudent> students;
        public bool wasKickedByParentMentor;

        public void ReadExternal(FInputStream stream)
        {
            this.canBeMentor = stream.ReadBoolean();
            this.canChooseMentor = stream.ReadBoolean();
            this.hasParentMentor = stream.ReadBoolean();
            this.levelRequiredToBeMentor = stream.ReadInt32();
            this.ownMentorCode = stream.ReadUtfString();
            this.parentMentorId = stream.ReadUtfString();
            this.parentMentorNickname = stream.ReadUtfString();
            this.students = stream.ReadStaticList<FStudent>(true);
            this.wasKickedByParentMentor = stream.ReadBoolean();
        }

        public void WriteExternal(FOutputStream stream)
        {
            stream.WriteBoolean(this.canBeMentor);
            stream.WriteBoolean(this.canChooseMentor);
            stream.WriteBoolean(this.hasParentMentor);
            stream.WriteInt32(this.levelRequiredToBeMentor);
            stream.WriteUtfString(this.ownMentorCode);
            stream.WriteUtfString(this.parentMentorId);
            stream.WriteUtfString(this.parentMentorNickname);
            stream.WriteStaticCollection(this.students, true);
            stream.WriteBoolean(this.wasKickedByParentMentor);
        }
    }
}
