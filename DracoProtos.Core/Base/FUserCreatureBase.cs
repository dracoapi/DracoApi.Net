using System.Collections.Generic;
using DracoProtos.Core.Enums;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FUserCreatureBase : FObject
	{
        public string alias;
        public int attackValue;
        public int baseCp;
        public CreatureType candyType;
        public int chargedSegments;
        public string chargedSkill;
        public float chargedSkillDps;
        public int cp;
        public int cReadexIndex;
        public float dps;
        public ElementType elementType;
        public long gotchaTime;
        public int group;
        public bool hasMaxResist;
        public float hp;
        public string id;
        public bool improvable;
        public int improveCandiesCost;
        public int improveDustCost;
        public bool isArenaDefender;
        public bool isAttacker;
        public bool isLibraryDefender;
        public int level;
        public string mainSkill;
        public float mainSkillDps;
        public float mainSkillEps;
        public CreatureType name;
        public bool permanent;
        public Dictionary<CreatureType, int> possibleEvolutions;
        public float resist;
        public ElementType resistFor;
        public int staminaValue;
        public int tier;
        public float totalHp;

        public void ReadExternal(FInputStream stream)
		{
			this.alias = (string)stream.ReadDynamicObject();
			this.attackValue = stream.ReadInt32();
			this.baseCp = stream.ReadInt32();
			this.candyType = (CreatureType)stream.ReadEnum(typeof(CreatureType));
			this.chargedSegments = stream.ReadInt32();
			this.chargedSkill = stream.ReadUtfString();
			this.chargedSkillDps = stream.ReadFloat();
			this.cp = stream.ReadInt32();
            this.cReadexIndex = stream.ReadInt32();
			this.dps = stream.ReadFloat();
			this.elementType = (ElementType)stream.ReadEnum(typeof(ElementType));
			this.gotchaTime = stream.ReadInt64();
			this.group = stream.ReadInt32();
            this.hasMaxResist = stream.ReadBoolean();
            this.hp = stream.ReadFloat();
			this.id = stream.ReadUtfString();
			this.improvable = stream.ReadBoolean();
			this.improveCandiesCost = stream.ReadInt32();
			this.improveDustCost = stream.ReadInt32();
			this.isArenaDefender = stream.ReadBoolean();
			this.isAttacker = stream.ReadBoolean();
			this.isLibraryDefender = stream.ReadBoolean();
			this.level = stream.ReadInt32();
			this.mainSkill = stream.ReadUtfString();
			this.mainSkillDps = stream.ReadFloat();
			this.mainSkillEps = stream.ReadFloat();
			this.name = (CreatureType)stream.ReadEnum(typeof(CreatureType));
            this.permanent = stream.ReadBoolean();
            this.possibleEvolutions = stream.ReadStaticMap<CreatureType, int>(true, true);
            this.resist = stream.ReadFloat();
            this.resistFor = (ElementType)stream.ReadDynamicObject();
            this.tier = stream.ReadInt32();
			this.staminaValue = stream.ReadInt32();
			this.totalHp = stream.ReadFloat();
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteDynamicObject(this.alias);
			stream.WriteInt32(this.attackValue);
			stream.WriteInt32(this.baseCp);
			stream.WriteEnum(this.candyType);
			stream.WriteInt32(this.chargedSegments);
			stream.WriteUtfString(this.chargedSkill);
			stream.WriteFloat(this.chargedSkillDps);
			stream.WriteInt32(this.cp);
            stream.WriteInt32(this.cReadexIndex);
			stream.WriteFloat(this.dps);
			stream.WriteEnum(this.elementType);
			stream.WriteInt64(this.gotchaTime);
			stream.WriteInt32(this.group);
            stream.WriteBoolean(this.hasMaxResist);
			stream.WriteFloat(this.hp);
			stream.WriteUtfString(this.id);
			stream.WriteBoolean(this.improvable);
			stream.WriteInt32(this.improveCandiesCost);
			stream.WriteInt32(this.improveDustCost);
			stream.WriteBoolean(this.isArenaDefender);
			stream.WriteBoolean(this.isAttacker);
			stream.WriteBoolean(this.isLibraryDefender);
			stream.WriteInt32(this.level);
			stream.WriteUtfString(this.mainSkill);
			stream.WriteFloat(this.mainSkillDps);
			stream.WriteFloat(this.mainSkillEps);
			stream.WriteEnum(this.name);
            stream.WriteBoolean(this.permanent);
            stream.WriteStaticMap(this.possibleEvolutions, true, true);
            stream.WriteFloat(this.resist);
            stream.WriteDynamicObject(this.resistFor);
			stream.WriteInt32(this.tier);
			stream.WriteInt32(this.staminaValue);
			stream.WriteFloat(this.totalHp);
		}
	}
}
