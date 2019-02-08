using DracoProtos.Core.Base;
using DracoProtos.Core.Objects;
using System.Collections.Generic;

namespace DracoLib.Core
{
    public class Auth : AuthService
    {
        private readonly DracoClient client;

        public Auth(DracoClient dracoClient)
        {
            this.client = dracoClient;
        }

        public new FUserInfo AcceptLicence(int licenseVersion)
        {
            return client.Call(client.clientAuth.AcceptLicence(licenseVersion));
        }

        public new FAuthData DevSingIn(string login, bool validateNickname, bool asDevice)
        {
            return client.Call(client.clientAuth.DevSingIn(login, validateNickname, asDevice));
        }

        public new FConfig GetConfig(string language)
        {
            client.FConfig = client.Call(client.clientAuth.GetConfig(language));
            return client.FConfig;
        }

        public new FNewsArticle GetNews(string locale, string lastSeen)
        {
            return client.Call(client.clientAuth.GetNews(locale, lastSeen));
        }

        public new FNewsArticle GetOffers(string locale, string seenNews)
        {
            return client.Call(client.clientAuth.GetOffers(locale, seenNews));
        }

        public new FTips GetTips(string locale)
        {
            return client.Call(client.clientAuth.GetTips(locale));
        }

        public new FAuthData LinkTo(AuthData authData, FClientInfo clientInfo, FRegistrationInfo regInfo, bool force)
        {
            return client.Call(client.clientAuth.LinkTo(authData, clientInfo, regInfo, force));
        }

        public new FTips MarkTip(string locale, bool value)
        {
            return client.Call(client.clientAuth.MarkTip(locale, value));
        }

        public new FAuthData Register(AuthData authData, string nickname, FClientInfo clientInfo, FRegistrationInfo regInfo)
        {
            return client.Call(client.clientAuth.Register(authData, nickname, clientInfo, regInfo));
        }

        public new FAuthData TrySingIn(AuthData authData, FClientInfo clientInfo, FRegistrationInfo regInfo)
        {
            return client.Call(client.clientAuth.TrySingIn(authData, clientInfo, regInfo));
        }

        public FNicknameValidationResult ValidateNickname(string nickname, bool takeSuggested = true)
        {
            var result = client.Call(client.clientAuth.ValidateNickname(nickname));

            if (result == null) return result;

            if (result.error == FNicknameValidationError.DUPLICATE)
            {
                if (takeSuggested)
                {
                    return ValidateNickname(result.suggestedNickname, true);
                }

                return result;
            }
            else
            {
                return result;
            }
        }
    }
}
