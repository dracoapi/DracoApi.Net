using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FBaseRatingRecordBase : FObject
    {
        public int level;
        public string nickName;
        public int place;
        public float score;

        public void ReadExternal(FInputStream stream)
        {
            this.level = stream.ReadInt32();
            this.nickName = stream.ReadUtfString();
            this.place = stream.ReadInt32();
            this.score = stream.ReadFloat();
        }

        public void WriteExternal(FOutputStream stream)
        {
            stream.WriteInt32(this.level);
            stream.WriteUtfString(this.nickName);
            stream.WriteInt32(this.place);
            stream.WriteFloat(this.score);
        }
    }
}
