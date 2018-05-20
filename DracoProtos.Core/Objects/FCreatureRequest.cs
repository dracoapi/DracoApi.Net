using DracoProtos.Core.Base;

namespace DracoProtos.Core.Objects
{
    public class FCreatureRequest : FCreatureRequestBase
	{
		public static FCreatureRequest of(FWildCreature creature, bool veryFirst)
		{
			return new FCreatureRequest
			{
				id = creature.id,
				veryFirst = veryFirst
			};
		}
	}
}
