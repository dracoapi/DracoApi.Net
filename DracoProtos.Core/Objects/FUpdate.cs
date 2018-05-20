using DracoProtos.Core.Base;
using System;
using System.Linq;

namespace DracoProtos.Core.Objects
{
    public class FUpdate : FUpdateBase
	{
		public void OnServerUpdate()
		{
		}

		public FBaseItemUpdate FindFirst(Type type)
		{
			return this.items.FirstOrDefault((FBaseItemUpdate item) => item.GetType() == type);
		}
	}
}
