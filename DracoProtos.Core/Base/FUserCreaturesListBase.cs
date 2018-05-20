using System.Collections.Generic;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FUserCreaturesListBase : FBaseItemUpdate
	{
        public List<FUserCreature> userCreatures;

        public override void ReadExternal(FInputStream stream)
		{
			base.ReadExternal(stream);
			this.userCreatures = stream.ReadStaticList<FUserCreature>(true);
		}

		public override void WriteExternal(FOutputStream stream)
		{
			base.WriteExternal(stream);
			stream.WriteStaticCollection(this.userCreatures, true);
		}
	}
}
