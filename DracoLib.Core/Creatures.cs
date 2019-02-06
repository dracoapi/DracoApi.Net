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

        public new object AddCreatureToGroup(string id, int group)
        {
            return client.Call(client.clientUserCreature.AddCreatureToGroup(id, group));
        }

        public new FUserCreature ChangeCreatureSpecialization(string creatureId)
        {
            return client.Creatures.ChangeCreatureSpecialization(creatureId);
        }

        public new FUpdate ConvertCreaturesToCandies(List<string> ids, bool sendUpdate)
        {
            return client.Call(client.clientUserCreature.ConvertCreaturesToCandies(ids, sendUpdate));
        }

        public new FUserCreatureUpdate EnhanceCreature(string creatureId)
        {
            return client.Call(client.clientUserCreature.EnhanceCreature(creatureId));
        }

        public new FUserCreatureUpdate EvolveCreature(string creatureId, CreatureType toType)
        {
            return client.Call(client.clientUserCreature.EvolveCreature(creatureId, toType));
        }

        public new FCreadex GetCreadex()
        {
            return client.Call(client.clientUserCreature.GetCreadex());
        }

        public new FUserHatchingInfo GetHatchingInfo()
        {
            return client.Call(client.clientUserCreature.GetHatchingInfo());
        }

        public new FResistModifyDetails GetResistDetails(string creatureId)
        {
            return client.Call(client.clientUserCreature.GetResistDetails(creatureId));
        }

        public new FUserCreaturesList GetUserCreatures()
        {
            return client.Call(client.clientUserCreature.GetUserCreatures());
        }

        public new FResistModifyResult ModifyResist(string creatureId, HashSet<string> sacrificeCreatureIds)
        {
            return client.Call(client.clientUserCreature.ModifyResist(creatureId, sacrificeCreatureIds));
        }

        public new FHatchingResult OpenHatchedEgg(string incubatorId)
        {
            return client.Call(client.clientUserCreature.OpenHatchedEgg(incubatorId));
        }

        public new FHatchingResult OpenHatchedEggWithCreature(string incubatorId, CreatureType selectedCreatureType)
        {
            return client.Call(client.clientUserCreature.OpenHatchedEggWithCreature(incubatorId, selectedCreatureType));
        }

        public new FUserCreature RemasterCreature(string creatureId, bool mainSkill)
        {
            return client.Call(client.clientUserCreature.RemasterCreature(creatureId, mainSkill));
        }

        public new FUserCreature SetCreatureAlias(string creatureId, string alias)
        {
            return client.Call(client.clientUserCreature.SetCreatureAlias(creatureId, alias));
        }

        public new object StartHatchingEgg(string eggId, string incubatorId)
        {
            client.Call(client.clientUserCreature.StartHatchingEgg(eggId, incubatorId));
            return GetHatchingInfo();
        }

        public new object StartHatchingEggInRoost(string eggId, FBuildingRequest roost, int slot)
        {
            return client.Call(client.clientUserCreature.StartHatchingEggInRoost(eggId, roost, slot));
        }

        public new float UsePotion(ItemType type, string creatureId)
        {
            return client.Call(client.clientUserCreature.UsePotion(type, creatureId));
        }
    }
}
