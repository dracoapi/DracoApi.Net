using DracoProtos.Core.Enums;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FDungeonUpdateBase : FBaseItemUpdate
	{
        public GeoCoords coords;
        public float rotation;
        public int size;
        public DungeonShapeType type;

        public override void ReadExternal(FInputStream stream)
		{
			base.ReadExternal(stream);
			this.coords = (GeoCoords)stream.ReadStaticObject(typeof(GeoCoords));
			this.rotation = stream.ReadFloat();
			this.size = stream.ReadInt32();
			this.type = (DungeonShapeType)stream.ReadEnum(typeof(DungeonShapeType));
		}

		public override void WriteExternal(FOutputStream stream)
		{
			base.WriteExternal(stream);
			stream.WriteStaticObject(this.coords);
			stream.WriteFloat(this.rotation);
			stream.WriteInt32(this.size);
			stream.WriteEnum(this.type);
		}
	}
}
