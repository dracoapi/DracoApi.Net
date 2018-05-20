using DracoProtos.Core.Base;

namespace DracoProtos.Core.Objects
{
    public class FLootItemDust : FLootItemDustBase
	{

		public override string GetLootGroup()
		{
			return base.GetLootGroup() + "/" + this.isStreak;
		}
	}
}
