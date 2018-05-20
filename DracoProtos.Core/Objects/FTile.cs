using DracoProtos.Core.Base;

namespace DracoProtos.Core.Objects
{
    public class FTile : FTileBase
	{
		protected bool Equals(FTileBase other)
		{
			return this.tile.zoom == other.tile.zoom && this.tile.x == other.tile.x && this.tile.y == other.tile.y && string.Equals(this.dungeonId, other.dungeonId);
		}

		public override bool Equals(object obj)
		{
			return !object.ReferenceEquals(null, obj) && (object.ReferenceEquals(this, obj) || (obj.GetType() == base.GetType() && this.Equals((FTileBase)obj)));
		}

		public override int GetHashCode()
		{
			int num = this.tile.zoom;
			num = (num * 397 ^ this.tile.x);
			num = (num * 397 ^ this.tile.y);
			return num * 397 ^ ((this.dungeonId == null) ? 0 : this.dungeonId.GetHashCode());
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"zoom:",
				this.tile.zoom,
				"; x: ",
				this.tile.x,
				"; y: ",
				this.tile.y,
				"; dungeionId = ",
				this.dungeonId
			});
		}
	}
}
