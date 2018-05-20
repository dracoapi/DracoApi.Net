using DracoProtos.Core.Enums;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FNicknameValidationResultBase : FObject
	{
        public FNicknameValidationError? error;
        public string suggestedNickname;

        public void ReadExternal(FInputStream stream)
		{
			this.error = (FNicknameValidationError?)stream.ReadDynamicObject();
			this.suggestedNickname = (string)stream.ReadDynamicObject();
		}

		public void WriteExternal(FOutputStream stream)
		{
			stream.WriteDynamicObject(this.error);
			stream.WriteDynamicObject(this.suggestedNickname);
		}
	}
}
