using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FEncounterUpdateBase : FBaseItemUpdate
    {
        public FFightCreature attacker;
        public FFightCreature defender;

        public override void ReadExternal(FInputStream stream)
		{
			base.ReadExternal(stream);
			this.attacker = (FFightCreature)stream.ReadStaticObject(typeof(FFightCreature));
			this.defender = (FFightCreature)stream.ReadStaticObject(typeof(FFightCreature));
		}

		public override void WriteExternal(FOutputStream stream)
		{
			base.WriteExternal(stream);
			stream.WriteStaticObject(this.attacker);
			stream.WriteStaticObject(this.defender);
		}
	}
}
