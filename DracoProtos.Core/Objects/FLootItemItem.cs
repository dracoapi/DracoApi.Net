using DracoProtos.Core.Base;

namespace DracoProtos.Core.Objects
{
    public class FLootItemItem : FLootItemItemBase
	{
		public override string GetLootGroup()
		{
			return string.Concat(new object[]
			{
				base.GetLootGroup(),
				"/",
				this.item,
				"/",
				this.isStreak
			});
		}
	}
}
