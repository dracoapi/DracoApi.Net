using DracoProtos.Core.Base;

namespace DracoProtos.Core.Objects
{
#pragma warning disable CS0659 // Le type se substitue à Object.Equals(object o) mais pas à Object.GetHashCode()
    public class FBagUpdate : FBagUpdateBase
#pragma warning restore CS0659 // Le type se substitue à Object.Equals(object o) mais pas à Object.GetHashCode()
    {

		public override bool Equals(object obj)
		{
			return !object.ReferenceEquals(null, obj) && (object.ReferenceEquals(this, obj) || (obj.GetType() == base.GetType() && this.Equals((FBagUpdateBase)obj)));
		}

		public static bool operator ==(FBagUpdate left, FBagUpdate right)
		{
			return object.Equals(left, right);
		}

		public static bool operator !=(FBagUpdate left, FBagUpdate right)
		{
			return !object.Equals(left, right);
		}
	}
}
