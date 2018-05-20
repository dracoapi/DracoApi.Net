using DracoProtos.Core.Base;

namespace DracoProtos.Core.Objects
{
    public class FEncounterUpdate : FEncounterUpdateBase
	{
		public override void Handle()
		{
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"FEncounterUpdate(attacker = ",
				this.attacker,
				"\ndefender=",
				this.defender,
				")"
			});
		}
	}
}
