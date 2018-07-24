using DracoProtos.Core.Base;
using DracoProtos.Core.Objects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DracoLib.Core
{
    public class Creatures
    {
        private readonly DracoClient dracoClient;

        public Creatures(DracoClient dracoClient)
        {
            this.dracoClient = dracoClient;
        }

        public FCatchingCreature Encounter(string id)
        {
            var request = new FCreatureRequest
            {
                id = id,
                //veryFirst = true
            };

            var response = this.dracoClient.Call(new GamePlayService().StartCatchingCreature(request));

            if (this.dracoClient.Config.Delay > 0)
            {
                var delay = this.dracoClient.Config.Delay * 1500;
                Task.Run(async() => await this.dracoClient.Delay(delay));
            }

            //this.dracoClient.Event("IsArAvailable", "False");

            return response;
        }

        public FCatchCreatureResult Catch(string id, ItemType ball, float quality, bool spin, object options = null)
        {
            return this.dracoClient.Call(new GamePlayService().TryCatchCreature(id, ball, quality, spin));
        }

        public FUpdate Release(List<string> ids)
        {
            return this.dracoClient.Call(new UserCreatureService().ConvertCreaturesToCandies(ids, false));
        }

        public FUserCreatureUpdate Evolve(string id, CreatureType toType)
        {
            return this.dracoClient.Call(new UserCreatureService().EvolveCreature(id, toType));
        }
    }
}