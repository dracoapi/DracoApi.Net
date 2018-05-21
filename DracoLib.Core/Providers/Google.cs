using System.Collections.Generic;
using System.Threading.Tasks;
using DracoLib.Core.Exceptions;
using GPSOAuthSharp;

namespace DracoLib.Core.Providers
{
    public class Google
    {
        const string GOOGLE_LOGIN_ANDROID_ID = "9774d56d682e549c";
        const string GOOGLE_LOGIN_SERVICE = "audience:server:client_id:142464098123-ihdru24fmqkb6pgdg5n3v6an5b4a664i.apps.googleusercontent.com";
        const string GOOGLE_LOGIN_APP = "net.elyland.DraconiusGO";
        const string GOOGLE_LOGIN_CLIENT_SIG = "fc0e7d31361f6c8722135af1024355d5a86b0689";

        public async Task<Dictionary<string, string>> Login(string username, string password)
        {
            var googleClient = new GPSOAuthClient(username, password);
            var masterLoginResponse = await googleClient.PerformMasterLogin();

            if (masterLoginResponse.ContainsKey("Error"))
            {
                if (masterLoginResponse["Error"].Equals("NeedsBrowser"))
                    throw new GoogleLoginException($"You have to log into an browser with the email '{username}'.");

                throw new GoogleLoginException($"Google returned an error message: '{masterLoginResponse["Error"]}'");
            }
            if (!masterLoginResponse.ContainsKey("Token"))
            {
                throw new GoogleLoginException("Token was missing from master login response.");
            }
            var oauthResponse = await googleClient.PerformOAuth(masterLoginResponse["Token"], GOOGLE_LOGIN_SERVICE,
                GOOGLE_LOGIN_APP, GOOGLE_LOGIN_CLIENT_SIG);
            if (!oauthResponse.ContainsKey("Auth"))
            {
                throw new GoogleLoginException("Auth token was missing from oauth login response.");
            }

            return oauthResponse;
        }
    }
}
