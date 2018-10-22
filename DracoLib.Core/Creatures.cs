using DracoProtos.Core.Base;
using DracoProtos.Core.Objects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DracoLib.Core
{
    public class Creatures : UserCreatureService
    {
        private readonly DracoClient client;

        public Creatures(DracoClient dclient)
        {
            client = dclient;
        }

        public FCatchingCreature StartCatchingCreature(string id)
        {
            var request = new FCreatureRequest
            {
                id = id,
                //veryFirst = true
            };

            var response = client.Call(client.gamePlay.StartCatchingCreature(request));

            //this.dracoClient.Event("IsArAvailable", "False");

            return response;
        }

        public FCatchCreatureResult Catch(string id, ItemType ball, float quality, bool spin, object options = null)
        {
            return client.Call(client.gamePlay.TryCatchCreature(id, ball, quality, spin));
        }

        public new FUserCreatureUpdate EnhanceCreature(string id)
        {
            return client.Call(client.userCreature.EnhanceCreature(id));
        }

        public new FUserCreatureUpdate EvolveCreature(string id, CreatureType toType)
        {
            return client.Call(client.userCreature.EvolveCreature(id, toType));
        }

        public new FUpdate ConvertCreaturesToCandies(List<string> ids, bool sendUpdate)
        {
            return client.Call(client.userCreature.ConvertCreaturesToCandies(ids, sendUpdate));
        }

        public new object AddCreatureToGroup(string id, int group)
        {
            return client.Call(client.userCreature.AddCreatureToGroup(id, group));
        }

        public new FUserCreature SetCreatureAlias(string id, string alias)
        {
            return client.Call(client.userCreature.SetCreatureAlias(id, alias));
        }

        public new FUserCreature RemasterCreature(string id, bool mainSkill)
        {
            return client.Call(client.userCreature.RemasterCreature(id, mainSkill));
        }
    }
}
