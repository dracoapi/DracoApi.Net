using DracoProtos.Core.Enums;
using DracoProtos.Core.Objects;
using System;

namespace DracoLib.Core
{
    public class Creatures
    {
        private DracoClient dracoClient;

        public Creatures(DracoClient dracoClient)
        {
            this.dracoClient = dracoClient;
        }

        public object Encounter(string id)
        {
            var response = this.dracoClient.Call("GamePlayService", "startCatchingCreature", new object[]
                {
                    new FCreatureRequest
                    {
                        id = id,
                        //veryFirst = true
                    },
                }) as FCatchingCreature;

            //if (this.dracoClient.Config.Delay > 0) this.dracoClient.Config.Delay = 1000 + Math.Round(1000 * 1500) * 1500;

            //await this.dracoClient.delay(options.delay);
            // await this.event("IsArAvailable", "False");

            return response;
        }

        public object Catch(string id, int ball, int quality, bool spin, object options)
        {
            return this.dracoClient.Call("GamePlayService", "tryCatchCreature", new object[] { id, ball, quality, spin });
        }

        public object Release(string[] ids)//: Promise<objects.FUpdate> 
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