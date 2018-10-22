using DracoProtos.Core.Base;
using DracoProtos.Core.Objects;

namespace DracoLib.Core
{
    public class Fight : EncounterService
    {
        private readonly DracoClient client;

        public Fight(DracoClient dracoClient)
        {
            this.client = dracoClient;
        }
        public FEncounterUpdate Start(FStartEncounterRequest attack)
        {
            return client.Call(client.encounter.StartEncounter(attack));
        }

        public object GiveUp()
        {
            return client.Call(client.encounter.GiveUpEncounter());
        }

        // To hide base methods
        private new FEncounterUpdate StartEncounter(FStartEncounterRequest attack)
        {
            return client.Call(client.encounter.StartEncounter(attack));
        }
        private new object GiveUpEncounter()
        {
            return client.Call(client.encounter.GiveUpEncounter());
        }
    }
}
