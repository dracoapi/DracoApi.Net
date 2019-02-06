using DracoProtos.Core.Base;
using DracoProtos.Core.Objects;
using System.Collections.Generic;

namespace DracoLib.Core
{
    public class DevMode : DevModeService
    {
        private readonly DracoClient client;

        public DevMode(DracoClient dracoClient)
        {
            this.client = dracoClient;
        }

        public new FAvaUpdate AddRunes()
        {
            return client.Call(client.clientDevMode.AddRunes());
        }

        public new object AddAllCreatures()
        {
            return client.Call(client.clientDevMode.AddAllCreatures());
        }

        public new object AddAllLogEvents()
        {
            return client.Call(client.clientDevMode.AddAllLogEvents());
        }

        public new object AddArenaExperience(FBuildingRequest building, int layer, int expToAdd)
        {
            return client.Call(client.clientDevMode.AddArenaExperience(building, layer, expToAdd));
        }

        public new FAvaUpdate AddArtifact(ArtifactName artifactName)
        {
            return client.Call(client.clientDevMode.AddArtifact(artifactName));
        }

        public new FAvaUpdate AddBuff(string buffName, int value)
        {
            return client.Call(client.clientDevMode.AddBuff(buffName, value));
        }

        public new FAvaUpdate AddCandies(int amount)
        {
            return client.Call(client.clientDevMode.AddCandies(amount));
        }

        public new FAvaUpdate AddCoins(int amount)
        {
            return client.Call(client.clientDevMode.AddCoins(amount));
        }

        public new object AddContestCompletedEvent(int contestPlace, float contestScore)
        {
            return client.Call(client.clientDevMode.AddContestCompletedEvent(contestPlace, contestScore));
        }

        public new object AddContestRatingAward(int month, int place, int dust, int runes)
        {
            return client.Call(client.clientDevMode.AddContestRatingAward(month, place, dust, runes));
        }

        public new FAvaUpdate AddDust(int amount)
        {
            return client.Call(client.clientDevMode.AddDust(amount));
        }

        public new object AddExistingCreatures()
        {
            return client.Call(client.clientDevMode.AddExistingCreatures());
        }

        public new FUpdate AddExp(int amount)
        {
            return client.Call(client.clientDevMode.AddExp(amount));
        }

        public new FAvaUpdate AddFood()
        {
            return client.Call(client.clientDevMode.AddFood());
        }

        public new FAvaUpdate AddItems(ItemType itemType, int amount)
        {
            return client.Call(client.clientDevMode.AddItems(itemType, amount));
        }

        public new FAvaUpdate AddLure()
        {
            return client.Call(client.clientDevMode.AddLure());
        }

        public new object AddMigrationWorker(string migrationWorkerClass)
        {
            return client.Call(client.clientDevMode.AddMigrationWorker(migrationWorkerClass));
        }

        public new FUserCreature AddRandomCreature()
        {
            return client.Call(client.clientDevMode.AddRandomCreature());
        }

        public new FAvaUpdate AddRandomEgg(ItemType eggType)
        {
            return client.Call(client.clientDevMode.AddRandomEgg(eggType));
        }

        public new FAvaUpdate AddRecipes()
        {
            return client.Call(client.clientDevMode.AddRecipes());
        }

        public new FHatchingResult AddReferralDragon()
        {
            return client.Call(client.clientDevMode.AddReferralDragon());
        }

        public new object AddWizardBattleRatingEvent(bool victory, float rewardPercent)
        {
            return client.Call(client.clientDevMode.AddWizardBattleRatingEvent(victory, rewardPercent));
        }

        public new FUpdate CastRecipe(RecipeType recipeType)
        {
            return client.Call(client.clientDevMode.CastRecipe(recipeType));
        }

        public new FUpdate CastSpell(SpellType spell)
        {
            return client.Call(client.clientDevMode.CastSpell(spell));
        }

        public new int CatchMonsterLimitCount()
        {
            return client.Call(client.clientDevMode.CatchMonsterLimitCount());
        }

        public new FQuestUpdate ChangeDailyQuest()
        {
            return client.Call(client.clientDevMode.ChangeDailyQuest());
        }

        public new FUserCreature DamageCreature(string id, int dmg)
        {
            return client.Call(client.clientDevMode.DamageCreature(id, dmg));
        }

        public new object DeleteUser(string userId)
        {
            return client.Call(client.clientDevMode.DeleteUser(userId));
        }

        public new object ExecuteCql(string cql)
        {
            return client.Call(client.clientDevMode.ExecuteCql(cql));
        }

        public new FAvaUpdate GetAvaUpdate()
        {
            return client.Call(client.clientDevMode.GetAvaUpdate());
        }

        public new double GetCurrentVisionRadius()
        {
            return client.Call(client.clientDevMode.GetCurrentVisionRadius());
        }

        public new byte[] GetDefaultConfigBytes()
        {
            return client.Call(client.clientDevMode.GetDefaultConfigBytes());
        }

        public new long? GetPushNotification(string userId, string type)
        {
            return client.Call(client.clientDevMode.GetPushNotification(userId, type));
        }

        public new int GetSuspicionPoints()
        {
            return client.Call(client.clientDevMode.GetSuspicionPoints());
        }

        public new List<string> GetSuspiciousList()
        {
            return client.Call(client.clientDevMode.GetSuspiciousList());
        }

        public new object GiveInstantItem(InstantUseItem itemType, long value)
        {
            return client.Call(client.clientDevMode.GiveInstantItem(itemType, value));
        }

        public new object GiveLootItem(ItemType itemType)
        {
            return client.Call(client.clientDevMode.GiveLootItem(itemType));
        }

        public new object KillCreature(string creatureId)
        {
            return client.Call(client.clientDevMode.KillCreature(creatureId));
        }

        public new FUpdate LevelUp(int levels)
        {
            return client.Call(client.clientDevMode.LevelUp(levels));
        }

        public new FWeeklyQuestFragment OpenMapFragment()
        {
            return client.Call(client.clientDevMode.OpenMapFragment());
        }

        public new long PauseAndReset(string testName)
        {
            return client.Call(client.clientDevMode.PauseAndReset(testName));
        }

        public new object ProcessNotificationsNow()
        {
            return client.Call(client.clientDevMode.ProcessNotificationsNow());
        }

        public new object ResetMigrationWorkers()
        {
            return client.Call(client.clientDevMode.ResetMigrationWorkers());
        }

        public new object ResetRemasterCooldown()
        {
            return client.Call(client.clientDevMode.ResetRemasterCooldown());
        }

        public new int Resume()
        {
            return client.Call(client.clientDevMode.Resume());
        }

        public new object RunMigration()
        {
            return client.Call(client.clientDevMode.RunMigration());
        }

        public new object SendPushNotification(long delay)
        {
            return client.Call(client.clientDevMode.SendPushNotification(delay));
        }

        public new FArenaDetails SetArenaAllianceType(FBuildingRequest building, int layer, AllianceType type)
        {
            return client.Call(client.clientDevMode.SetArenaAllianceType(building, layer, type));
        }

        public new long TimeJump(long millis)
        {
            return client.Call(client.clientDevMode.TimeJump(millis));
        }

        public new object UploadConfig(byte[] bytes)
        {
            return client.Call(client.clientDevMode.UploadConfig(bytes));
        }

        public new FUpdate WeeklyQuestDig()
        {
            return client.Call(client.clientDevMode.WeeklyQuestDig());
        }
    }
}
