using DracoProtos.Core.Base;

namespace DracoProtos.Core.Objects
{
    public class FCreadexEntry : FCreadexEntryBase
	{
		public static bool operator ==(FCreadexEntry left, FCreadexEntry right)
		{
			return object.Equals(left, right);
		}

		public static bool operator !=(FCreadexEntry left, FCreadexEntry right)
		{
			return !object.Equals(left, right);
		}
	}
}
