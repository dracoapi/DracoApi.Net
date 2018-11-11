using DracoLib.Core.Exceptions;
using DracoLib.Core.Extensions;
using DracoLib.Core.Providers;
using DracoLib.Core.Text;
using DracoProtos.Core.Base;
using DracoProtos.Core.Extensions;
using DracoProtos.Core.Objects;
using DracoProtos.Core.Serializer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
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

    public class DracoClient
    {
        public FClientInfo ClientInfo { get; private set; }
        public User User { get; private set; }
        public Fight Fight { get; private set; }
        public Inventory Inventory { get; private set; }
        public Eggs Eggs { get; private set; }
        public Creatures Creatures { get; private set; }
        public Battle Battle { get; private set; }
        public string ProtocolVersion { get; private set; }
        public string ClientVersion { get; private set; }
        public Strings Strings { get; private set; }
        public FConfig FConfig { get; private set; }

        private RestRequest Request { get; set; }
        private IWebProxy Proxy { get; set; }
        private string Dcportal { get; set; }
        private bool CheckProtocol { get; set; }
        private Auth Auth { get; set; }
        private sbyte[] ConfigHash { get; set; }
        private Dictionary<string, int> EventsCounter { get; set; } = new Dictionary<string, int>();
        private readonly SerializerContext serializer;
        private RestClient client;
        private readonly Semaphore _rpcQueue = new Semaphore(1, 1);

        internal int UtcOffset;
        internal Config Config { get; private set; }
        internal readonly AuthService auth = new AuthService();
        internal readonly MapService map = new MapService();
        internal readonly PlayerService player = new PlayerService();
        internal readonly UserCreatureService userCreature = new UserCreatureService();
        internal readonly BattleService battle = new BattleService();
        internal readonly DevModeService devMode = new DevModeService();
        internal readonly MagicService magic = new MagicService();
        internal readonly ContestMapService contest = new ContestMapService();
        internal readonly GamePlayService gamePlay = new GamePlayService();
        internal readonly EncounterService encounter = new EncounterService();
        internal readonly ItemService Item = new ItemService();
        internal readonly ClientEventService clientEvent = new ClientEventService();
        //internal readonly List<RequestListener> _listeners = new List<RequestListener>();

        //Others
        internal long TimeServer { get; private set; }

        public DracoClient(IWebProxy proxy = null, Config config = null)
        {
            this.Config = config ?? new Config();

            this.ProtocolVersion = FGameObjects.ProtocolVersion.ToString();
            this.ClientVersion = "12709";
            if (this.Config.CheckProtocol) this.CheckProtocol = this.Config.CheckProtocol;
            if (this.Config.EventsCounter.Any()) this.EventsCounter = this.Config.EventsCounter;
            if (this.Config.UtcOffset > 0)
            {
                this.UtcOffset = this.Config.UtcOffset;
            }
            else
            {
                this.UtcOffset = (int)TimeZoneInfo.Utc.GetUtcOffset(DateTime.Now).TotalSeconds * 60;
            }

            this.Proxy = proxy;
            int timeout = 20 * 1000;
            if (this.Config.TimeOut > 0)
            {
                timeout = this.Config.TimeOut;
            }

            this.serializer = new SerializerContext("portal", FGameObjects.CLASSES, FGameObjects.ProtocolVersion);

            if (this.Proxy != null)
                this.client.Proxy = this.Proxy;

            this.client = new RestClient("https://us.draconiusgo.com");
            this.client.ClearHandlers();
            this.client.AddDefaultHeader("X-Unity-Version", "2017.1.3f1");
            this.client.AddDefaultHeader("Accept", "*/*");
            this.client.AddDefaultHeader("Protocol-Version", this.ProtocolVersion);
            this.client.AddDefaultHeader("Client-Version", this.ClientVersion);
            this.client.AddDefaultHeader("Accept-Language", "en-us");
            this.client.UserAgent = $"DraconiusGO/{this.ClientVersion} CFNetwork/975.0.3 Darwin/18.2.0";
            this.client.CookieContainer = new CookieContainer();
            //this.client.Encoding = null;
            this.client.Timeout = timeout;

            this.ClientInfo = new FClientInfo
            {
                deviceModel = "iPhone9,3",
                iOsAdvertisingTrackingEnabled = false,
                language = this.Config.Lang,
                platform = "IPhonePlayer",
                platformVersion = "iOS 12.1.1",
                revision = this.ClientVersion,
                screenHeight = 1334,
                screenWidth = 750,
            };

            this.User = new User();
            this.Fight = new Fight(this);
            this.Inventory = new Inventory(this);
            this.Eggs = new Eggs(this);
            this.Creatures = new Creatures(this);
            this.Battle = new Battle(this);
            this.Strings = new Strings(config.Lang, this);
        }

        public float GetAccuracy()
        {
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

        public T Call<T>(Async<T> request)
        {
            try
            {
                _rpcQueue.WaitOne();

                var service = request.GetType().GetField("service", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(request);
                var method = request.GetType().GetField("methodName", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(request);
                var body = request.GetType().GetField("args", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(request);

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
                if (protocolVersion != this.ProtocolVersion && this.CheckProtocol)
                {
                    throw new DracoError("Incorrect protocol version received: " + protocolVersion);
                }

                var dcportal = response.Headers.ToList().Find(x => x.Name == "dcportal").Value.ToString();
                if (dcportal != null) this.Dcportal = dcportal;

                var data = this.serializer.Deserialize(response.RawBytes);
                (data ?? string.Empty).ToString();
                return (T)data;
            }
            catch (Exception)
            {
                // return empty;
                object err = string.Empty;
                return (T)err;
            }
            finally
            {
                _rpcQueue.Release();
            }
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

            this.Call(new ClientEventService().OnEventWithCounter(
                name,
                this.User.Id,
                this.ClientInfo,
                eventCounter,
                one,
                two,
                three,
                null,
                null
           ));
            this.EventsCounter[name] = eventCounter + 1;
        }

        public FConfig Boot(User userinfo)
        {
            this.User.Id = userinfo.Id;
            this.User.DeviceId = userinfo.DeviceId;
            this.User.Login = (userinfo.Login ?? "GOOGLE").ToUpper();
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
            FConfig = this.Call(auth.GetConfig(this.ClientInfo.language));
            this.BuildConfigHash(FConfig);
            return FConfig;
        }

        private sbyte[] BuildConfigHash(FConfig config)
        {
            FConfig = config;
            this.ConfigHash = FConfig.GetMd5HashAsSbyte(config);
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

                //return DevSingIn(this.User.Nickname, true, true);
                throw new DracoError("Device login not implemented.");
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
                throw new DracoError("Unsupported login type: " + this.User.Login);
            }

            //this.Event("TrySingIn", this.Auth.Name);
            var response = this.Call(auth.TrySingIn(
                new AuthData
                {
                    authType = this.Auth.Type,
                    profileId = this.Auth.ProfileId,
                    tokenId = this.Auth.TokenId
                },
                this.ClientInfo,
                new FRegistrationInfo(this.Auth.Reg)
                {
                    email = this.User.Username
                }
            ));

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

                this.Auth.TokenId = login["Auth"];

                var token = JsonConvert.DeserializeObject<JObject>(new CustomJsonWebToken().Decode(this.Auth.TokenId, null, false));

                if (token == null)
                    throw new DracoError("Unable to get the token.");

                bool verified = (bool)token["email_verified"];

                if (!verified)
                    throw new DracoError("You mail is not verified, please verify this before.");

                string sub = (string)token["sub"];

                if (string.IsNullOrEmpty(sub))
                    throw new DracoError("You mail is not verified, please verify this before.");

                this.Auth.ProfileId = sub;
            });
        }

        public void Load()
        {
            if (this.User.Avatar == 0) throw new DracoError("Please login first.");

            // this.Event("LoadingScreenPercent", "100");
            // this.Event("CreateAvatarByType", "MageMale");
            // this.Event("LoadingScreenPercent", "100");
            // this.Event("AvatarUpdateView", this.user.avatar.toString());
            // this.Event("InitPushNotifications", "False");
        }

        public FAuthData DevSingIn(string login, bool validateNickname, bool asDevice)
        {
            return this.Call(auth.DevSingIn(login, validateNickname, asDevice));
        }

        public FNicknameValidationResult ValidateNickName(string nickname, bool takeSuggested = true)
        {
            //this.Event("ValidateNickname", nickname);
            var result = this.Call(auth.ValidateNickname(nickname));

            if (result == null) return result;

            if (result.error == FNicknameValidationError.DUPLICATE)
            {
                //this.Event("ValidateNicknameError", "DUPLICATE");
                if (takeSuggested)
                {
                    return ValidateNickName(result.suggestedNickname, true);
                }

                return result;
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

        public FUserInfo AcceptLicence(int licence)
        {
            return this.Call(auth.AcceptLicence(licence));
        }

        public FAuthData Register(string nickname)
        {
            this.User.Nickname = nickname;
            //this.Event("Register", this.Auth.Name, nickname);
            var data = this.Call(auth.Register(
                new AuthData
                {
                    authType = this.Auth.Type,
                    profileId = this.Auth.ProfileId,
                    tokenId = this.Auth.TokenId
                },
                nickname,
                this.ClientInfo,
                new FRegistrationInfo(this.Auth.Reg)
                {
                    email = this.User.Username
                }
            ));

            this.User.Id = data.info.userId;

            //this.Event("ServerAuthSuccess", this.User.Id);

            return data;
        }

        public FNewsArticle GetNews(string lastSeen)
        {
            return this.Call(auth.GetNews(this.ClientInfo.language, lastSeen));
        }

        public FNewsArticle GetOffers(string seenNews)
        {
            return this.Call(auth.GetOffers(this.ClientInfo.language, seenNews));
        }

        public FTips GetTips()
        {
            return this.Call(auth.GetTips(this.ClientInfo.language));
        }

        public FAuthData LinkTo(AuthData authData, FClientInfo clientInfo, FRegistrationInfo regInfo, bool force)
        {
            return this.Call(auth.LinkTo(authData, clientInfo, regInfo, force));
        }

        public FTips MarkTip(bool value)
        {
            return this.Call(auth.MarkTip(this.ClientInfo.language, value));
        }

        public object SetAvatar(int avatar)
        {
            this.User.Avatar = avatar;
            //this.Event("AvatarPlayerGenderRace", "1", "1");
            //this.Event("AvatarPlayerSubmit", avatar.ToString());
            return this.Call(player.SaveUserSettings(this.User.Avatar));
        }

        public FUpdate SelectAlliance(AllianceType alliance, int bonus)
        {
            return this.Call(player.SelectAlliance(alliance, bonus));
        }

        public FAvaUpdate SelectBuddy(string creatureid)
        {
            return this.Call(player.SelectBuddy(creatureid));
        }

        public object AcknowledgeNotification(string type)
        {
            return this.Call(player.AcknowledgeNotification(type));
        }

        public FUpdate GetMapUpdate(double latitude, double longitude, float horizontalAccuracy = 0, Dictionary<FTile, long> tilescache = null)
        {
            horizontalAccuracy = horizontalAccuracy > 0 ? horizontalAccuracy : this.GetAccuracy();
            tilescache = tilescache ?? new Dictionary<FTile, long>() { };
            long _time_sever = this.TimeServer <= 0 ?  0 : this.TimeServer;

            var data = this.Call(map.GetUpdate(new FUpdateRequest()
            {
                clientRequest = new FClientRequest()
                {
                    time = 0, // _time_sever,
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
            }));

            if (data == null)
                throw new DracoError("Null error no data.");

            this.TimeServer = data.serverTime;

            if (data.items != null)
            {
                var config = data.items.Find(i => i?.GetType() == typeof(FConfig)) as FConfig;
                if (config != null) this.BuildConfigHash(config);
            }
            return data;
        }

        public FUpdate TryUseBuilding(double clientLat, double clientLng, string buildingId, double buildingLat, double buildingLng, string dungeonId, float horizontalAccuracy = 0)
        {
            horizontalAccuracy = horizontalAccuracy > 0 ? horizontalAccuracy : this.GetAccuracy();
            long _time_sever = this.TimeServer <= 0 ? 0 : this.TimeServer;

            FUpdate fUpdate = this.Call(map.TryUseBuilding(new FClientRequest
            {
                time = 0, // _time_sever,
                currentUtcOffsetSeconds = this.UtcOffset,
                coordsWithAccuracy = new GeoCoordsWithAccuracy
                {
                    latitude = clientLat,
                    longitude = clientLng,
                    horizontalAccuracy = horizontalAccuracy
                },
            },
                new FBuildingRequest(buildingId, new GeoCoords { latitude = buildingLat, longitude = buildingLng }, dungeonId)
            ));

            this.TimeServer = fUpdate.serverTime;
            return fUpdate;
        }

        public FOpenChestResult OpenChest(FChest chest)
        {
            this.Call(map.StartOpeningChest(chest));
            return this.Call(map.OpenChestResult(chest));
        }

        public FUpdate LeaveDungeon(double latitude, double longitude, float horizontalAccuracy = 0)
        {
            horizontalAccuracy = horizontalAccuracy > 0 ? horizontalAccuracy : this.GetAccuracy();
            long _time_sever = this.TimeServer <= 0 ? 0 : this.TimeServer;

            FUpdate fUpdate = this.Call(map.LeaveDungeon(new FClientRequest
            {
                time = 0, //_time_sever,
                currentUtcOffsetSeconds = this.UtcOffset,
                coordsWithAccuracy = new GeoCoordsWithAccuracy
                {
                    latitude = latitude,
                    longitude = longitude,
                    horizontalAccuracy = horizontalAccuracy,
                },
            }));

            this.TimeServer = fUpdate.serverTime;
            return fUpdate;
        }

        public FCatchingCreature FeedCreature(string creatureId, ItemType item, Tile tile)
        {
            return this.Call(gamePlay.FeedCreature(creatureId, item, tile));
        }

        public void Dispose()
        {
            Dispose(true);
        }

        internal void Dispose(bool disposing)
        {
            if (!disposing) return;
            _rpcQueue?.Dispose();
        }
    }
}
