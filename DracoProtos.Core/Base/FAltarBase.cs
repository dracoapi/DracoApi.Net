using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FAltarBase : FObject
	{
        public string ownerId;
        public bool sharedWithEmptyPassword;

        public void ReadExternal(FInputStream stream)
		{
			this.ownerId = stream.ReadUtfString();
			this.sharedWithEmptyPassword = stream.ReadBoolean();
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteUtfString(this.ownerId);
			stream.WriteBoolean(this.sharedWithEmptyPassword);
		}
	}
}
