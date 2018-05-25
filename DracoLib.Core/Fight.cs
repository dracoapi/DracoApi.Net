using DracoProtos.Core.Objects;

namespace DracoLib.Core
{
    public class Fight    
    {
        private DracoClient dracoClient;

        public Fight(DracoClient dracoClient)
        {
            this.dracoClient = dracoClient;
        }

        public object Start(FStartEncounterRequest attack)
        {
            return dracoClient.Call("EncounterService", "startEncounter", new object[] { attack }) as FEncounterUpdate;
        }

        public object GiveUp()
        {
            return dracoClient.Call("EncounterService", "giveUpEncounter", new object[] { });
        }
    }
}