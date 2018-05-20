using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FScoutRequestBase : FObject
	{
        public GeoCoords scoutCoords;
        public FClientRequest clientRequest;

        public void ReadExternal(FInputStream stream)
		{
			this.clientRequest = (FClientRequest)stream.ReadStaticObject(typeof(FClientRequest));
			this.scoutCoords = (GeoCoords)stream.ReadStaticObject(typeof(GeoCoords));
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteStaticObject(this.clientRequest);
			stream.WriteStaticObject(this.scoutCoords);
		}
	}
}
