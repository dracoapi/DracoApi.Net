using System;

namespace DracoProtos.Core.Extensions
{
    [Serializable]
	public class Coordinates
	{
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double altitude { get; set; }
        public double horizontalAccuracy { get; set; }
        
		public Coordinates(double latitude, double longitude, double altitude) : this(latitude, longitude, altitude, false)
		{
		}

		public Coordinates(double latitude, double longitude, double altitude, bool withOutTime)
		{
			this.latitude = latitude;
			this.longitude = longitude;
			this.altitude = altitude;

		}
        /*
		public Coordinates(double latitude, double longitude, double altitude, double horizontalAccuracy, double timestampLastUpdate)
		{
			this.latitude = latitude;
			this.longitude = longitude;
			this.altitude = altitude;
			this.horizontalAccuracy = new double?(horizontalAccuracy);
			this.timestampLastUpdate = timestampLastUpdate;
		}

		public Coordinates(LocationInfo location)
		{
			this.latitude = (double)location.latitude;
			this.longitude = (double)location.longitude;
			this.altitude = (double)location.altitude;
			this.timestampLastUpdate = Coordinates.getCurrentTimestamp();
			this.horizontalAccuracy = new double?((double)location.horizontalAccuracy);
			this.verticalAccuracy = new double?((double)location.verticalAccuracy);
		}

        public Coordinates(Vector2 tileCoords, int zoom)
		{
            Vector2 vector = this.TileToWorldPos((double)tileCoords.x + 0.5, (double)tileCoords.y + 0.5, zoom);
			this.latitude = (double)vector.y;
			this.longitude = (double)vector.x;
			this.altitude = 0.0;
			this.timestampLastUpdate = Coordinates.getCurrentTimestamp();
		}

		public Coordinates(string s)
		{
			Regex.Replace(s, "\\t\\n\\r ", string.Empty);
			string[] array = s.Split(new string[]
			{
				","
			}, StringSplitOptions.None);
			double.TryParse(array[0], out this.latitude);
			double.TryParse(array[1], out this.longitude);
			this.altitude = 0.0;
			this.timestampLastUpdate = Coordinates.getCurrentTimestamp();
		}
		public void copyLocation(Coordinates location)
		{
			this.latitude = location.latitude;
			this.longitude = location.longitude;
			this.altitude = location.altitude;
			this.timestampLastUpdate = location.timestampLastUpdate;
			this.horizontalAccuracy = location.horizontalAccuracy;
			this.verticalAccuracy = location.verticalAccuracy;
		}

		public float gpsAngle(float longitude_o, float latitude_o)
		{
			if (this.longitude == (double)longitude_o)
			{
				return 0f;
			}
			if (this.longitude - (double)longitude_o < 0.0)
			{
                return (float)Math.Atan((float)(this.latitude - (double)latitude_o) / (float)(this.longitude - (double)longitude_o)) + 3.14159274f;
			}
			return (float)Math.Atan((float)(this.latitude - (double)latitude_o) / (float)(this.longitude - (double)longitude_o));
		}

		public double DistanceToInMeters(Coordinates other)
		{
			double num = 0.01745329238474369 * (other.latitude - this.latitude);
			double num2 = 0.01745329238474369 * (other.longitude - this.longitude);
			double num3 = Math.Sin(num / 2.0);
			double num4 = Math.Sin(num2 / 2.0);
			double num5 = num3 * num3 + num4 * num4 * Math.Cos(0.01745329238474369 * this.latitude) * Math.Cos(0.01745329238474369 * other.latitude);
			double num6 = 2.0 * Math.Atan2(Math.Sqrt(num5), Math.Sqrt(1.0 - num5));
			return (double)Coordinates.EarthRadius * num6;
		}

		public float DistanceFromPoint(Coordinates pt)
		{
			Vector3 vector = pt.convertCoordinateToVector();
			Vector3 vector2 = this.convertCoordinateToVector();
			return Vector3.Distance(vector, vector2);
		}

		public double DistanceFromOtherGPSCoordinate(Coordinates targetCoordinates)
		{
			double num = 3.1415926535897931 * this.latitude / 180.0;
			double num2 = 3.1415926535897931 * targetCoordinates.latitude / 180.0;
			double num3 = this.longitude - targetCoordinates.longitude;
			double d = 3.1415926535897931 * num3 / 180.0;
			double num4 = Math.Sin(num) * Math.Sin(num2) + Math.Cos(num) * Math.Cos(num2) * Math.Cos(d);
			num4 = Math.Acos(num4);
			num4 = num4 * 180.0 / 3.1415926535897931;
			num4 = num4 * 60.0 * 1.1515;
			return num4 * 1.609344 / 1000.0;
		}

		public Vector3 convertCoordinateToVector()
		{
			Vector3 result = GPSEncoder.GPSToUCS(new Vector((float)this.latitude, (float)this.longitude));
			result.y = (float)this.altitude;
			return result;
		}

		public Vector3 convertCoordinateToVector(float y)
		{
			Vector3 result = GPSEncoder.GPSToUCS(new Vector((float)this.latitude, (float)this.longitude));
			result.y = y;
			return result;
		}

		public Vector convertCoordinateToVector2D()
		{
			Vector3 vector = GPSEncoder.GPSToUCS(new Vector((float)this.latitude, (float)this.longitude));
			return new Vector(vector.x, vector.z);
		}

		public bool isEqualToCoordinate(Coordinates coordinate)
		{
			return this.Equals(coordinate);
		}

		public bool Equals(Coordinates coordinate)
		{
			return coordinate.latitude.Equal(this.latitude) && coordinate.longitude.Equal(this.longitude);
		}

		public double intervalBetweenTimestamps(Coordinates coordinate)
		{
			return Math.Abs(coordinate.timestampLastUpdate - this.timestampLastUpdate);
		}

		public Vector tileCoordinates(int zoom)
		{
			return Coordinates.WorldToTilePos(this.longitude, this.latitude, zoom);
		}

		public Coordinates tileCenter(int zoom)
		{
			Vector vector = this.tileCoordinates(zoom);
			Vector vector2 = this.TileToWorldPos((double)vector.x + 0.5, (double)vector.y + 0.5, zoom);
			return new Coordinates((double)vector2.y, (double)vector2.x, 0.0);
		}

		public Coordinates tileOrigin(int zoom)
		{
			Vector vector = this.tileCoordinates(zoom);
			Vector vector2 = this.TileToWorldPos((double)vector.x, (double)vector.y, zoom);
			return new Coordinates((double)vector2.y, (double)vector2.x, 0.0);
		}

		public float diagonalLenght(int zoom)
		{
			Coordinates coordinates = this.tileCenter(zoom);
			Coordinates coordinates2 = this.tileOrigin(zoom);
			return 2f * Mathf.Abs(Vector.Distance(coordinates.convertCoordinateToVector2D(), coordinates2.convertCoordinateToVector2D()));
		}

		public float tileSize(int zoom)
		{
			Vector vector = this.tileCoordinates(zoom);
			Vector3 vector2 = GPSEncoder.GPSToUCS(this.TileToWorldPos((double)vector.x, (double)vector.y, zoom).flipped());
			Vector3 vector3 = GPSEncoder.GPSToUCS(this.TileToWorldPos((double)(vector.x + 1f), (double)vector.y, zoom).flipped());
			return Mathf.Abs(Vector.Distance(vector2, vector3));
		}

		public List<Vector3> tileVertices(int zoom)
		{
			float num = this.tileSize(zoom) / 2f;
			return new List<Vector3>
			{
				new Vector3(num, -0.1f, num),
				new Vector3(num, -0.1f, -num),
				new Vector3(-num, -0.1f, -num),
				new Vector3(-num, -0.1f, num)
			};
		}

		public List<Vector2> adiacentNTiles(int zoom, int buffer)
		{
			List<Vector2> list = new List<Vector2>();
			Vector2 centerTileCoords = this.tileCoordinates(zoom);
			int num = 1 << zoom;
			for (int i = -buffer; i <= buffer; i++)
			{
				for (int j = -buffer; j <= buffer; j++)
				{
					Vector2 item = new Vector2(centerTileCoords.x + (float)j, centerTileCoords.y + (float)i);
					if (item.y >= 0f && item.y < (float)num)
					{
						list.Add(item);
					}
				}
			}
			list.Sort(delegate(Vector2 a, Vector2 b)
			{
				float num2 = Math.Abs(Vector2.Distance(centerTileCoords, a));
				float value = Math.Abs(Vector2.Distance(centerTileCoords, b));
				return num2.CompareTo(value);
			});
			for (int k = 0; k < list.Count; k++)
			{
				Vector2 vector = list[k];
				if (vector.x < 0f)
				{
					list[k] = new Vector2(vector.x + (float)num, vector.y);
				}
				else if (vector.x >= (float)num)
				{
					list[k] = new Vector(vector.x - (float)num, vector.y);
				}
			}
			return list;
		}

		public Coordinates pointAtDistance(int latitudeMeters, int longitudeMeters)
		{
			Vector vector;
			vector..ctor((float)longitudeMeters, (float)latitudeMeters);
			double num = (double)vector.magnitude;
			double num2 = (double)(0.0174532924f * Vector.Angle(Vector.up, vector));
			if (Vector3.Cross(Vector.up, vector).z > 0f)
			{
				num2 = 6.2831853071795862 - num2;
			}
			double num3 = Math.Sin(0.01745329238474369 * this.latitude);
			double num4 = Math.Cos(0.01745329238474369 * this.latitude);
			double num5 = num / (double)Coordinates.EarthRadius;
			double num6 = Math.Sin(num2);
			double num7 = Math.Cos(num2);
			double num8 = Math.Sin(num5);
			double num9 = Math.Cos(num5);
			double num10 = Math.Asin(num3 * num9 + num4 * num8 * num7);
			double num11 = 0.01745329238474369 * this.longitude + Math.Atan2(num6 * num8 * num4, num9 - num3 * Math.Sin(num10));
			num11 = (num11 + 9.42477796076938) % 6.2831853071795862 - 3.1415926535897931;
			return new Coordinates(57.295780181884766 * num10, 57.295780181884766 * num11, 0.0);
		}

		public static Vector WorldToTilePos(double lon, double lat, int zoom)
		{
			Vector result = default(Vector);
			result.x = (float)((int)((lon + 180.0) / 360.0 * (double)(1 << zoom)));
			result.y = (float)((int)((1.0 - Math.Log(Math.Tan(lat * 3.1415926535897931 / 180.0) + 1.0 / Math.Cos(lat * 3.1415926535897931 / 180.0)) / 3.1415926535897931) / 2.0 * (double)(1 << zoom)));
			return result;
		}

		private Vector2 TileToWorldPos(double tile_x, double tile_y, int zoom)
		{
            Vector2 result = default(Vector);
			double value = 3.1415926535897931 - 6.2831853071795862 * tile_y / Math.Pow(2.0, (double)zoom);
			result.x = (float)(tile_x / Math.Pow(2.0, (double)zoom) * 360.0 - 180.0);
			result.y = (float)(57.295779513082323 * Math.Atan(Math.Sinh(value)));
			return result;
		}

		public string description()
		{
			return string.Concat(new object[]
			{
				"lat: ",
				this.latitude,
				", lon: ",
				this.longitude
			});
		}

		public string toLatLongString()
		{
			return this.latitude.ToString() + "," + this.longitude.ToString();
		}

		public static double DistanceBetweenInMeters(Vector3 pos1, Vector3 pos2)
		{
			return Coordinates.Of(pos1).DistanceToInMeters(Coordinates.Of(pos2));
		}

		public static Coordinates Of(Vector3 vector)
		{
			Coordinates coordinates = new Coordinates(0.0, 0.0, 0.0);
			Vector vector2 = GPSEncoder.USCToGPS(vector);
			coordinates.latitude = (double)vector2.x;
			coordinates.longitude = (double)vector2.y;
			return coordinates;
		}

		public static Coordinates convertVectorToCoordinates(Vector3 vector)
		{
			Coordinates coordinates = new Coordinates(0.0, 0.0, 0.0);
			Vector vector2 = GPSEncoder.USCToGPS(vector);
			coordinates.latitude = (double)vector2.x;
			coordinates.longitude = (double)vector2.y;
			coordinates.altitude = (double)vector.y;
			return coordinates;
		}

		public static void setStartLocation(Coordinates originCoords)
		{
			GPSEncoder.SetLocalOrigin(new Vector((float)originCoords.latitude, (float)originCoords.longitude));
		}

		public string ToUserString()
		{
			return string.Concat(new object[]
			{
				"latitude=",
				this.latitude,
				", longitude=",
				this.longitude,
				", altitude=",
				this.altitude
			});
		}

		public static Coordinates getWorldOrigin()
		{
			Vector localOrigin = GPSEncoder.GetLocalOrigin();
			return new Coordinates((double)localOrigin.x, (double)localOrigin.y, 0.0);
		}

		public static double getCurrentTimestamp()
		{
			if (!Coordinates.gameContextCreated)
			{
				Coordinates.gameContextCreated = (GameContext.i != null);
			}
			if (Coordinates.gameContextCreated)
			{
				if (GameContext.i.frameNumber != Coordinates.frameNumber)
				{
					Coordinates.frameNumber = GameContext.i.frameNumber;
					Coordinates.updateTimestamp(GameContext.i.UtcNow);
				}
			}
			else
			{
				Coordinates.updateTimestamp(DateTime.UtcNow);
			}
			return Coordinates.timestamp;
		}

		private static void updateTimestamp(DateTime dateTime)
		{
			Coordinates.timestamp = (dateTime - Coordinates.epochStart).TotalSeconds;
		}

		private string toDms(double value)
		{
			int num = (int)value;
			int num2 = (int)((value - (double)num) * 60.0);
			int num3 = (int)((value - (double)num - (double)((float)num2 / 60f)) * 3600.0);
			return string.Concat(new object[]
			{
				num,
				"° ",
				num2,
				"' ",
				num3,
				"\""
			});
		}

		public string ToDmsString()
		{
			return "lat = " + this.toDms(this.latitude) + ", lng = " + this.toDms(this.longitude);
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"lat=",
				this.latitude,
				", lng=",
				this.longitude,
				", hAccu=",
				this.horizontalAccuracy,
				", timestampLastUpdate=",
				this.timestampLastUpdate
			});
		}

		public static int EarthRadius = 6371000;

		public double latitude;

		public double longitude;

		public double altitude;

		[HideInInspector]
		public double timestampLastUpdate;

		[HideInInspector]
		public double? horizontalAccuracy;

		[HideInInspector]
		public double? verticalAccuracy;

		private static double timestamp;

		private static readonly DateTime epochStart = new DateTime(1970, 1, 1, 8, 0, 0, DateTimeKind.Utc);

		private static int frameNumber;

		private static bool gameContextCreated;*/
	}
}
