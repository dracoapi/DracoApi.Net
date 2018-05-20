using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FBaseItemUpdateBase : FObject
	{
		public virtual void ReadExternal(FInputStream stream)
		{
		}

		public virtual void WriteExternal(FOutputStream stream)
		{
		}
	}
}
