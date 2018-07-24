using DracoProtos.Core.Base;
using DracoProtos.Core.Objects;

namespace DracoLib.Core
{
    public class Eggs
    {
        private readonly DracoClient dracoClient;

        public Eggs(DracoClient dracoClient)
        {
            this.dracoClient = dracoClient;
        }

        public FUserHatchingInfo GetHatchingInfo()
        {
            return this.dracoClient.Call(new UserCreatureService().GetHatchingInfo());
        }

        public FHatchingResult OpenHatchedEgg(string incubatorId)
        {
            return this.dracoClient.Call(new UserCreatureService().OpenHatchedEgg(incubatorId));
        }

        public object StartHatchingEgg(string eggId, string incubatorId)
        {
            this.dracoClient.Call(new UserCreatureService().StartHatchingEgg(eggId, incubatorId));
            return this.GetHatchingInfo();
        }

        public object StartHatchingEggInRoost(string eggId, FBuildingRequest roost, int slot)
        {
            return this.dracoClient.Call(new UserCreatureService().StartHatchingEggInRoost(eggId, roost, slot));
        }
    }
}