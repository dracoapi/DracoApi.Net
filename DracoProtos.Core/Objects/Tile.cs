using DracoProtos.Core.Base;

namespace DracoProtos.Core.Objects
{
    public class Tile : TileBase
	{
		public static int calcZoom(int y15)
		{
			if (y15 >= 9410 && y15 < 23358)
			{
				return 15;
			}
			if (y15 >= 5528 && y15 < 27240)
			{
				return 14;
			}
			return 13;
		}

		public static Tile from15(int x15, int y15)
		{
			int num = Tile.calcZoom(y15);
			x15 &= 32767;
			return new Tile
			{
				zoom = num,
				x = x15 >> 15 - num,
				y = y15 >> 15 - num
			};
		}

		public string ToUserString()
		{
			return string.Concat(new object[]
			{
				"x=",
				this.x,
				", y=",
				this.y,
				", zoom=",
				this.zoom
			});
		}
	}
}
