using DracoProtos.Core.Base;

namespace DracoProtos.Core.Objects
{
    public class FAvaUpdate : FAvaUpdateBase
	{
		public override void Handle()
		{
		}

		public bool InDungeon
		{
			get
			{
				return this.dungeonId != null;
			}
		}
	}
}
