using DracoProtos.Core.Enums;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FCreadexChainBase : FObject
	{
        public bool caught;
        public CreatureType creature;
        public bool seen;

        public void ReadExternal(FInputStream stream)
		{
			this.caught = stream.ReadBoolean();
			this.creature = (CreatureType)stream.ReadEnum(typeof(CreatureType));
			this.seen = stream.ReadBoolean();
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteBoolean(this.caught);
			stream.WriteEnum(this.creature);
			stream.WriteBoolean(this.seen);
		}
	}
}
