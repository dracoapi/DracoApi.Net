using DracoProtos.Core.Base;
using DracoProtos.Core.Objects;

namespace DracoLib.Core
{
    public class Eggs : UserCreatureService
    {
        private readonly DracoClient client;

        public Eggs(DracoClient dracoClient)
        {
            client = dracoClient;
        }

        public new FUserHatchingInfo GetHatchingInfo()
        {
            return client.Call(base.GetHatchingInfo());
        }

        public new FHatchingResult OpenHatchedEgg(string incubatorId)
        {
            return client.Call(base.OpenHatchedEgg(incubatorId));
        }

        public new object StartHatchingEgg(string eggId, string incubatorId)
        {
            client.Call(base.StartHatchingEgg(eggId, incubatorId));
            return GetHatchingInfo();
        }

        public new object StartHatchingEggInRoost(string eggId, FBuildingRequest roost, int slot)
        {
            return client.Call(base.StartHatchingEggInRoost(eggId, roost, slot));
        }
    }
}