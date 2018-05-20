using DracoProtos.Core.Enums;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class ExtraPackBase : FObject
    {
        public int price;
        public int wizardBattles;

        public void ReadExternal(FInputStream stream)
        {
            this.price = stream.ReadInt32();
            this.wizardBattles = stream.ReadInt32();
        }

        public void WriteExternal(FOutputStream stream)
        {
            stream.WriteInt32(this.price);
            stream.WriteInt32(this.wizardBattles);
        }
    }
}
