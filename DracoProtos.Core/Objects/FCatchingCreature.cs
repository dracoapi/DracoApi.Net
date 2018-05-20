using DracoProtos.Core.Base;

namespace DracoProtos.Core.Objects
{
    public class FCatchingCreature : FCatchingCreatureBase
	{
		public void SetNotFed()
		{
			this.feedLeftTime = 0;
			this.feedFoodType = null;
		}
	}
}
