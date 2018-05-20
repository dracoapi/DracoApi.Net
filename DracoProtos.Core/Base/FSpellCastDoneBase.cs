using DracoProtos.Core.Enums;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FSpellCastDoneBase : FBaseItemUpdate
	{
        public GeoCoords altarCoords;
        public SpellType spellType;

        public override void ReadExternal(FInputStream stream)
		{
			base.ReadExternal(stream);
			this.altarCoords = (GeoCoords)stream.ReadStaticObject(typeof(GeoCoords));
			this.spellType = (SpellType)stream.ReadEnum(typeof(SpellType));
		}

		public override void WriteExternal(FOutputStream stream)
		{
			base.WriteExternal(stream);
			stream.WriteStaticObject(this.altarCoords);
			stream.WriteEnum(this.spellType);
		}
	}
}
