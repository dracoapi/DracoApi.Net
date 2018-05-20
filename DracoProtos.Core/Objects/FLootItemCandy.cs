using DracoProtos.Core.Base;

namespace DracoProtos.Core.Objects
{
    public class FLootItemCandy : FLootItemCandyBase
	{
		public override string GetLootGroup()
		{
			return base.GetLootGroup() + "/" + this.candyType;
		}
	}
}
