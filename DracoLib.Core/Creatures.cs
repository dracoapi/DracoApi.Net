using DracoProtos.Core.Enums;
using DracoProtos.Core.Objects;
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
            var response = this.dracoClient.Call("GamePlayService", "startCatchingCreature", new object[]
                {
                    new FCreatureRequest
                    {
                        id = id,
                        //veryFirst = true
                    },
                }) as FCatchingCreature;

            if (this.dracoClient.Config.Delay > 0)
            {
                var delay = this.dracoClient.Config.Delay * 1500;
                Task.Run(async() => await this.dracoClient.Delay(delay));
            }

            //this.dracoClient.Event("IsArAvailable", "False");

            return response;
        }

        public object Catch(string id, int ball, int quality, bool spin, object options)
        {
            return this.dracoClient.Call("GamePlayService", "tryCatchCreature", new object[] { id, ball, quality, spin });
        }

        public FUpdate Release(string[] ids)
        {
            //if (!Array.isArray(ids)) ids = [ids];
            return this.dracoClient.Call("UserCreatureService", "convertCreaturesToCandies", new object[] { ids, false }) as FUpdate;
        }

        public object Evolve(string id, CreatureType toType)
        {
            return this.dracoClient.Call("UserCreatureService", "evolveCreature", new object[] { id, toType });
        }
    }
}