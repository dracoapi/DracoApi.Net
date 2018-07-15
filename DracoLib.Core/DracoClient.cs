using DracoLib.Core.Exceptions;
using DracoLib.Core.Extensions;
using DracoLib.Core.Providers;
using DracoLib.Core.Text;
using DracoProtos.Core.Base;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DracoLib.Core
{
    public class User
    {
        public string Id { get; set; }
        public string DeviceId { get; set; }
        public string Nickname { get; set; }
        public int Avatar { get; set; }
        public string Login { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    internal class Auth
    {
        public string Name { get; set; }
        public AuthType Type { get; set; }
        public string Reg { get; set; }
        public string ProfileId { get; set; }
        public string TokenId { get; set; }
    }

    /*
     * Exeption on exeptions
     * 
    class DracoError //extends Error
    {
        readonly object details;
        //constructor(message?, details?) {
        //    super(message);
        //    Object.setPrototypeOf(this, DracoError.prototype);
        //    this.details = details;
    }
    */

    public class DracoClient
    {
        public FClientInfo ClientInfo { get; private set; }
        public User User { get; private set; }
        public Fight Fight { get; private set; }
        public Inventory Inventory { get; private set; }
        public Eggs Eggs { get; private set; }
        public Creatures Creatures { get; private set; }

        public string ProtocolVersion { get; private set; }
        public string ClientVersion { get; private set; }
        // Text
        public Strings Strings { get; private set; }

        private RestRequest Request { get; set; }
        private string Proxy { get; set; }
        private string Dcportal { get; set; }
        private bool CheckProtocol { get; set; }
        private Auth Auth { get; set; }
        private sbyte[] ConfigHash { get; set; }

        private Dictionary<string, int> EventsCounter { get; set; } = new Dictionary<string, int>();
        internal int UtcOffset;

        /*
         * Vars c#
         */
        private SerializerContext serializer;
        private RestClient client;
        internal Config Config { get; set; }

        public DracoClient(string proxy = null, Config config = null)
        {
            this.Config = config ?? new Config();

            this.ProtocolVersion = FGameObjects.ProtocolVersion.ToString();
            this.ClientVersion = "12047";
            if (this.Config.CheckProtocol) this.CheckProtocol = this.Config.CheckProtocol;
            if (this.Config.EventsCounter.Any()) this.EventsCounter = this.Config.EventsCounter;
            if (this.Config.UtcOffset > 0)
            {
                this.UtcOffset = this.Config.UtcOffset;
            }
            else
            {
                //Original line GetTimezoneOffset() * 60;  
                this.UtcOffset = (int)TimeZoneInfo.Utc.GetUtcOffset(DateTime.Now).TotalSeconds;// * 60;
            }
            
            this.Proxy = proxy;
            int timeout = 20 * 1000;
            if (this.Config.TimeOut > 0)
            {
                timeout = this.Config.TimeOut;
            }

            this.serializer = new SerializerContext("portal", FGameObjects.CLASSES, FGameObjects.ProtocolVersion);

            this.client = new RestClient("https://us.draconiusgo.com");
            this.client.ClearHandlers();
            if (!string.IsNullOrEmpty(this.Proxy))
                this.client.Proxy = new WebProxy(this.Proxy);
            this.client.AddDefaultHeader("X-Unity-Version", "2017.1.3f1");
            this.client.AddDefaultHeader("Accept", "*/*");
            this.client.AddDefaultHeader("Protocol-Version", this.ProtocolVersion);
            this.client.AddDefaultHeader("Client-Version", this.ClientVersion);
            this.client.AddDefaultHeader("Accept-Language", "en-us");
            this.client.UserAgent = $"DraconiusGO/{this.ClientVersion} CFNetwork/897.15 Darwin/17.5.0";
            this.client.CookieContainer = new CookieContainer();
            //this.client.Encoding = null;
            this.client.Timeout = timeout;

            this.ClientInfo = new FClientInfo
            {
                deviceModel = "iPhone8,1",
                iOsAdvertisingTrackingEnabled = false,
                language = this.Config.Lang,
                platform = "IPhonePlayer",
                platformVersion = "iOS 11.2.6",
                revision = this.ClientVersion,
                screenHeight = 1334,
                screenWidth = 750,
            };

            this.User = new User();
            this.Fight = new Fight(this);
            this.Inventory = new Inventory(this);
            this.Eggs = new Eggs(this);
            this.Creatures = new Creatures(this);

            // Text
            this.Strings = new Strings(this);
        }

        private float GetAccuracy() {
            double random = new Random().Next(20, 65) * 2;
            return (float)Math.Floor(random);
        }
        
        public bool Ping()
        {
            Request = new RestRequest("ping", Method.POST);
            Request.AddHeader("Content-Type", "application /x-www-form-urlencoded");
            var response = client.Execute(Request);

            this.client.AddDefaultParameter("Path", "/", ParameterType.Cookie);
            this.client.AddDefaultParameter("path", "/", ParameterType.Cookie);
            this.client.AddDefaultParameter("domain", ".draconiusgo.com", ParameterType.Cookie);

            return response.StatusCode == HttpStatusCode.OK;
        }

        public object Call(string service, string method, object body)
        {
            var rawbody = this.serializer.Serialize(body);

            Request = new RestRequest("serviceCall", Method.POST);
            Request.AddHeader("Protocol-Version", this.ProtocolVersion);
            Request.AddHeader("Client-Version", this.ClientVersion);
            if (this.Dcportal != null)
            {
                Request.AddHeader("dcportal", this.Dcportal);
            }
            Request.AddParameter("service", service);
            Request.AddParameter("method", method);
            Request.AddFile("args", rawbody, "args.dat", "application/octet-stream");

            var response = this.client.Execute(Request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new DracoError("Invalid status received: " + response.StatusDescription);
            }

            var protocolVersion = response.Headers.ToList().Find(x => x.Name == "Protocol-Version" || x.Name == "protocol-version").Value.ToString();
            var dcportal = response.Headers.ToList().Find(x => x.Name == "dcportal").Value.ToString();
            if (dcportal != null) this.Dcportal = dcportal;
            if (protocolVersion != this.ProtocolVersion && this.CheckProtocol)
            {
                throw new Exception("Incorrect protocol version received: " + protocolVersion);
            }

            var data = this.serializer.Deserialize(response.RawBytes);
            (data ?? "").ToString();
            return data;
        }

        public void Post(string url, object data)
        {
            var _client = new RestClient(url);
            var _request = new RestRequest(Method.POST);
            _request.AddHeader("dcportal", this.Dcportal);
            _request.AddObject(data);

            var response = _client.Execute(_request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Invalid status received: " + response.StatusDescription);
            }
        }

        public void Event(string name, string one = null, string two = null, string three = null)
        {
            int eventCounter = 1;

            if (this.EventsCounter.ContainsKey(name))
                eventCounter = this.EventsCounter[name];

            this.Call("ClientEventService", "onEventWithCounter", new object[]
            {
                name,
                this.User.Id,
                this.ClientInfo,
                eventCounter,
                one,
                two,
                three,
                null,
                null
            });
            this.EventsCounter[name] = eventCounter + 1;
        }

        public FConfig Boot(User userinfo)
        {
            this.User.Id = userinfo.Id;
            this.User.DeviceId = userinfo.DeviceId;
            this.User.Login = (userinfo.Login ?? "DEVICE").ToUpper();
            this.User.Username = userinfo.Username;
            this.User.Password = userinfo.Password;
            this.ClientInfo.iOsVendorIdentifier = userinfo.DeviceId;
            /*foreach (var key in this.ClientInfo) {
                if (this.ClientInfo.GetHashCode(key))
                {
                    this.ClientInfo[key] = userinfo[key];
                }
            }*/
            //this.Event("LoadingScreenPercent", "100");
            //this.Event("Initialized");
            return this.GetConfig();
        }

        private FConfig GetConfig()
        {
            var config = this.Call("AuthService", "getConfig", new object[] { this.ClientInfo.language }) as FConfig;
            this.BuildConfigHash(config);
            return config;
        }

        private sbyte[] BuildConfigHash(FConfig config)
        {
            byte[] buffer = serializer.Serialize(config);
            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(buffer);
            sbyte[] signed = Array.ConvertAll(hash, b => unchecked((sbyte)b));
            this.ConfigHash = signed;
            return this.ConfigHash;
        }

        public async Task<FAuthData> Login()
        {
            if (this.User.Login == "DEVICE")
            {
                this.Auth = new Auth()
                {
                    Name = "DEVICE",
                    Type = AuthType.DEVICE,
                    Reg = "dv",
                    ProfileId = this.User.DeviceId,
                };
            }
            else if (this.User.Login == "GOOGLE")
            {
                this.Auth = new Auth
                {
                    Name = "GOOGLE",
                    Type = AuthType.GOOGLE,
                    Reg = "gl",
                    ProfileId = "?",
                };
                await this.GoogleLogin();
            }            
            else if (this.User.Login == "FACEBOOK")
            {
                throw new FacebookLoginException("Facebook login not implemented.");
            }
            else
            {
                throw new Exception("Unsupported login type: " + this.User.Login);
            }

            //this.Event("TrySingIn", this.Auth.Name);
            var response = this.Call("AuthService", "trySingIn", new object[] { 
                new AuthData() { authType = this.Auth.Type, profileId = this.Auth.ProfileId, tokenId = this.Auth.TokenId },
                this.ClientInfo,
                new FRegistrationInfo(this.Auth.Reg) { email = this.User.Username }
            }) as FAuthData;

            if (response != null && response.info != null)
            {
                this.User.Id = response.info.userId;
                this.User.Avatar = response.info.avatarAppearanceDetails;
            }
            return response;
        }

        private async Task GoogleLogin()
        {
            await Task.Run(async () =>
            {
                //this.Event("StartGoogleSignIn");
                
                var login = await new Google().Login(this.User.Username, this.User.Password);
                if (login == null)
                     throw new DracoError("Unable to login");
                this.Auth.TokenId = login["Auth"]; //["Token"];
                var sub = new CustomJsonWebToken().Decode(this.Auth.TokenId, null, false).Replace("\"", "").Replace("\r\n", "").Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                string profileId = sub[3].Replace(" ", "").Replace("email", "").Replace(",", "");

                /*
                 * ref only
                 foreach (var word in sub)
                    Console.WriteLine(word);
                //*/

                this.Auth.ProfileId = profileId;
            });
        }

        public void Load()
        {
            if (this.User.Avatar == 0) throw new Exception("Please login first.");

            // this.Event("LoadingScreenPercent", "100");
            // this.Event("CreateAvatarByType", "MageMale");
            // this.Event("LoadingScreenPercent", "100");
            // this.Event("AvatarUpdateView", this.user.avatar.toString());
            // this.Event("InitPushNotifications", "False");
        }

        public FNicknameValidationResult ValidateNickName(string nickname, bool takeSuggested = true)
        {
            //this.Event("ValidateNickname", nickname);
            var result = this.Call("AuthService", "validateNickname", new object[] { nickname }) as FNicknameValidationResult;
            if (result == null) return result;
            else if (result.error == FNicknameValidationError.DUPLICATE)
            {
                this.Event("ValidateNicknameError", "DUPLICATE");
                if (takeSuggested) return ValidateNickName(result.suggestedNickname, true);
                else return result;
            }
            else
            {
                return result;
            }
        }

        public void AcceptToS()
        {
            //this.Event("LicenceShown");
            //this.Event("LicenceAccepted");
        }

        public object AcceptLicence(object licence)
        {
            return this.Call("AuthService", "acceptLicence", new object[] { licence });
        }

        public FAuthData Register(string nickname)
        {
            this.User.Nickname = nickname;
            //this.Event("Register", this.Auth.Name, nickname);
            var data = this.Call("AuthService", "register", new object[]
            {
                new AuthData() { authType = this.Auth.Type, profileId = this.Auth.ProfileId, tokenId = this.Auth.TokenId },
                nickname,
                this.ClientInfo,
                new FRegistrationInfo(this.Auth.Reg) { email = this.User.Username }
            }) as FAuthData;

            this.User.Id = data.info.userId;

            //this.Event("ServerAuthSuccess", this.User.Id);

            return data;
        }

        public object GetNews(string lastSeen)
        {
            return this.Call("AuthService", "getNews", new object[] { this.ClientInfo.language, lastSeen });
        }

        //TODO: look this
        /*
        public void generateAvatar(object options) {
        return (options.gender || 0) |          // 0 or 1
               (options.race || 0)     << 1 |   // 0 or 1
               (options.skin || 0)     << 3 |   //
               (options.hair || 0)     << 6 |
               (options.eyes || 0)     << 9 |
               (options.jacket || 0)   << 12 |
               (options.trousers || 0) << 15 |
               (options.shoes || 0)    << 18 |
               (options.backpack || 0) << 21;
        }
        */

        public object SetAvatar(int avatar)
        {
            this.User.Avatar = avatar;
            //this.Event("AvatarPlayerGenderRace", "1", "1");
            //this.Event("AvatarPlayerSubmit", avatar.ToString());
            return this.Call("PlayerService", "saveUserSettings", new object[] { this.User.Avatar });
        }

        public object SelectAlliance(AllianceType alliance, int bonus)
        {
            return this.Call("PlayerService", "selectAlliance", new object[] { "AllianceType", alliance, bonus });
        }

        public object AcknowledgeNotification(string type)
        {
            return this.Call("PlayerService", "acknowledgeNotification", new object[] { type });
        }

        public FUpdate GetMapUpdate(double latitude, double longitude, float horizontalAccuracy, Dictionary<FTile, long> tilescache = null)
        {
            horizontalAccuracy = horizontalAccuracy > 0 ? horizontalAccuracy : this.GetAccuracy();
            tilescache = tilescache ?? new Dictionary<FTile, long>() { };
            var data = this.Call("MapService", "getUpdate", new object[] {
                new FUpdateRequest()
                {
                    clientRequest = new FClientRequest()
                    {
                        time = 0,
                        currentUtcOffsetSeconds = this.UtcOffset,
                        coordsWithAccuracy = new GeoCoordsWithAccuracy()
                        {
                            latitude = latitude,
                            longitude = longitude,
                            horizontalAccuracy = horizontalAccuracy,
                        },
                    },
                    configCacheHash = this.ConfigHash,
                    language = this.ClientInfo.language,
                    clientPlatform = ClientPlatform.IOS,
                    tilesCache =  tilescache,
                }
            }) as FUpdate;

            if (data.items != null)
            {
                var config = data.items.Find(i => i?.GetType() == typeof(FConfig)) as FConfig;
                if (config != null) this.BuildConfigHash(config);
            }
            return data;
        }

        public object UseBuilding(double clientLat, double clientLng, string buildingId, double buildingLat, double buildingLng, string dungeonId)
        {
            return this.Call("MapService", "tryUseBuilding", new object[] {
                new FClientRequest
                {
                    time = 0,
                    currentUtcOffsetSeconds = this.UtcOffset,
                    coordsWithAccuracy = new GeoCoordsWithAccuracy
                    {
                        latitude = clientLat,
                        longitude = clientLng,
                        horizontalAccuracy = this.GetAccuracy(),
                    },
                },

                new FBuildingRequest(buildingId, new GeoCoords { latitude = buildingLat, longitude = buildingLng }, dungeonId)
            });
        }

        public object OpenChest(object chest)
        {
            this.Call("MapService", "startOpeningChest", new object[] { chest });
            return this.Call("MapService", "openChestResult", new object[] { chest });
        }

        public object LeaveDungeon(double latitude, double longitude, float horizontalAccuracy)
        {
            horizontalAccuracy = horizontalAccuracy > 0 ? horizontalAccuracy : this.GetAccuracy();
            return this.Call("MapService", "leaveDungeon", new object[]
            {
                new FClientRequest
                {
                    time = 0,
                    currentUtcOffsetSeconds = this.UtcOffset,
                    coordsWithAccuracy = new GeoCoordsWithAccuracy
                    {
                        latitude = latitude,
                        longitude = longitude,
                        horizontalAccuracy = horizontalAccuracy,
                    },
                },
            });
        }

        // utils
        internal async Task Delay(int ms)
        {
            await Task.Delay(ms);
        }
    }
}

