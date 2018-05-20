using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class TileBase : FObject
	{
        public int zoom;
        public int x;
        public int y;

        public void ReadExternal(FInputStream stream)
		{
			this.x = stream.ReadInt32();
			this.y = stream.ReadInt32();
			this.zoom = stream.ReadInt32();
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteInt32(this.x);
			stream.WriteInt32(this.y);
			stream.WriteInt32(this.zoom);
		}
	}
}
