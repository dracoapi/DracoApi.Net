using System.Collections.Generic;
using DracoProtos.Core.Enums;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FAvaUpdateBase : FBaseItemUpdate
	{
        public double activationRadius;
        public AllianceType? alliance;
        public GeoCoords altarCoords;
        public float artifactDustFactor;
        public List<ArtifactName> artifacts;
        public FBuddy buddy;
        public List<FBuff> buffs;
        public Dictionary<CreatureType, int> candies;
        public int coins;
        public int creatureStorageSize;
        public int currentExperience;
        public string dungeonId;
        public int dust;
        public int eggsHatchedCount;
        public bool emulatorCheckDisabled;
        public float exp;
        public long freshNewsDate;
        public bool hasUnhandledAdvices;
        public long incenseDuration;
        public string incenseGenMonstersGroupName;
        public long incenseLeftTime;
        public bool isArtifactsBagFull;
        public bool isBagFull;
        public bool isEggBagFull;
        public int level;
        public int monstersCaughtCount;
        public int nextLevelExperience;
        public Dictionary<RecipeType, int> recipeLevels;
        public long registerDate;
        public long stopFieldDuration;
        public long stopFieldLeftTime;
        public GeoCoords superVisionCenter;
        public long superVisionDuration;
        public long superVisionLeftTime;
        public float totalDistanceF;
        public List<ArtifactName> wearArtifacts;

		public override void ReadExternal(FInputStream stream)
		{
			base.ReadExternal(stream);
            this.activationRadius = stream.ReadDouble();
            this.alliance = (AllianceType?)stream.ReadDynamicObject();
            this.altarCoords = (GeoCoords)stream.ReadDynamicObject();
            this.artifactDustFactor = stream.ReadFloat();
            this.artifacts = stream.ReadStaticList<ArtifactName>(true);
            this.buddy = (FBuddy)stream.ReadDynamicObject();
            this.buffs = stream.ReadStaticList<FBuff>(true);
            this.candies = stream.ReadStaticMap<CreatureType, int>(true, true);
            this.coins = stream.ReadInt32();
            this.creatureStorageSize = stream.ReadInt32();
            this.currentExperience = stream.ReadInt32();
            this.dungeonId = (string)stream.ReadDynamicObject();
            this.dust = stream.ReadInt32();
            this.eggsHatchedCount = stream.ReadInt32();
            this.emulatorCheckDisabled = stream.ReadBoolean();
            this.exp = stream.ReadFloat();
            this.freshNewsDate = stream.ReadInt64();
            this.hasUnhandledAdvices = stream.ReadBoolean();
            this.incenseDuration = stream.ReadInt64();
            this.incenseGenMonstersGroupName = stream.ReadUtfString();
            this.incenseLeftTime = stream.ReadInt64();
            this.isArtifactsBagFull = stream.ReadBoolean();
            this.isBagFull = stream.ReadBoolean();
            this.isEggBagFull = stream.ReadBoolean();
            this.level = stream.ReadInt32();
            this.monstersCaughtCount = stream.ReadInt32();
            this.nextLevelExperience = stream.ReadInt32();
            this.recipeLevels = stream.ReadStaticMap<RecipeType, int>(true, true);
            this.registerDate = stream.ReadInt64();
            this.stopFieldDuration = stream.ReadInt64();
            this.stopFieldLeftTime = stream.ReadInt64();
            this.superVisionCenter = (GeoCoords)stream.ReadDynamicObject();
            this.superVisionDuration = stream.ReadInt64();
            this.superVisionLeftTime = stream.ReadInt64();
            this.totalDistanceF = stream.ReadFloat();
            this.wearArtifacts = stream.ReadStaticList<ArtifactName>(true);
        }

        public override void WriteExternal(FOutputStream stream)
        {
            base.WriteExternal(stream);
            stream.WriteDouble(this.activationRadius);
            stream.WriteDynamicObject(this.alliance);
            stream.WriteDynamicObject(this.altarCoords);
            stream.WriteFloat(this.artifactDustFactor);
            stream.WriteStaticCollection(this.artifacts, true);
            stream.WriteDynamicObject(this.buddy);
            stream.WriteStaticCollection(this.buffs, true);
            stream.WriteStaticMap(this.candies, true, true);
            stream.WriteInt32(this.coins);
            stream.WriteInt32(this.creatureStorageSize);
            stream.WriteInt32(this.currentExperience);
            stream.WriteDynamicObject(this.dungeonId);
            stream.WriteInt32(this.dust);
            stream.WriteInt32(this.eggsHatchedCount);
            stream.WriteBoolean(this.emulatorCheckDisabled);
            stream.WriteFloat(this.exp);
            stream.WriteInt64(this.freshNewsDate);
            stream.WriteBoolean(this.hasUnhandledAdvices);
            stream.WriteInt64(this.incenseDuration);
            stream.WriteUtfString(this.incenseGenMonstersGroupName);
            stream.WriteInt64(this.incenseLeftTime);
            stream.WriteBoolean(this.isArtifactsBagFull);
            stream.WriteBoolean(this.isBagFull);
            stream.WriteBoolean(this.isEggBagFull);
            stream.WriteInt32(this.level);
            stream.WriteInt32(this.monstersCaughtCount);
            stream.WriteInt32(this.nextLevelExperience);
            stream.WriteStaticMap(this.recipeLevels, true, true);
            stream.WriteInt64(this.registerDate);
            stream.WriteInt64(this.stopFieldDuration);
            stream.WriteInt64(this.stopFieldLeftTime);
            stream.WriteDynamicObject(this.superVisionCenter);
            stream.WriteInt64(this.superVisionDuration);
            stream.WriteInt64(this.superVisionLeftTime);
            stream.WriteFloat(this.totalDistanceF);
            stream.WriteStaticCollection(this.wearArtifacts, true);
        }
    }
}
