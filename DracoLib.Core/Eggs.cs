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
            return this.dracoClient.Call("UserCreatureService", "getHatchingInfo", new object[] { }) as FUserHatchingInfo;
        }

        public object OpenHatchedEgg(string incubatorId)
        {
            return this.dracoClient.Call("UserCreatureService", "openHatchedEgg", new object[] { incubatorId });
        }

        public object StartHatchingEgg(string eggId, string incubatorId)
        {
            this.dracoClient.Call("UserCreatureService", "startHatchingEgg", new object[] { eggId, incubatorId, });
            return this.GetHatchingInfo();
        }

        public object StartHatchingEggInRoost(string eggId, FBuildingRequest roost, int slot)
        {
            return this.dracoClient.Call("UserCreatureService", "startHatchingEggInRoost", new object[] { eggId, roost, slot });
        }
    }
}