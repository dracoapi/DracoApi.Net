using DracoProtos.Core.Base;

namespace DracoProtos.Core.Objects
{
    public class FLootItemBuff : FLootItemBuffBase
	{
		public override string GetLootGroup()
		{
			return string.Concat(new object[]
			{
				base.GetLootGroup(),
				"/",
				this.buff.type,
				"/",
				this.buff.valuePercent,
				"/",
				this.buff.durationMs
			});
		}
	}
}
