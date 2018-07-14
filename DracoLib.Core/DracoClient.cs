using System.Net.Http;
using System.Net.Http.Headers;
using DracoLib.Core.Exceptions;
using DracoLib.Core.Extensions;
using DracoLib.Core.Providers;
using DracoLib.Core.Text;
using DracoProtos.Core.Classes;
using DracoProtos.Core.Enums;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;
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

        private HttpRequestHeader Request { get; set; }
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
        private HttpClient client;
        private HttpClientHandler clientHandler;
        internal Config Config { get; set; }

        public DracoClient(string proxy = null, Config config = null)
        {
            this.Config = config ?? new Config();

            this.ProtocolVersion = FGameObjects.ProtocolVersion.ToString(); //Use vars "389771870";
            this.ClientVersion = FGameObjects.ClientVersion.ToString(); //Use vars "11808";
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
            this.clientHandler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                AllowAutoRedirect = true,
                UseProxy = !string.IsNullOrEmpty(this.Proxy),
                Proxy = (string.IsNullOrEmpty(this.Proxy)) ? null : new WebProxy(this.Proxy),
                UseCookies = true,
                CookieContainer = new CookieContainer()
            };
            //this.clientHandler.;

            clientHandler.CookieContainer.Add(new Uri("https://us.draconiusgo.com"), new Cookie("path", "/"));
            clientHandler.CookieContainer.Add(new Uri("https://us.draconiusgo.com"), new Cookie("Path", "/"));
            clientHandler.CookieContainer.Add(new Uri("https://us.draconiusgo.com"), new Cookie("domain", ".draconiusgo.com"));

            this.client = new HttpClient(clientHandler)
            {
                BaseAddress = new Uri("https://us.draconiusgo.com")
            };

            this.client.DefaultRequestHeaders.Clear();
            this.client.DefaultRequestHeaders.Host = "us.draconiusgo.com";
            this.client.DefaultRequestHeaders.TryAddWithoutValidation("X-Unity-Version", "2017.1.3f1");
            this.client.DefaultRequestHeaders.Accept.TryParseAdd("*/*");
            this.client.DefaultRequestHeaders.Add("Protocol-Version", this.ProtocolVersion);
            this.client.DefaultRequestHeaders.Add("Client-Version", this.ClientVersion);
            this.client.DefaultRequestHeaders.AcceptLanguage.TryParseAdd("en-us");
            this.client.DefaultRequestHeaders.AcceptEncoding.TryParseAdd("gzip");
            this.client.DefaultRequestHeaders.UserAgent.TryParseAdd($"DraconiusGO/{this.ClientVersion} CFNetwork/897.15 Darwin/17.5.0");
            this.client.Timeout.Add(TimeSpan.FromMilliseconds(timeout));

            /*encoding: null,
                        gzip: true,
                        jar: cookies,
                        simple: false,
                        resolveWithFullResponse: true,
            timeout,*/

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

        private float GetAccuracy()
        {
            double random = new Random().Next(20, 65) * 2;
            return (float)Math.Floor(random);
        }

        public bool Ping()
        {
            var content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("", "")
            });
            var response = client.PostAsync("ping", content).Result;
            return response.StatusCode == HttpStatusCode.OK;
        }

        public object Call(string service, string method, object body)
        {
            var multiContent = new MultipartFormDataContent();
            var strService = new StringContent(service);
            strService.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
            strService.Headers.Add("charset", "utf-8");
            multiContent.Add(strService, "service");
            var strmethod = new StringContent(service);
            strmethod.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
            strmethod.Headers.Add("charset", "utf-8");
            multiContent.Add(strmethod, "method");
            var filecontent = new ByteArrayContent(serializer.Serialize(body));
            filecontent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            multiContent.Add(filecontent, "args", "args.dat");

            if (this.Dcportal != null)
            {
                multiContent.Headers.Add("dcportal", this.Dcportal);
            }


            var response = client.PostAsync("serviceCall", multiContent).Result;

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new DracoError("Invalid status received: " + response.StatusCode);
            }

            var protocolVersion = response.Headers.ToList().Find(x => x.Key == "Protocol-Version" || x.Key == "protocol-version").Value.ToString();
            var dcportal = response.Headers.ToList().Find(x => x.Key == "dcportal").Value.ToString();
            if (dcportal != null) this.Dcportal = dcportal;
            if (protocolVersion != this.ProtocolVersion && this.CheckProtocol)
            {
                throw new Exception("Incorrect protocol version received: " + protocolVersion);
            }

            var data = serializer.Deserialize(response.Content.ReadAsByteArrayAsync().Result);
            (data ?? "").ToString();
            return data;
        }

        public void Post(string url, object data)
        {
            var arrData = new List<KeyValuePair<string, string>>();
            foreach (var element in data.GetType().GetProperties())
            {
                arrData.Add(new KeyValuePair<string, string>(element.Name, (string)element.GetValue(data)));
            }
            var content = new FormUrlEncodedContent(arrData);
            content.Headers.Add("dcportal", this.Dcportal);
            var response = client.PostAsync(url, content).Result;

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Invalid status received: " + response.StatusCode);
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
                    tilesCache = tilescache,
                }
            }) as FUpdate;

            if (data.items != null)
            {
                var config = data.items.Find(i => i?.GetType() == typeof(FConfig)) as FConfig;
                if (config != null) this.BuildConfigHash(config);
            }
            return data;
        }

        public object UseBuilding(double clientLat, double clientLng, string buildingId, double buildingLat, double buildingLng)
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

