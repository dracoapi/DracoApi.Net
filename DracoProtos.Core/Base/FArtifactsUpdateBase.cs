using System.Collections.Generic;
using DracoProtos.Core.Enums;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FArtifactsUpdateBase : FObject
	{
        public List<ArtifactName> artifacts;
        public int artifactsBagSize;
        public int artifactsSlots;
        public FAvaUpdateBase avaUpdate;
        public bool hasBonusSetFirst;
        public bool hasBonusSetSecond;
        public List<ArtifactName> putOn;

        public void ReadExternal(FInputStream stream)
		{
			this.artifacts = stream.ReadStaticList<ArtifactName>(true);
			this.artifactsBagSize = stream.ReadInt32();
			this.artifactsSlots = stream.ReadInt32();
            this.avaUpdate = (FAvaUpdateBase)stream.ReadStaticObject(typeof(FAvaUpdateBase));
            this.hasBonusSetFirst = stream.ReadBoolean();
            this.hasBonusSetSecond = stream.ReadBoolean();
			this.putOn = stream.ReadStaticList<ArtifactName>(true);
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteStaticCollection(this.artifacts, true);
			stream.WriteInt32(this.artifactsBagSize);
			stream.WriteInt32(this.artifactsSlots);
            stream.WriteStaticObject(this.avaUpdate);
			stream.WriteBoolean(this.hasBonusSetFirst);
            stream.WriteBoolean(this.hasBonusSetSecond);
			stream.WriteStaticCollection(this.putOn, true);
		}
	}
}
