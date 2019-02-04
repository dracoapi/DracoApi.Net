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
            return client.Call(client.userCreature.GetHatchingInfo());
        }

        public new FHatchingResult OpenHatchedEggWithCreature(string incubatorId, CreatureType selectedCreatureType)
        {
            return client.Call(client.userCreature.OpenHatchedEggWithCreature(incubatorId, selectedCreatureType));
        }

        public new FHatchingResult OpenHatchedEgg(string incubatorId)
        {
            return client.Call(client.userCreature.OpenHatchedEgg(incubatorId));
        }

        public new object StartHatchingEgg(string eggId, string incubatorId)
        {
            client.Call(client.userCreature.StartHatchingEgg(eggId, incubatorId));
            return GetHatchingInfo();
        }

        public new object StartHatchingEggInRoost(string eggId, FBuildingRequest roost, int slot)
        {
            return client.Call(client.userCreature.StartHatchingEggInRoost(eggId, roost, slot));
        }
    }
}
