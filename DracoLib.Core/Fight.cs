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

        public new FEncounterUpdate StartEncounter(FStartEncounterRequest attack)
        {
            return client.Call(client.encounter.StartEncounter(attack));
        }

        public new object GiveUpEncounter()
        {
            return client.Call(client.encounter.GiveUpEncounter());
        }

        public new FEncounterBattleResult UpdateEncounterDetails(FFightRequest fightRequest)
        {
            return client.Call(client.encounter.UpdateEncounterDetails(fightRequest));
        }
    }
}
