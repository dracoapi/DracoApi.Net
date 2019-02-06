using DracoProtos.Core.Base;
using DracoProtos.Core.Objects;
using System.Collections.Generic;

namespace DracoLib.Core
{
    public class Player : PlayerService
    {
        private readonly DracoClient client;

        public Player(DracoClient dracoClient)
        {
            this.client = dracoClient;
        }

        public new object AcknowledgeNotification(string type)
        {
            return client.Call(client.clientPlayer.AcknowledgeNotification(type));
        }

        public new FAvaUpdate Buy(ItemType type, int amount, int price)
        {
            return client.Call(client.clientPlayer.Buy(type, amount, price));
        }

        public new FAvaUpdate BuyArtifact(ArtifactName name)
        {
            return client.Call(client.clientPlayer.BuyArtifact(name));
        }

        public new FAvaUpdate BuyCoins(string productId, string receipt, string localizedPriceStr, string currency, double localizedPrice)
        {
            return client.Call(client.clientPlayer.BuyCoins(productId, receipt, localizedPriceStr, currency, localizedPrice));
        }

        public new FPurchaseResult BuyFromStore(string productId, string receipt, string localizedPriceStr, string currency, double localizedPrice)
        {
            return client.Call(client.clientPlayer.BuyFromStore(productId, receipt, localizedPriceStr, currency, localizedPrice));
        }

        public new FAvaUpdate BuySaleSet(SaleSetConfig saleSet)
        {
            return client.Call(client.clientPlayer.BuySaleSet(saleSet));
        }

        public new FObeliskDetails ChangeDailyQuest(FClientRequest request, FBuildingRequest building)
        {
            return client.Call(client.clientPlayer.ChangeDailyQuest(request, building));
        }

        public new object ChangeLanguage(string language)
        {
            return client.Call(client.clientPlayer.ChangeLanguage(language));
        }

        public new FMentorshipInfo ChooseMentor(string code)
        {
            return client.Call(client.clientPlayer.ChooseMentor(code));
        }

        public new FUpdate CollectContestRatingAward()
        {
            return client.Call(client.clientPlayer.CollectContestRatingAward());
        }

        public new FActiveObjectsUpdate CollectTribute(int currentUtcOffsetSeconds)
        {
            return client.Call(client.clientPlayer.CollectTribute(currentUtcOffsetSeconds));
        }

        public new FActiveObjectsUpdate GetActiveObjects(int currentUtcOffsetSeconds)
        {
            return client.Call(client.clientPlayer.GetActiveObjects(currentUtcOffsetSeconds));
        }

        public new FArtifactsUpdate GetArtifacts()
        {
            return client.Call(client.clientPlayer.GetArtifacts());
        }

        public new FAvaUpdate GetAvaUpdate()
        {
            return client.Call(client.clientPlayer.GetAvaUpdate());
        }

        public new FCollectorRating GetCollectorGlobalRatingTop()
        {
            return client.Call(client.clientPlayer.GetCollectorGlobalRatingTop());
        }

        public new FCollectorRating GetCollectorRegionalRatingTop()
        {
            return client.Call(client.clientPlayer.GetCollectorRegionalRatingTop());
        }

        public new FContestRating GetContestGlobalRatingTop()
        {
            return client.Call(client.clientPlayer.GetContestGlobalRatingTop());
        }

        public new FContestRating GetContestRegionalRatingTop()
        {
            return client.Call(client.clientPlayer.GetContestRegionalRatingTop());
        }

        public new FDailyQuest GetDailyQuest()
        {
            return client.Call(client.clientPlayer.GetDailyQuest());
        }

        public new FPurchaseResult GetFreeFromStore(string productId)
        {
            return client.Call(client.clientPlayer.GetFreeFromStore(productId));
        }

        public new FJournalUpdate GetJournalUpdate()
        {
            return client.Call(client.clientPlayer.GetJournalUpdate());
        }

