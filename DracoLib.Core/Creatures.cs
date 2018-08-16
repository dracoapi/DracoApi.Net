using DracoProtos.Core.Base;
using DracoProtos.Core.Objects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DracoLib.Core
{
    public class Creatures : UserCreatureService
    {
        private readonly DracoClient client;
        private readonly GamePlayService gamePlayService;

        public Creatures(DracoClient dclient)
        {
            client = dclient;
            gamePlayService = new GamePlayService();
        }

        public FCatchingCreature Encounter(string id)
        {
            var request = new FCreatureRequest
            {
                id = id,
                //veryFirst = true
            };

            var response = client.Call(gamePlayService.StartCatchingCreature(request));

            if (client.Config.Delay > 0)
            {
                var delay = client.Config.Delay * 1500;
                Task.Run(async() => await client.Delay(delay));
            }

            //this.dracoClient.Event("IsArAvailable", "False");

            return response;
        }

        public FCatchCreatureResult Catch(string id, ItemType ball, float quality, bool spin, object options = null)
        {
            return client.Call(gamePlayService.TryCatchCreature(id, ball, quality, spin));
        }

        public FUpdate Release(List<string> ids)
        {
            return client.Call(base.ConvertCreaturesToCandies(ids, false));
        }

        public FUserCreatureUpdate Evolve(string id, CreatureType toType)
        {
            return client.Call(base.EvolveCreature(id, toType));
        }

        private new FUpdate ConvertCreaturesToCandies(List<string> ids, bool sendUpdate)
        {
           return client.Call(base.ConvertCreaturesToCandies(ids, sendUpdate));
        }
    }
}