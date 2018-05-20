using DracoProtos.Core.Base;

namespace DracoProtos.Core.Objects
{
    public abstract class FBaseLootItem : FBaseLootItemBase
	{

		public virtual string GetLootGroup()
		{
			return base.GetType().FullName;
		}
	}
}
