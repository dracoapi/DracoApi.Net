using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FAuthDataBase : FObject
	{
        public FUserInfo info;

        public void ReadExternal(FInputStream stream)
		{
			this.info = (FUserInfo)stream.ReadStaticObject(typeof(FUserInfo));
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteStaticObject(this.info);
		}
	}
}
