using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;

namespace DracoProtos.Core.Base
{
    public abstract class FUpdateNicknameResultBase : FBaseItemUpdate
	{
        public FUserInfo userInfo;
        public FNicknameValidationResult validationResult;

        public override void ReadExternal(FInputStream stream)
		{
			base.ReadExternal(stream);
			this.userInfo = (FUserInfo)stream.ReadDynamicObject();
			this.validationResult = (FNicknameValidationResult)stream.ReadDynamicObject();
		}

		public override void WriteExternal(FOutputStream stream)
		{
			base.WriteExternal(stream);
			stream.WriteDynamicObject(this.userInfo);
			stream.WriteDynamicObject(this.validationResult);
		}
	}
}
