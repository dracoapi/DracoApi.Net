using DracoProtos.Core.Base;

namespace DracoProtos.Core.Objects
{
    public class FDailyQuest : FDailyQuestBase
	{
		public bool IsEmpty()
		{
			return this.id == null;
		}
	}
}
