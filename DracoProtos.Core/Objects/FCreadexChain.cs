using DracoProtos.Core.Base;

namespace DracoProtos.Core.Objects
{
    public class FCreadexChain : FCreadexChainBase
	{
		protected bool Equals(FCreadexChainBase other)
		{
			return this.creature == other.creature && this.seen == other.seen && this.caught == other.caught;
		}

		public override bool Equals(object obj)
		{
			return !object.ReferenceEquals(null, obj) && (object.ReferenceEquals(this, obj) || (obj.GetType() == base.GetType() && this.Equals((FCreadexChainBase)obj)));
		}

		public override int GetHashCode()
		{
			int num = (int)this.creature;
			num = (num * 397 ^ this.seen.GetHashCode());
			return num * 397 ^ this.caught.GetHashCode();
		}

		public static bool operator ==(FCreadexChain left, FCreadexChain right)
		{
			return object.Equals(left, right);
		}

		public static bool operator !=(FCreadexChain left, FCreadexChain right)
		{
			return !object.Equals(left, right);
		}
	}
}
