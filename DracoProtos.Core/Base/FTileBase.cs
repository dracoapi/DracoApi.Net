using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FTileBase : FObject
	{
        public string dungeonId;
        public Tile tile;

        public void ReadExternal(FInputStream stream)
		{
			this.dungeonId = (string)stream.ReadDynamicObject();
			this.tile = (Tile)stream.ReadStaticObject(typeof(Tile));
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteDynamicObject(this.dungeonId);
			stream.WriteStaticObject(this.tile);
		}
	}
}
