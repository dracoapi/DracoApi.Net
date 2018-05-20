using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FCreatureRequestBase : FObject
	{
        public string id;
        public bool veryFirst;

        public void ReadExternal(FInputStream stream)
		{
			this.id = stream.ReadUtfString();
			this.veryFirst = stream.ReadBoolean();
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteUtfString(this.id);
			stream.WriteBoolean(this.veryFirst);
		}
	}
}
