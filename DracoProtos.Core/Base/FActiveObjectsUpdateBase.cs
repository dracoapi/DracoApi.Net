using System.Collections.Generic;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FActiveObjectsUpdateBase : FObject
	{
        public int arenaQuantity;
        public int coins;
        public int dust;
        public bool increasedTribute;
        public int libraryAvailableCharges;
        public int libraryPoints;
        public int libraryQuantity;
        public int libraryRequired;
        public int libraryTotalCharges;
        public float libraryTotalCooldown;
        public int libraryWaitCooldown;
        public FLoot loot;
        public int maxArenas;
        public List<FActiveObject> objectList;
        public float timeToNextTributeCollection;
        public int totalArenas;
        public float tributeCooldown;

        public void ReadExternal(FInputStream stream)
		{
			this.arenaQuantity = stream.ReadInt32();
			this.coins = stream.ReadInt32();
			this.dust = stream.ReadInt32();
            this.increasedTribute = stream.ReadBoolean();
            this.libraryAvailableCharges = stream.ReadInt32();
            this.libraryPoints = stream.ReadInt32();
			this.libraryQuantity = stream.ReadInt32();
			this.libraryRequired = stream.ReadInt32();
            this.libraryTotalCharges = stream.ReadInt32();
            this.libraryTotalCooldown = stream.ReadFloat();
			this.libraryWaitCooldown = stream.ReadInt32();
			this.loot = (FLoot)stream.ReadStaticObject(typeof(FLoot));
            this.maxArenas = stream.ReadInt32();
			this.objectList = stream.ReadStaticList<FActiveObject>(true);
			this.timeToNextTributeCollection = stream.ReadFloat();
            this.totalArenas = stream.ReadInt32();
			this.tributeCooldown = stream.ReadFloat();
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteInt32(this.arenaQuantity);
			stream.WriteInt32(this.coins);
			stream.WriteInt32(this.dust);
            stream.WriteBoolean(this.increasedTribute);
            stream.WriteInt32(this.libraryAvailableCharges);
			stream.WriteInt32(this.libraryPoints);
			stream.WriteInt32(this.libraryQuantity);
			stream.WriteInt32(this.libraryRequired);
            stream.WriteInt32(this.libraryTotalCharges);
			stream.WriteFloat(this.libraryTotalCooldown);
			stream.WriteInt32(this.libraryWaitCooldown);
			stream.WriteStaticObject(this.loot);
            stream.WriteInt32(this.maxArenas);
			stream.WriteStaticCollection(this.objectList, true);
			stream.WriteFloat(this.timeToNextTributeCollection);
            stream.WriteInt32(this.totalArenas);
			stream.WriteFloat(this.tributeCooldown);
		}
	}
}
