using DracoProtos.Core.Base;
using DracoProtos.Core.Objects;
using System.Collections.Generic;

namespace DracoLib.Core
{
    public class Battle : BattleService
    {
        private readonly DracoClient client;

        public Battle(DracoClient dclient)
        {
            client = dclient;
        }

        public new FUpdate AddDefenderToArena(FBuildingRequest buildingRequest, FClientRequest clientRequest, string creatureId)
        {
            return client.Call(client.clientBattle.AddDefenderToArena(buildingRequest, clientRequest, creatureId));
        }

        public new FFightUpdate AttackArena(FAttackArenaRequest attackArenaRequest)
        {
            return client.Call(client.clientBattle.AttackArena(attackArenaRequest));
        }
        
        public new FWizardBattleInfo BuyExtraWizardBattles(string extraPackName)
        {
            return client.Call(client.clientBattle.BuyExtraWizardBattles(extraPackName));
        }

        public new FFightUpdate FindWizardBattle(List<string>  selectedCreatures)
        {
            return client.Call(client.clientBattle.FindWizardBattle(selectedCreatures));
        }

        public new FUserCreaturesList GetPossibleArenaAttackers()
        {
            return client.Call(client.clientBattle.GetPossibleArenaAttackers());
        }

        public new FUserCreaturesList GetPossibleArenaAttackersV2(List<string> selectedCreatures)
        {
            return client.Call(client.clientBattle.GetPossibleArenaAttackersV2(selectedCreatures));
        }

        public new FUserCreature GetPossibleEncounterAttacker(ElementType elementType)
        {
            return client.Call(client.clientBattle.GetPossibleEncounterAttacker(elementType));
        }

        public new FUserCreaturesList GetPossibleWizardAttackers()
        {
            return client.Call(client.clientBattle.GetPossibleWizardAttackers());
        }

        public new FWizardBattleInfo GetWizardBattleInfo()
        {
            return client.Call(client.clientBattle.GetWizardBattleInfo());
        }

        public new bool StartedWizardBattleV2(string battleId)
        {
            return client.Call(client.clientBattle.StartedWizardBattleV2(battleId));
        }

        public new FUpdate UpdateBattleDetails(FBuildingRequest buildingRequest, FClientRequest clientRequest, FFightRequest fightRequest)
        {
            return client.Call(client.clientBattle.UpdateBattleDetails(buildingRequest, clientRequest, fightRequest));
        }

        public new FWizardBattleResult UpdateWizardBattle(FFightRequest fightRequest)
        {
            return client.Call(client.clientBattle.UpdateWizardBattle(fightRequest));
        }
    }
}
