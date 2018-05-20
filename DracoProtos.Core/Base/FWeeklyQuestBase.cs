using System.Collections.Generic;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FWeeklyQuestBase : FObject
	{
        public bool allOpen;
        public bool completed;
        public int currentFragment;
        public List<int> digFragments;
        public int extraQuestsAvailable;
        public int nextWeeklyQuestIn;
        public Dictionary<int, sbyte[]> openFragments;
        public int shovelsAvailable;
        public int sideFragmentNumber;

        public void ReadExternal(FInputStream stream)
		{
			this.allOpen = stream.ReadBoolean();
			this.completed = stream.ReadBoolean();
			this.currentFragment = stream.ReadInt32();
			this.digFragments = stream.ReadStaticList<int>(true);
            this.extraQuestsAvailable = stream.ReadInt32();
			this.nextWeeklyQuestIn = stream.ReadInt32();
			this.openFragments = stream.ReadStaticMap<int, sbyte[]>(true, true);
            this.shovelsAvailable = stream.ReadInt32();
			this.sideFragmentNumber = stream.ReadInt32();
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteBoolean(this.allOpen);
			stream.WriteBoolean(this.completed);
			stream.WriteInt32(this.currentFragment);
			stream.WriteStaticCollection(this.digFragments, true);
            stream.WriteInt32(this.extraQuestsAvailable);
			stream.WriteInt32(this.nextWeeklyQuestIn);
			stream.WriteStaticMap(this.openFragments, true, true);
            stream.WriteInt32(this.shovelsAvailable);
			stream.WriteInt32(this.sideFragmentNumber);
		}
	}
}
