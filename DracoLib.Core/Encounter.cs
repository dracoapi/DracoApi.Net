using DracoProtos.Core.Base;
using DracoProtos.Core.Objects;

namespace DracoLib.Core
{
    public class Encounter : EncounterService
    {
        private readonly DracoClient client;

        public Encounter(DracoClient dracoClient)
        {
            this.client = dracoClient;
        }

        public new object GiveUpEncounter()
        {
            return client.Call(client.clientEncounter.GiveUpEncounter());
        }

        public new FEncounterUpdate StartEncounter(FStartEncounterRequest attack)
        {
            return client.Call(client.clientEncounter.StartEncounter(attack));
        }

        public new FEncounterBattleResult UpdateEncounterDetails(FFightRequest fightRequest)
        {
            return client.Call(client.clientEncounter.UpdateEncounterDetails(fightRequest));
        }
    }
}
