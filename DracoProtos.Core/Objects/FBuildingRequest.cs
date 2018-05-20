using DracoProtos.Core.Base;

namespace DracoProtos.Core.Objects
{
    public class FBuildingRequest : FBuildingRequestBase
	{
        public FBuildingRequest()
        {

        }

		public FBuildingRequest(string id, GeoCoords coords, string dungeonId)
		{
			this.id = id;
			this.coords = coords;
			this.dungeonId = dungeonId;
		}
	}
}
