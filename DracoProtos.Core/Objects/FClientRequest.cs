using DracoProtos.Core.Base;
using DracoProtos.Core.Objects;
using System;

namespace DracoProtos.Core.Objects
{
    public class FClientRequest : FClientRequestBase
	{
		public FClientRequest()
		{
			this.currentUtcOffsetSeconds = (int)TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalSeconds;
		}

		public static FClientRequest CurrentLocation()
		{
			return new FClientRequest
			{
                //TODO:
                //coords = GeoCoords.UNKNOWN,
                coordsWithAccuracy = new GeoCoordsWithAccuracy { horizontalAccuracy = 0, latitude = 0.0, longitude = 0.0 }
            };
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"FClientRequest{time=",
				this.time,
				", latitude=",
				this.coordsWithAccuracy.latitude,
				", longitude=",
				this.coordsWithAccuracy.longitude,
				", horizontalAccuracy=",
				this.coordsWithAccuracy.horizontalAccuracy,
				'}'
			});
		}
	}
}
