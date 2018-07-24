using DracoProtos.Core.Base;
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
            return this.dracoClient.Call(new EncounterService().StartEncounter(attack));
        }

        public object GiveUp()
        {
            return this.dracoClient.Call(new EncounterService().GiveUpEncounter());
        }
    }
}