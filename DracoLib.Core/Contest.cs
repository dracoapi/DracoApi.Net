using DracoProtos.Core.Base;
using DracoProtos.Core.Objects;
using System.Collections.Generic;

namespace DracoLib.Core
{
    public class Contest : ContestMapService
    {
        private readonly DracoClient client;

        public Contest(DracoClient dracoClient)
        {
            this.client = dracoClient;
        }

        public new FUpdate AcceptStartContest(bool accept)
        {
            return client.Call(client.clientContestMap.AcceptStartContest(accept));
        }

        public new FUpdate CreateContest(GeoCoords userCoords)
        {
            return client.Call(client.clientContestMap.CreateContest(userCoords));
        }

        public new FUpdate CreateContestWithPassword(GeoCoords userCoords, string passwordHash)
        {
            return client.Call(client.clientContestMap.CreateContestWithPassword(userCoords, passwordHash));
        }

        public new object DevTimeoutInContest()
        {
            return client.Call(client.clientContestMap.DevTimeoutInContest());
        }

        public new FUpdate FinishContestBattle(FFightRequest fightRequest, string opponentNickname)
        {
            return client.Call(client.clientContestMap.FinishContestBattle(fightRequest, opponentNickname));
        }

        public new FContestUpdate GetContestUpdate(BuildingType requestBuildingType, ClientScreen clientScreen)
        {
            return client.Call(client.clientContestMap.GetContestUpdate(requestBuildingType, clientScreen));
        }

        public new FUserCreaturesList GetPossibleAttackers()
        {
            return client.Call(client.clientContestMap.GetPossibleAttackers());
        }

        public new FUpdate JoinContest(GeoCoords coords, string id)
        {
            return client.Call(client.clientContestMap.JoinContest(coords, id));
        }

        public new FUpdate JoinContestWithPassword(GeoCoords coords, string id, string passwordHash)
        {
            return client.Call(client.clientContestMap.JoinContestWithPassword(coords, id, passwordHash));
        }

        public new FUpdate LeaveContest()
        {
            return client.Call(client.clientContestMap.LeaveContest());
        }

        public new FUpdate StartContest()
        {
            return client.Call(client.clientContestMap.StartContest());
        }

        public new FFightUpdate StartContestBattle(List<string> attackerIds, string opponentNickname)
        {
            return client.Call(client.clientContestMap.StartContestBattle(attackerIds, opponentNickname));
        }

        public new FUpdate SurrenderContest()
        {
            return client.Call(client.clientContestMap.SurrenderContest());
        }
    }
}
