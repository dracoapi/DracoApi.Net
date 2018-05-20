using DracoProtos.Core.Enums;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FAllianceChooseRequestBase : FBaseItemUpdate
	{
        public int bonus;
        public bool oneOption;
        public AllianceType? recommendedType;

        public override void ReadExternal(FInputStream stream)
		{
			base.ReadExternal(stream);
			this.bonus = stream.ReadInt32();
			this.oneOption = stream.ReadBoolean();
			this.recommendedType = (AllianceType?)stream.ReadDynamicObject();
		}

		public override void WriteExternal(FOutputStream stream)
		{
			base.WriteExternal(stream);
			stream.WriteInt32(this.bonus);
			stream.WriteBoolean(this.oneOption);
			stream.WriteDynamicObject(this.recommendedType);
		}
	}
}
