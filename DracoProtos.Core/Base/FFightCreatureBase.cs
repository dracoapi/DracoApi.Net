using DracoProtos.Core.Enums;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FFightCreatureBase : FObject
	{
        public string alias;
        public bool attacker;
        public int baseCp;
        public string chargedSkill;
        public bool chargedSkillAim;
        public int chargedSkillAngle;
        public float chargedSkillDps;
        public SkillQuality chargedSkillQuality;
        public float chargedSkillSpeed;
        public float chargedSkillTtl;
        public int cp;
        public ElementType decreasedDmgTo;
        public float distance;
        public float dodgeAngle;
        public float dodgeDamageRatio;
        public float dodgeMoveTime;
        public int energySegments;
        public float height;
        public float holdTimeForChargedSkill;
        public float hp;
        public string id;
        public float incomingEnergyOnAttack;
        public ElementType increasedDmgTo;
        public string mainSkill;
        public bool mainSkillAim;
        public int mainSkillAngle;
        public float mainSkillDps;
        public SkillQuality mainSkillQuality;
        public float mainSkillSpeed;
        public float mainSkillTtl;
        public float maxEnergy;
        public CreatureType name;
        public float resistCoef;
        public ElementType resistFor;
        public float rightElementAttackCoef;
        public float scale;
        public float specAttackCoef;
        public float startCamPosDistance;
        public float startCamPosHeight;
        public float totalHp;
        public ElementType type;
        public float wrongElementAttackCoef;

        public void ReadExternal(FInputStream stream)
		{
			this.alias = (string)stream.ReadDynamicObject();
			this.attacker = stream.ReadBoolean();
			this.baseCp = stream.ReadInt32();
			this.chargedSkill = stream.ReadUtfString();
			this.chargedSkillAim = stream.ReadBoolean();
			this.chargedSkillAngle = stream.ReadInt32();
			this.chargedSkillDps = stream.ReadFloat();
			this.chargedSkillQuality = (SkillQuality)stream.ReadEnum(typeof(SkillQuality));
			this.chargedSkillSpeed = stream.ReadFloat();
			this.chargedSkillTtl = stream.ReadFloat();
			this.cp = stream.ReadInt32();
			this.decreasedDmgTo = (ElementType)stream.ReadEnum(typeof(ElementType));
			this.distance = stream.ReadFloat();
			this.dodgeAngle = stream.ReadFloat();
			this.dodgeDamageRatio = stream.ReadFloat();
			this.dodgeMoveTime = stream.ReadFloat();
			this.energySegments = stream.ReadInt32();
			this.height = stream.ReadFloat();
			this.holdTimeForChargedSkill = stream.ReadFloat();
			this.hp = stream.ReadFloat();
			this.id = stream.ReadUtfString();
			this.incomingEnergyOnAttack = stream.ReadFloat();
			this.increasedDmgTo = (ElementType)stream.ReadEnum(typeof(ElementType));
			this.mainSkill = stream.ReadUtfString();
			this.mainSkillAim = stream.ReadBoolean();
			this.mainSkillAngle = stream.ReadInt32();
			this.mainSkillDps = stream.ReadFloat();
			this.mainSkillQuality = (SkillQuality)stream.ReadEnum(typeof(SkillQuality));
			this.mainSkillSpeed = stream.ReadFloat();
			this.mainSkillTtl = stream.ReadFloat();
			this.maxEnergy = stream.ReadFloat();
			this.name = (CreatureType)stream.ReadEnum(typeof(CreatureType));
            this.resistCoef = stream.ReadFloat();
            this.resistFor = (ElementType)stream.ReadEnum(typeof(ElementType));
            this.rightElementAttackCoef = stream.ReadFloat();
			this.scale = stream.ReadFloat();
			this.specAttackCoef = stream.ReadFloat();
			this.startCamPosDistance = stream.ReadFloat();
			this.startCamPosHeight = stream.ReadFloat();
			this.totalHp = stream.ReadFloat();
			this.type = (ElementType)stream.ReadEnum(typeof(ElementType));
			this.wrongElementAttackCoef = stream.ReadFloat();
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteDynamicObject(this.alias);
			stream.WriteBoolean(this.attacker);
			stream.WriteInt32(this.baseCp);
			stream.WriteUtfString(this.chargedSkill);
			stream.WriteBoolean(this.chargedSkillAim);
			stream.WriteInt32(this.chargedSkillAngle);
			stream.WriteFloat(this.chargedSkillDps);
			stream.WriteEnum(this.chargedSkillQuality);
			stream.WriteFloat(this.chargedSkillSpeed);
			stream.WriteFloat(this.chargedSkillTtl);
			stream.WriteInt32(this.cp);
			stream.WriteEnum(this.decreasedDmgTo);
			stream.WriteFloat(this.distance);
			stream.WriteFloat(this.dodgeAngle);
			stream.WriteFloat(this.dodgeDamageRatio);
			stream.WriteFloat(this.dodgeMoveTime);
			stream.WriteInt32(this.energySegments);
			stream.WriteFloat(this.height);
			stream.WriteFloat(this.holdTimeForChargedSkill);
			stream.WriteFloat(this.hp);
			stream.WriteUtfString(this.id);
			stream.WriteFloat(this.incomingEnergyOnAttack);
			stream.WriteEnum(this.increasedDmgTo);
			stream.WriteUtfString(this.mainSkill);
			stream.WriteBoolean(this.mainSkillAim);
			stream.WriteInt32(this.mainSkillAngle);
			stream.WriteFloat(this.mainSkillDps);
			stream.WriteEnum(this.mainSkillQuality);
			stream.WriteFloat(this.mainSkillSpeed);
			stream.WriteFloat(this.mainSkillTtl);
			stream.WriteFloat(this.maxEnergy);
			stream.WriteEnum(this.name);
            stream.WriteFloat(this.resistCoef);
            stream.WriteEnum(this.resistFor);
			stream.WriteFloat(this.rightElementAttackCoef);
			stream.WriteFloat(this.scale);
			stream.WriteFloat(this.specAttackCoef);
			stream.WriteFloat(this.startCamPosDistance);
			stream.WriteFloat(this.startCamPosHeight);
			stream.WriteFloat(this.totalHp);
			stream.WriteEnum(this.type);
			stream.WriteFloat(this.wrongElementAttackCoef);
		}
	}
}
