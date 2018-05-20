using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FServiceErrorBase : FObject
    {
        public object[] args;
        public string cause;

        public void ReadExternal(FInputStream stream)
        {
            this.args = stream.ReadStaticArray<object>(false);
            this.cause = stream.ReadUtfString();
        }
        
        public void WriteExternal(FOutputStream stream)
        {
            stream.WriteStaticCollection(this.args, false);
            stream.WriteUtfString(this.cause);
        }
    }
}
