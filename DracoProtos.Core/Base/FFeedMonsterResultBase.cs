using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FFeedMonsterResultBase : FObject
	{
        public long feedLiveTime;

        public void ReadExternal(FInputStream stream)
		{
			this.feedLiveTime = stream.ReadInt64();
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteInt64(this.feedLiveTime);
		}
	}
}
