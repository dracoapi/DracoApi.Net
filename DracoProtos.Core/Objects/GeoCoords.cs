using DracoProtos.Core.Base;
using DracoProtos.Core.Extensions;
using System;
using System.Collections.Generic;

namespace DracoProtos.Core.Objects
{
    public class GeoCoords : GeoCoordsBase
	{
		public static GeoCoords of(Coordinates coordinates)
		{
			if (coordinates == null)
			{
				return GeoCoords.UNKNOWN;
			}
			return new GeoCoords
			{
				latitude = coordinates.latitude,
				longitude = coordinates.longitude,
				//horizontalAccuracy = coordinates.horizontalAccuracy
			};
		}

		public Coordinates toGroundLevel()
		{
			return new Coordinates(this.latitude, this.longitude, 0.0, true);
		}

		public Tile toTile()
		{
			return Tile.from15(this.tileX(15), this.tileY(15));
		}

		public int tileX(int zoom)
		{
			return (int)((this.longitude + 180.0) / 360.0 * (double)(1 << zoom));
		}

		public int tileY(int zoom)
		{
			double num = this.latitude * 3.1415926535897931 / 180.0;
			return (int)((1.0 - Math.Log(Math.Tan(num) + 1.0 / Math.Cos(num)) / 3.1415926535897931) * (double)(1 << zoom - 1));
		}

		public string ToUserString()
		{
			return this.latitude.ToString("###.000") + " " + this.longitude.ToString("###.000");
		}

		public List<Vector2> tilesInRangeMeters(double range, int zoom)
		{
			int num = this.tileX(zoom);
			int num2 = this.tileY(zoom);
			double num3 = GeoCoords.topLeftOf(num + 1, num2, zoom).distanceTo(GeoCoords.topLeftOf(num, num2, zoom));
			double num4 = range / num3;
			int num5 = (int)Math.Ceiling(num4);
			int num6 = num5 * 2 + 1;
			if (num6 > 49)
			{
				throw new Exception("range too big:" + range);
			}
			GeoCoords geoCoords = GeoCoords.topLeftOf(num, num2, zoom);
			GeoCoords geoCoords2 = GeoCoords.topLeftOf(num + 1, num2 + 1, zoom);
			GeoCoords geoCoords3 = GeoCoords.centerOf(num, num2, zoom);
			double fromX = (this.longitude - geoCoords3.longitude) / (geoCoords2.longitude - geoCoords.longitude);
			double fromY = (this.latitude - geoCoords3.latitude) / (geoCoords2.latitude - geoCoords.latitude);
			double num7 = num4 * num4;
			List<Vector2> list = new List<Vector2>();
			int num8 = (1 << zoom) - 1;
			for (int i = -num5; i <= num5; i++)
			{
				for (int j = -num5; j <= num5; j++)
				{
					if (GeoCoords.distanceSqToNearestPointInSquare(fromX, fromY, i, j) < num7)
					{
						int num9 = num2 + j;
						if (num9 >= 0 && num9 <= num8)
						{
							int num10 = num + i & num8;
							list.Add(new Vector2((float)num10, (float)num9));
						}
					}
				}
			}
			return list;
		}

		private static double distanceSqToNearestPointInSquare(double fromX, double fromY, int squareX, int squareY)
		{
			long num = (long)Math.Round(fromX);
			long num2 = (long)Math.Round(fromY);
			double num3 = (num != (long)squareX) ? (fromX - (double)squareX + 0.5 * (double)Math.Sign((long)squareX - num)) : 0.0;
			double num4 = (num2 != (long)squareY) ? (fromY - (double)squareY + 0.5 * (double)Math.Sign((long)squareY - num2)) : 0.0;
			return num3 * num3 + num4 * num4;
		}

		public static GeoCoords topLeftOf(int x, int y, int zoom)
		{
			double value = 3.1415926535897931 - 3.1415926535897931 * (double)y / (double)(1 << zoom - 1);
			return new GeoCoords
			{
				latitude = 57.295779513082323 * Math.Atan(Math.Sinh(value)),
				longitude = 360.0 * (double)x / (double)(1 << zoom) - 180.0
			};
		}

		public static GeoCoords centerOf(int x, int y, int zoom)
		{
			GeoCoords geoCoords = GeoCoords.topLeftOf(x, y, zoom);
			GeoCoords geoCoords2 = GeoCoords.topLeftOf(x + 1, y + 1, zoom);
			return new GeoCoords
			{
				latitude = (geoCoords.latitude + geoCoords2.latitude) / 2.0,
				longitude = (geoCoords.longitude + geoCoords2.longitude) / 2.0
			};
		}

		public double distanceTo(GeoCoords other)
		{
			double num = GeoCoords.toRadians(other.latitude - this.latitude);
			double num2 = GeoCoords.toRadians(other.longitude - this.longitude);
			double num3 = Math.Sin(num / 2.0);
			double num4 = Math.Sin(num2 / 2.0);
			double num5 = num3 * num3 + num4 * num4 * Math.Cos(GeoCoords.toRadians(this.latitude)) * Math.Cos(GeoCoords.toRadians(other.latitude));
			double num6 = 2.0 * Math.Atan2(Math.Sqrt(num5), Math.Sqrt(1.0 - num5));
			return (double)GeoCoords.EARTH_RADIUS * num6;
		}

		private static double toRadians(double angdeg)
		{
			return angdeg / 180.0 * 3.1415926535897931;
		}

		public static readonly int EARTH_RADIUS = 6371000;

		public static readonly GeoCoords UNKNOWN = new GeoCoords
		{
			latitude = 0.0,
			longitude = 0.0
		};
	}
}
