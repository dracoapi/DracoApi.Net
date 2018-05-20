using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FObeliskDetailsBase : FBaseItemUpdate
	{
        public GeoCoords coords;
        public FDailyQuest dailyQuest;
        public FWeeklyQuestFragment fragment;
        public string id;
        public bool justOpened;
        public FWeeklyQuest weeklyQuest;

        public override void ReadExternal(FInputStream stream)
		{
			base.ReadExternal(stream);
			this.coords = (GeoCoords)stream.ReadStaticObject(typeof(GeoCoords));
			this.dailyQuest = (FDailyQuest)stream.ReadDynamicObject();
			this.fragment = (FWeeklyQuestFragment)stream.ReadDynamicObject();
			this.id = stream.ReadUtfString();
			this.justOpened = stream.ReadBoolean();
			this.weeklyQuest = (FWeeklyQuest)stream.ReadDynamicObject();
		}

		public override void WriteExternal(FOutputStream stream)
		{
			base.WriteExternal(stream);
			stream.WriteStaticObject(this.coords);
			stream.WriteDynamicObject(this.dailyQuest);
			stream.WriteDynamicObject(this.fragment);
			stream.WriteUtfString(this.id);
			stream.WriteBoolean(this.justOpened);
			stream.WriteDynamicObject(this.weeklyQuest);
		}
	}
}
