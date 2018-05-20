using DracoProtos.Core.Enums;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FLootItemArtifactBase : FBaseLootItem
	{
        //public int qty;
        public ArtifactName artifact;

        public override void ReadExternal(FInputStream stream)
		{
			base.ReadExternal(stream);
            //this.qty = stream.ReadInt32();
			this.artifact = (ArtifactName)stream.ReadEnum(typeof(ArtifactName));
		}

		public override void WriteExternal(FOutputStream stream)
		{
			base.WriteExternal(stream);
            //stream.WriteInt32(this.qty);
			stream.WriteEnum(this.artifact);
		}
	}
}
