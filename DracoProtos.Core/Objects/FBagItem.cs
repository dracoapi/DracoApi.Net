using DracoProtos.Core.Base;
using DracoProtos.Core.Enums;

namespace DracoProtos.Core.Objects
{
    public class FBagItem : FBagItemBase
	{
		public FBagItem()
		{
		}

		public FBagItem(ItemType type, int count, bool removable, bool stack)
		{
			this.type = type;
			this.count = count;
			this.removable = removable;
			this.stack = stack;
		}

		protected bool Equals(FBagItemBase other)
		{
			return this.type == other.type && this.count == other.count && this.removable == other.removable && this.stack == other.stack;
		}

		public override bool Equals(object obj)
		{
			return !object.ReferenceEquals(null, obj) && (object.ReferenceEquals(this, obj) || (obj.GetType() == base.GetType() && this.Equals((FBagItemBase)obj)));
		}

		public override int GetHashCode()
		{
			int num = (int)this.type;
			num = (num * 397 ^ this.count);
			num = (num * 397 ^ this.removable.GetHashCode());
			return num * 397 ^ this.stack.GetHashCode();
		}

		public static bool operator ==(FBagItem left, FBagItem right)
		{
			return object.Equals(left, right);
		}

		public static bool operator !=(FBagItem left, FBagItem right)
		{
			return !object.Equals(left, right);
		}
	}
}
