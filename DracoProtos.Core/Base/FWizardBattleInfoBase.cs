using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FWizardBattleInfoBase : FObject
	{
        public int extraBattles;
        public int limit;
        public float timeToRefresh;
        public int used;
        public int userCoins;

        public void ReadExternal(FInputStream stream)
		{
			this.extraBattles = stream.ReadInt32();
			this.limit = stream.ReadInt32();
			this.timeToRefresh = stream.ReadFloat();
			this.used = stream.ReadInt32();
            this.userCoins = stream.ReadInt32();
        }

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteInt32(this.extraBattles);
			stream.WriteInt32(this.limit);
			stream.WriteFloat(this.timeToRefresh);
			stream.WriteInt32(this.used);
            stream.WriteInt32(this.userCoins);
		}
	}
}