        public new FMentorshipInfo GetMentorshipInfo()
        {
            return client.Call(client.clientPlayer.GetMentorshipInfo());
        }

        public new FShopConfig GetShopConfig()
        {
            return client.Call(client.clientPlayer.GetShopConfig());
        }

        public new FShopConfig GetShopConfigIfModified(FShopConfigRequest configRequest)
        {
            return client.Call(client.clientPlayer.GetShopConfigIfModified(configRequest));
        }

        public new FWeeklyQuest GetWeeklyQuest(FClientRequest request)
        {
            return client.Call(client.clientPlayer.GetWeeklyQuest(request));
        }

        public new FWizardBattleRating GetWizardGlobalRatingTop()
        {
            return client.Call(client.clientPlayer.GetWizardGlobalRatingTop());
        }

        public new FWizardBattleRating GetWizardRegionalRatingTop()
        {
            return client.Call(client.clientPlayer.GetWizardRegionalRatingTop());
        }

        public new FMentorshipInfo KickStudent(string studentId)
        {
            return client.Call(client.clientPlayer.KickStudent(studentId));
        }

        public new object MarkAllianceChooseScreenSeen()
        {
            return client.Call(client.clientPlayer.MarkAllianceChooseScreenSeen());
        }

        public new object OnClientStarted()
        {
            return client.Call(client.clientPlayer.OnClientStarted());
        }

        public new FObeliskDetails OpenWeeklyQuestFragment(FClientRequest request, FBuildingRequest building)
        {
            return client.Call(client.clientPlayer.OpenWeeklyQuestFragment(request, building));
        }

        public new FAvaUpdate PurchaseError(string productId, string store, string receiptStr, string localizedPriceStr, string currency, string status)
        {
            return client.Call(client.clientPlayer.PurchaseError(productId, store, receiptStr, localizedPriceStr, currency, status));
        }

        public new FMentorshipInfo RegenerateMentorCode()
        {
            return client.Call(client.clientPlayer.RegenerateMentorCode());
        }

        public new FArtifactsUpdate RemoveArtifact(ArtifactName artifact)
        {
            return client.Call(client.clientPlayer.RemoveArtifact(artifact));
        }

        public new object ResetWeeklyQuest(FClientRequest request)
        {
            return client.Call(client.clientPlayer.ResetWeeklyQuest(request));
        }

        public new FUpdateNicknameResult SaveNickname(string nickname)
        {
            return client.Call(client.clientPlayer.SaveNickname(nickname));
        }

        public new object SaveUserSettings(int avatarAppearanceDetails)
        {
            return client.Call(client.clientPlayer.SaveUserSettings(avatarAppearanceDetails));
        }

        public new FUpdate SelectAlliance(AllianceType alliance, int bonus)
        {
            return client.Call(client.clientPlayer.SelectAlliance(alliance, bonus));
        }

        public new FAvaUpdate SelectBuddy(string creatureId)
        {
            return client.Call(client.clientPlayer.SelectBuddy(creatureId));
        }

        public new object UpdatePushRegistrationId(string deviceId, string providerName, string registrationId)
        {
            return client.Call(client.clientPlayer.UpdatePushRegistrationId(deviceId, providerName, registrationId));
        }

        public new FAvaUpdate UpgradeBag()
        {
            return client.Call(client.clientPlayer.UpgradeBag());
        }

        public new FAvaUpdate UpgradeCreatureStorage()
        {
            return client.Call(client.clientPlayer.UpgradeCreatureStorage());
        }

        public new FArtifactsUpdate WearOnArtifact(ArtifactName artifact, int place)
        {
            return client.Call(client.clientPlayer.WearOnArtifact(artifact, place));
        }

        public new FArtifactsUpdate WearOnArtifacts(List<ArtifactName> artifacts)
        {
            return client.Call(client.clientPlayer.WearOnArtifacts(artifacts));
        }
    }
}
