using DracoLib.Core.Exceptions;
using DracoLib.Core.Extensions;
using DracoLib.Core.Providers;
using DracoLib.Core.Utils;
using DracoProtos.Core.Classes;
using DracoProtos.Core.Enums;
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

    public class Auth
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
        public FClientInfo ClientInfo { get; set; }
        public User User { get; set; }
        public Fight Fight { get; set; }
        public Inventory Inventory { get; set; }
        public Eggs Eggs { get; set; }
        public Creatures Creatures { get; set; }

        public string ProtocolVersion;
        public string ClientVersion;

        private object Request { get; set; }
        private string Proxy { get; set; }
        private string Dcportal { get; set; }
        private bool CheckProtocol { get; set; } = true;
        private Auth Auth { get; set; }
        private sbyte ConfigHash { get; set; }

        public Dictionary<string, int> EventsCounter { get; set; } = new Dictionary<string, int>();
        public int UtcOffset;

        /*
         * Vars c#
         */
        private SerializerContext serializer;
        private RestClient client;

        public DracoClient(string proxy = null)
        {
            this.ProtocolVersion = FGameObjects.ProtocolVersion.ToString() ?? "389771870";
            this.ClientVersion = FGameObjects.ClientVersion.ToString() ?? "11808";
            /*if (config.CheckProtocol) this.CheckProtocol = config.CheckProtocol;
            if (config.EventsCounter.Count() > 0) this.EventsCounter = config.EventsCounter;
            if (config.UtcOffset > 0)
            {
                this.UtcOffset = config.UtcOffset;
            }
            else
            {
                //TODO: look this 
                //this.utcOffset = -new DateTime().GetTimezoneOffset() * 60;            
            }
            */
            
            this.Proxy = proxy;
            int timeout = 20 * 1000;
            //if (config.TimeOut > 0)
            //{
            //    timeout = config.TimeOut;
            //}

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
            this.client.Encoding = null;
            this.client.Timeout = timeout;

            this.ClientInfo = new FClientInfo
            {
                deviceModel = "iPhone8,1",
                iOsAdvertisingTrackingEnabled = false,
                language = "English",// config.Lang ?? "English",
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
        }

        //TODO: look this
        /*
        private float GetAccuracy() {
            return [20, 65][Math.floor(Math.random() * 2)];
        }
        */

        public bool Ping()
        {
            var request = new RestRequest("ping", Method.POST);
            request.AddHeader("Content-Type", "application /x-www-form-urlencoded");
            var response = client.Execute(request);

            this.client.AddDefaultParameter("Path", "/", ParameterType.Cookie);
            this.client.AddDefaultParameter("path", "/", ParameterType.Cookie);
            this.client.AddDefaultParameter("domain", ".draconiusgo.com", ParameterType.Cookie);

            return response.StatusCode == HttpStatusCode.OK;
        }

        public object Call(string service, string method, object body)
        {
            var rawbody = serializer.Serialize(body);

            var request = new RestRequest("serviceCall", Method.POST);
            request.AddHeader("Protocol-Version", this.ProtocolVersion.ToString());
            if (this.Dcportal != null)
            {
                request.AddHeader("dcportal", this.Dcportal);
            }
            request.AddParameter("service", service);
            request.AddParameter("method", method);
            request.AddFile("args", rawbody, "args.dat", "application/octet-stream");

            var response = client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Invalid status received: " + response.StatusDescription);
            }

            var protocolVersion = response.Headers.ToList().Find(x => x.Name == "Protocol-Version" || x.Name == "protocol-version").Value.ToString();
            var dcportal = response.Headers.ToList().Find(x => x.Name == "dcportal").Value.ToString();
            if (dcportal != null) this.Dcportal = dcportal;
            if (protocolVersion != serializer.protocolVersion.ToString())
            {
                throw new Exception("Incorrect protocol version received: " + protocolVersion);
            }

            var data = serializer.Deserialize(response.RawBytes);
            (data ?? "").ToString();

            if ((int)response.StatusCode > 300)
            {
                try
                {
                    data = serializer.Deserialize(response.RawBytes);
                }
                catch (Exception ex)
                {
                    throw new DracoError("Error from server: " + response.StatusCode + " - " + ex);
                }
            }

            return data;
        }

        public void Event(string name, string one = null, string two = null, string three = null)
        {
            int eventCounter = 1;

            if (this.EventsCounter[name] > 0)
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

        public FConfig Boot(User clientinfo)
        {
            this.User.Id = clientinfo.Id;
            this.User.DeviceId = clientinfo.DeviceId ?? DracoUtils.GenerateDeviceId();
            this.User.Login = (clientinfo.Login ?? "DEVICE").ToUpper();
            this.User.Username = clientinfo.Username;
            this.User.Password = clientinfo.Password;
            this.ClientInfo.iOsVendorIdentifier = clientinfo.DeviceId ?? DracoUtils.GenerateDeviceId();
            /*foreach (var key in new List<string> { clientinfo }) {
                if (this.ClientInfo.GetHashCode(key))
                {
                    this.ClientInfo[key] = clientinfo[key];
                }
            }*/
            //this.Event("LoadingScreenPercent", "100");
            //this.Event("Initialized");
            return this.GetConfig();
        }

        public FConfig GetConfig()
        {
            var config = this.Call("AuthService", "getConfig", new object[] { this.ClientInfo.language }) as FConfig;
            this.BuildConfigHash(config);
            return config;
        }

        public sbyte BuildConfigHash(FConfig config)
        {
            byte[] buffer = serializer.Serialize(config);
            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(buffer);
            this.ConfigHash = (sbyte)hash.FirstOrDefault();
            return this.ConfigHash;
        }

        public FAuthData Login()
        {
            if (this.User.Login == "DEVICE")
            {
                this.Auth = new Auth()
                {
                    Name = "DEVICE",
                    Type = AuthType.DEVICE,
                    Reg = "dv",
                    ProfileId = this.User.DeviceId
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
                this.GoogleLogin();
            }
            else if (this.User.Login == "FACEBOOK")
            {
                throw new FacebookLoginException("Facebook login not implemented.");
            }
            else
            {
                throw new Exception("Unsupported login type: " + this.User.Login);
            }
            // await this.event('TrySingIn', this.auth.name);
            var response = this.Call("AuthService", "trySingIn", new object[]
            {
                new AuthData() { authType = this.Auth.Type, profileId = this.Auth.ProfileId, tokenId = this.Auth.TokenId },
                this.ClientInfo,
                new FRegistrationInfo () { email = this.User.Username, regType = this.Auth.Reg },
            }) as FAuthData;

            if (response != null && response.info != null)
            {
                this.User.Id = response.info.userId;
                this.User.Avatar = response.info.avatarAppearanceDetails;
            }
            return response;
        }

        public async void GoogleLogin()
        {
            // await this.event('StartGoogleSignIn');
            var login = new Google();
            this.Auth.TokenId = await login.Login(this.User.Username, this.User.Password);
            var decoder = new CustomJsonWebToken();
            this.Auth.ProfileId = decoder.Decode(this.Auth.TokenId, null, true);
        }

        public void Load()
        {
            if (this.User.Avatar == 0 ) throw new Exception("Please login first.");

            // await this.event('LoadingScreenPercent', '100');
            // await this.event('CreateAvatarByType', 'MageMale');
            // await this.event('LoadingScreenPercent', '100');
            // await this.event('AvatarUpdateView', this.user.avatar.toString());
            // await this.event('InitPushNotifications', 'False');
        }

        public string ValidateNickName(string nickname, bool takeSuggested = true)
        {
            //this.Event("ValidateNickname", nickname);
            var result = this.Call("AuthService", "validateNickname", new object[] { nickname }) as FNicknameValidationResult;
            if (result == null) return nickname;
            else if (result.error == FNicknameValidationError.DUPLICATE)
            {
                this.Event("ValidateNicknameError", "DUPLICATE");
                if (takeSuggested) return ValidateNickName(result.suggestedNickname, true);
                else return null;
            }
            else
            {
                return null;
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
                new FRegistrationInfo() { regType = this.Auth.Reg },
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
            return this.Call("PlayerService", "selectAlliance", new object[] {"AllianceType", alliance, bonus });
        }

        public object AcknowledgeNotification(string type)
        {
            return this.Call("PlayerService", "acknowledgeNotification", new object[] { type });
        }

        public FUpdate GetMapUpdate(float latitude, float longitude, float horizontalAccuracy, Dictionary<FTile, long> tilescache)
        {
            //TODO: look this
            //horizontalAccuracy = horizontalAccuracy; || this.getAccuracy();
            tilescache = tilescache ?? new Dictionary<FTile, long>();
            var data = this.Call("MapService", "getUpdate", new object[] {
                new FUpdateRequest()
                {
                    clientRequest = new FClientRequest()
                    {
                        time = 0,
                        //TODO: look this
                        currentUtcOffsetSeconds = /*this.utcOffset,*/ 7200, 
                        coordsWithAccuracy = new GeoCoordsWithAccuracy()
                        {
                            latitude = latitude,
                            longitude = longitude,
                            horizontalAccuracy = horizontalAccuracy,
                        },
                    },
                    configCacheHash = this.ConfigHash,
                    clientPlatform = ClientPlatform.IOS,
                    tilesCache = tilescache,
                }
            }) as FUpdate;

            if (data.items != null)
            {
                var config = data.items.Select(i => i.GetType() == typeof(FConfig)) as FConfig;
                if (config != null) this.BuildConfigHash(config);
            }
            return data;
        }

        public object UseBuilding(float clientLat, float clientLng, string buildingId, float buildingLat, float buildingLng)
        {
            return this.Call("MapService", "tryUseBuilding", new object[]
            {
                new FClientRequest
                {
                    time = 0,
                    currentUtcOffsetSeconds = this.UtcOffset,
                    coordsWithAccuracy = new GeoCoordsWithAccuracy
                    {
                        latitude = clientLat,
                        longitude = clientLng,
                        //TODO: look this
                        //horizontalAccuracy = this.getAccuracy(),
                    },
                },

                new FBuildingRequest
                {
                    coords =  new GeoCoords
                    {
                        latitude = buildingLat,
                        longitude = buildingLng,
                },
                    id = buildingId,
                },
            });
        }

        public object OpenChest(FChest chest)
        {
            this.Call("MapService", "startOpeningChest", new object[] { chest });
            return this.Call("MapService", "openChestResult", new object[] { chest });
        }

        public object LeaveDungeon(float latitude, float longitude, float horizontalAccuracy)
        {
            //TODO: look this
            //horizontalAccuracy = horizontalAccuracy || this.getAccuracy();
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
                            //TODO: look this
                            //horizontalAccuracy = horizontalAccuracy,
                        },
                    },
                });
        }

        // utils
        public async Task Delay(int ms)
        {
            await Task.Delay(ms);
        }
    }
}

