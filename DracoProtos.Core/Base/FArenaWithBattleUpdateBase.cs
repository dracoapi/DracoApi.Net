using System.Collections.Generic;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FArenaWithBattleUpdateBase : FBaseItemUpdate
	{
        public HashSet<string> arenaWithBattle;

        public override void ReadExternal(FInputStream stream)
		{
			base.ReadExternal(stream);
			this.arenaWithBattle = stream.ReadStaticHashSet<string>(true);
		}

		public override void WriteExternal(FOutputStream stream)
		{
			base.WriteExternal(stream);
			stream.WriteStaticEnumerable(this.arenaWithBattle, true);
		}
	}
}
