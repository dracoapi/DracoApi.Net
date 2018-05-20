using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class GeoCoordsBase : FObject
	{
        public double latitude;
        public double longitude;

        public void ReadExternal(FInputStream stream)
		{
			this.latitude = stream.ReadDouble();
			this.longitude = stream.ReadDouble();
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteDouble(this.latitude);
			stream.WriteDouble(this.longitude);
		}
	}
}
