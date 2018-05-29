using DracoProtos.Core.Objects;

namespace DracoLib.Core
{
    public class Fight    
    {
        private readonly DracoClient dracoClient;

        public Fight(DracoClient dracoClient)
        {
            this.dracoClient = dracoClient;
        }

        public FEncounterUpdate Start(FStartEncounterRequest attack)
        {
            return this.dracoClient.Call("EncounterService", "startEncounter", new object[] { attack }) as FEncounterUpdate;
        }

        public object GiveUp()
        {
            return this.dracoClient.Call("EncounterService", "giveUpEncounter", new object[] { });
        }
    }
}