using DracoProtos.Core.Base;

namespace DracoProtos.Core.Objects
{
    public class FLootItemInstantUseItem : FLootItemInstantUseItemBase
	{
		public override string GetLootGroup()
		{
			return base.GetLootGroup() + "/" + this.item;
		}
	}
}
