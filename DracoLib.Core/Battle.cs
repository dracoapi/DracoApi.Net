using DracoProtos.Core.Base;
using DracoProtos.Core.Objects;
using System;
using System.Collections.Generic;
using System.Text;

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
            return client.Call(client.battle.AddDefenderToArena(buildingRequest, clientRequest, creatureId));
        }

        public new FFightUpdate AttackArena(FAttackArenaRequest attackArenaRequest)
        {
            return client.Call(client.battle.AttackArena(attackArenaRequest));
        }
        
        public new FWizardBattleInfo BuyExtraWizardBattles(string extraPackName)
        {
            return client.Call(client.battle.BuyExtraWizardBattles(extraPackName));
        }

        public new FFightUpdate FindWizardBattle(List<string>  selectedCreatures)
        {
            return client.Call(client.battle.FindWizardBattle(selectedCreatures));
        }

        public new FUserCreaturesList GetPossibleArenaAttackers()
        {
            return client.Call(client.battle.GetPossibleArenaAttackers());
        }

        public new FUserCreaturesList GetPossibleArenaAttackersV2(List<string> selectedCreatures)
        {
            return client.Call(client.battle.GetPossibleArenaAttackersV2(selectedCreatures));
        }

        public new FUserCreature GetPossibleEncounterAttacker(ElementType elementType)
        {
            return client.Call(client.battle.GetPossibleEncounterAttacker(elementType));
        }

        public new FUserCreaturesList GetPossibleWizardAttackers()
        {
            return client.Call(client.battle.GetPossibleWizardAttackers());
        }

        public new FWizardBattleInfo GetWizardBattleInfo()
        {
            return client.Call(client.battle.GetWizardBattleInfo());
        }

        public new bool StartedWizardBattleV2(string battleId)
        {
            return client.Call(client.battle.StartedWizardBattleV2(battleId));
        }

        public new FUpdate UpdateBattleDetails(FBuildingRequest buildingRequest, FClientRequest clientRequest, FFightRequest fightRequest)
        {
            return client.Call(client.battle.UpdateBattleDetails(buildingRequest, clientRequest, fightRequest));
        }

        public new FWizardBattleResult UpdateWizardBattle(FFightRequest fightRequest)
        {
            return client.Call(client.battle.UpdateWizardBattle(fightRequest));
        }
    }
}
