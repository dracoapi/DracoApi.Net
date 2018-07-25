using DracoProtos.Core.Base;
using DracoProtos.Core.Objects;

namespace DracoLib.Core
{
    public class Fight : EncounterService
    {
        private readonly DracoClient dracoClient;

        public Fight(DracoClient dracoClient)
        {
            this.dracoClient = dracoClient;
        }
        public FEncounterUpdate Start(FStartEncounterRequest attack)
        {
            return dracoClient.Call(base.StartEncounter(attack));
        }

        public object GiveUp()
        {
            return dracoClient.Call(base.GiveUpEncounter());
        }

        // To hide base methods
        private new FEncounterUpdate StartEncounter(FStartEncounterRequest attack)
        {
            return null;
        }
        private new object GiveUpEncounter()
        {
            return null;
        }


    }
}