using DracoProtos.Core.Base;

namespace DracoProtos.Core.Objects
{
    public class FRegistrationInfo : FRegistrationInfoBase
	{
		public FRegistrationInfo()
		{
		}

		public FRegistrationInfo(string regType)
		{
            this.age = string.Empty;
            this.gender = string.Empty;
            this.regType = regType;
            this.email = string.Empty;
			this.socialId = string.Empty;
		}
	}
}
